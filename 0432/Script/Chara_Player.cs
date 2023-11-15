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

    [field: SerializeField] public float score { get; private set; }
    [SerializeField] private Parameter dashSpeed;
    [SerializeField] private PlayerInput input;
    [SerializeField] private CircleClamp norCircle;
    [SerializeField] private SmoothRotate smooth;

    [SerializeField, NonEditable] private Vector2 beforeinputVelocity;
    [SerializeField, NonEditable] private bool inputting;   // 移動の入力
    [SerializeField, NonEditable] private bool run;         // 走り入力
    [SerializeField, NonEditable] private bool rigor;       // 硬直状態（入力を受け付けない）
    [SerializeField, NonEditable] private Vector3 dirrection;
    [SerializeField] private Animator animator;
    [SerializeField, NonEditable] private CharaState motionState;
    private float velocitySum;


    [SerializeField] private MotionWithCollider _attack1;
    private List<Motion> interruptByDamageMotions = new List<Motion>(); // 被弾モーションに割り込まれるモーションを登録
    [SerializeField] private Motion damage;
    [SerializeField] private Motion death;


    void Awake()
    {

    }
    protected override void Start()
    {
        score = 0.0f;
        gameObject.tag = gameObject.transform.parent.tag;
        beforeinputVelocity = Vector2.zero;
        norCircle.Initialize();
        smooth.Initialize(gameObject);
        input = GetComponent<PlayerInput>();
        base.Start();
        dashSpeed.Initialize();
        underAttackAction += Damage;

        //  Motionの設定

        _attack1.Initialize(animator, Anims.attack1);
        _attack1.enableAction += () => StateChange(CharaState.Attack);
        _attack1.endAction += () => StateChange(CharaState.None);
        _attack1.endAction += () => rigor = false;

        damage.Initialize(animator, Anims.damege);
        damage.startAction += () => Debug.Log("Damage");
        damage.startAction += () => inputMotionReset();
        damage.startAction += () => StateChange(CharaState.Damage, true);
        damage.startAction += () => animator.Play(Anims.damege, 0, 0.0f);       // 連続で再生できる
        damage.enableAction += () => StateChange(CharaState.Damage, true);
        damage.endAction += () => StateChange(CharaState.None);
        damage.endAction += () => rigor = false;

        interruptByDamageMotions.Add(_attack1.motion);
        interruptByDamageMotions.Add(damage);

        death.Initialize(animator, Anims.die);
        death.startAction += () => StateChange(CharaState.Death);
        death.enableAction += () => StateChange(CharaState.Death);

        invincible.Initialize(true,false);
    }

    /// <summary>
    /// 前提処理<br/>
    /// Updateの最初に行われる
    /// </summary>
    public void Reset()
    {
        dashSpeed.Update();
        invincible.Update();
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
                death.LaunchOneShot();
            }
        }
    }
    protected override void Update()
    {
        Reset();
        base.Update();

        _attack1.Update();
        death.Update();
        damage.Update();


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
            if(m.exist.state != ExistState.Disable)
            {
                m.cutIn?.Invoke();
                if(m != interruptByDamageMotions[1])    // 実行中のモーションなら
                {

                    m.Reset();
                }
            }
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

    public void Damage(UnderAttackType type)
    {
        if(type == UnderAttackType.Normal)
        {
            damage.Launch();
            invincible.Reset();

        }
    }

    public void AddScore(float score)
    {
        this.score += score;
    }

    /// <summary>
    /// 倒した側が行う<br/>
    /// 引数は倒されたプレイヤー
    /// </summary>
    /// <param name="you"></param>
    public void ChangeScoreByKill(Chara_Player you)
    {
        if(you.score < 0)
        {
            AddScore(you.score / 2);
            you.score /= 2;
            Debug.Log(this.score);
            Debug.Log(you.score);
        }
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
        _attack1.Launch(50, 3);
    }


    public void OnDamage(InputValue value)
    {
        UnderAttack(50, UnderAttackType.Normal);

    }
    #endregion


}

