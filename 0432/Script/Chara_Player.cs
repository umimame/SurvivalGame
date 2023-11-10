using My;
using System;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.InputSystem;

public class Chara_Player : Chara
{
    public enum CharaState
    {
        Idle,
        Walk,
        Run,
        Attack,
        Damage,
        Death,
        None,
    }

    [SerializeField] private Parameter dashSpeed;
    [SerializeField] private PlayerInput input;
    [SerializeField] private CircleClamp norCircle;
    [SerializeField] private SmoothRotate smooth;

    [SerializeField] private Vector2 beforeinputVelocity;
    [SerializeField, NonEditable] private bool inputting;   // 移動の入力
    [SerializeField, NonEditable] private bool run;         // 走り入力
    [SerializeField, NonEditable] private bool rigor;       // 硬直状態（入力を受け付けない）
    [SerializeField] private Vector3 dirrection;
    [SerializeField] private Animator animator;
    [SerializeField] private CharaState motionState;
    private float velocitySum;
    [SerializeField] private Motion attack1;
    private List<Motion> interruptByDamageMotions = new List<Motion>(); // 被弾モーションに割り込まれるモーションを登録
    [SerializeField] private Motion damage;
    [SerializeField] private Motion death;

    void Awake()
    {

    }
    protected override void Start()
    {
        gameObject.tag = gameObject.transform.parent.tag;
        beforeinputVelocity = Vector2.zero;
        norCircle.Initialize();
        smooth.Initialize(gameObject);
        input = GetComponent<PlayerInput>();
        base.Start();
        dashSpeed.Initialize();

        //  Motionの設定
        attack1.Initialize(animator, Anims.attack1);
        attack1.enableAction += () => StateChange(CharaState.Attack);  // stateを変えるラムダ式
        attack1.endAction += () => StateChange(CharaState.None);  
        attack1.endAction += () => rigor = false;
        attack1.inThreshold += () => Debug.Log("Attack");
        attack1.outThreshold += () => Debug.Log("Breach");

        damage.Initialize(animator, Anims.damege);
        damage.startAction += () => inputMotionReset();
        damage.startAction += () => StateChange(CharaState.Damage, true);
        damage.startAction += () => animator.Play(Anims.damege, 0, 0.0f);       // 連続で再生できる
        damage.enableAction += () => StateChange(CharaState.Damage, true);
        damage.endAction += () => StateChange(CharaState.None);
        damage.endAction += () => rigor = false;

        interruptByDamageMotions.Add(attack1);
        interruptByDamageMotions.Add(damage);

        death.Initialize(animator, Anims.die);
        death.startAction += () => StateChange(CharaState.Death);
        death.enableAction += () => StateChange(CharaState.Death);

    }

    /// <summary>
    /// 前提処理<br/>
    /// Updateの最初に行われる
    /// </summary>
    public void Reset()
    {
        dashSpeed.Update();
        animator.speed = 1;
        if (hp.entity <= 0) { alive = false; }
        inputting = (inputMoveVelocity.entity != Vector2.zero) ? true : false;
        rigor = false;

        if (alive == true)
        {
            if (rigor == false)
            {
                if (inputting == false)
                {
                    StateChange(CharaState.Idle);
                }
                else
                {
                    if (run == true && velocitySum >= 1)
                    {
                        StateChange(CharaState.Run);

                    }
                    else 
                    {
                        StateChange(CharaState.Walk);
                    }
                }
            }
        }
        else
        {
            if(motionState != CharaState.Damage)
            {
                death.StartOneShot();
            }
        }
    }
    protected override void Update()
    {
        Reset();
        base.Update();

        attack1.Update();
        damage.Update();
        death.Update();

        if(velocitySum > 1) { velocitySum = 1; }
        switch (motionState)
        {
            case CharaState.Idle:
                DirrectionManager();
                break;

            case CharaState.Walk:
                animator.speed = velocitySum;   // 歩きモーションのスピードをスティックに応じて変える
                assignSpeed = speed.entity;

                DirrectionManager();
                break;
            case CharaState.Run:
                velocitySum = 1;
                assignSpeed = dashSpeed.entity;

                DirrectionManager();
                break;

            case CharaState.Attack:
                rigor = true;
                break;

            case CharaState.Damage:
                rigor = true;
                break;

            case CharaState.Death:
                rigor = true;
                break;
        }
        animator.SetInteger(Anims.AnimIdx, (int)motionState);


        RigorReset();
    }



    /// <summary>
    /// 硬直状態(rigor == true)の時に行われる
    /// </summary>
    public void RigorReset()
    {

        if (rigor == true)
        {
            inputMoveVelocity.plan = Vector2.zero;
        }
        else
        {
            inputMoveVelocity.Assign();
        }
    }

    public void DirrectionManager()
    {

        if (inputting == true)  // 入力されていれば
        {                       // 向きを制御
            norCircle.AdjustByCenter();
            Vector3 addPos;
            addPos.x = norCircle.moveObject.transform.position.x + beforeinputVelocity.normalized.x;
            addPos.y = transform.position.y;
            addPos.z = norCircle.moveObject.transform.position.z + beforeinputVelocity.y;
            addPos = new Vector3(beforeinputVelocity.x, transform.position.y, beforeinputVelocity.y);
            Vector3 newPos = norCircle.moveObject.transform.position + (addPos.normalized * norCircle.radius);
            norCircle.moveObject.transform.position = newPos;
            dirrection = (norCircle.moveObject.transform.position - gameObject.transform.position).normalized;

            beforeinputVelocity = inputMoveVelocity.entity;
        }
        if(dirrection != Vector3.zero) { smooth.Update(dirrection); }   // 条件分岐を書かないとエラーが出る
        
        norCircle.Limit();
    }

    public void inputMotionReset()
    {
        foreach(Motion m in interruptByDamageMotions)
        {
            m.Reset();
        }
    }

    public void StateChange(CharaState m, bool affectAlive = false)
    {
        if (affectAlive == false)
        {
            motionState = m;
        }
        else
        {
            if(alive == true)
            {
                motionState = m;
            }
        }
    }

    /// <summary>
    /// 引数はダメージ量と被弾モーションを行うかどうか
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="damageMotion"></param>
    public void Damage(float damage, bool damageMotion = true)
    {
        if (alive == false) { return; }
        hp.entity -= damage;
        this.damage.Start();
    }

    #region PlayerInputに自動で登録されるEvent

    public void OnMove(InputValue value)
    {
        inputMoveVelocity.entity = value.Get<Vector2>();
        velocitySum = Mathf.Abs(value.Get<Vector2>().x) + Mathf.Abs(value.Get<Vector2>().y);

        
    }

    public void OnRunning(InputValue value)
    {
        run = Convert.ToBoolean(value.Get<float>());
    }

    public void OnAttack1(InputValue value)
    {
        if(rigor == true) { return; }
        attack1.Start();
    }


    public void OnDamage(InputValue value)
    {
        Damage(50, true);

    }
    #endregion


}

