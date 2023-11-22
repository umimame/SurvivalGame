using My;
using System;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.InputSystem;

public class Chara_Player : Chara
{
    public enum MotionState
    {
        Idle,
        Walk,
        Run,
        Attack,
        Damage,
        Death,
        None,
    }

    [field: SerializeField] public int score { get; private set; }
    [field: SerializeField] public Parameter stamina { get; private set; }
    [SerializeField] private Parameter dashSpeed;
    [field: SerializeField] public float dashCost { get; private set; }
    [field: SerializeField] public Interval overStamina {  get; private set; }
    [SerializeField] private PlayerInput input;
    [SerializeField] private CircleClamp norCircle;
    [SerializeField] private SmoothRotate smooth;

    [SerializeField, NonEditable] private Vector2 beforeinputVelocity;
    [SerializeField, NonEditable] private bool inputting;   // 移動の入力
    [SerializeField, NonEditable] private bool run;         // 走り入力
    [SerializeField, NonEditable] private bool rigor;       // 硬直状態（入力を受け付けない）
    [SerializeField, NonEditable] private Vector3 dirrection;
    [SerializeField] private Animator animator;
    [SerializeField, NonEditable] private MotionState motionState;
    private float velocitySum;


    [SerializeField] private MotionWithCollider _attack1;
    private List<Motion> interruptByDamageMotions = new List<Motion>(); // 被弾モーションに割り込まれるモーションを登録
    [SerializeField] private Motion damage;
    [SerializeField] private Motion death;
    

    void Awake()
    {

    }

    protected override void Spawn()
    {
        base.Spawn();

        hp.Initialize();
        speed.Initialize();
        pow.Initialize();
        stamina.Initialize();
        overStamina.Initialize(true, false);
        dashSpeed.Initialize();

        StateChange(MotionState.Idle);
        alive = true;
    }

    protected override void Start()
    {
        score = 0;
        stamina.Initialize();
        overStamina.Initialize(true, false);
        overStamina.activeAction += stamina.Update;
        dashSpeed.Initialize();

        gameObject.tag = gameObject.transform.parent.tag;
        beforeinputVelocity = Vector2.zero;
        norCircle.Initialize();
        smooth.Initialize(gameObject);
        input = GetComponent<PlayerInput>();
        base.Start();

        underAttackAction += Damage;

        //  Motionの設定

        _attack1.Initialize(animator, Anims.attack1, this);
        _attack1.enableAction += () => StateChange(MotionState.Attack);
        _attack1.endAction += () => StateChange(MotionState.None);
        _attack1.endAction += () => rigor = false;

        damage.Initialize(animator, Anims.damege);
        damage.startAction += () => Debug.Log("Damage");
        damage.startAction += () => InputMotionReset();
        damage.startAction += () => StateChange(MotionState.Damage, true);
        damage.startAction += () => animator.Play(Anims.damege, 0, 0.0f);       // 連続で再生できる
        damage.enableAction += () => StateChange(MotionState.Damage, true);
        damage.endAction += () => StateChange(MotionState.None);
        damage.endAction += () => rigor = false;

        interruptByDamageMotions.Add(_attack1.motion);
        interruptByDamageMotions.Add(damage);

        death.Initialize(animator, Anims.die);
        death.startAction += () => StateChange(MotionState.Death);
        death.startAction += () => StateChange(CharaState.Death);
        death.startAction += ChangeScoreByKill;
        death.enableAction += () => StateChange(MotionState.Death);
        death.startAction += () => StateChange(CharaState.Death);

        invincible.Initialize(true,false);

    }

    /// <summary>
    /// 前提処理<br/>
    /// Updateの最初に行われる
    /// </summary>
    protected override void Reset()
    {
        dashSpeed.Update();
        invincible.Update();

        stamina.ReturnRange();
        if(stamina.overZero == true) {
            if(overStamina.active == true)
            {
                overStamina.Reset();
            }
        }

        animator.speed = 1;                                                     // アニメーションスピードのリセット
        if (hp.entity <= 0) { alive = false; }                                  // 生存boolのリセット
        inputting = (inputMoveVelocity.entity != Vector2.zero) ? true : false;  // 入力boolのリセット
        rigor = false;                                                          // 硬直boolのリセット

        if (alive == true)
        {
            if (rigor == false)
            {
                if (inputting == false)
                {
                    StateChange(MotionState.Idle);
                }
                else
                {
                    if (run == true && velocitySum >= 1)
                    {
                        if(stamina.overZero == false) { StateChange(MotionState.Run); }

                        else
                        {
                            StateChange(MotionState.Walk);
                        }
                    }
                    else 
                    {
                        StateChange(MotionState.Walk);
                    }
                }
            }
        }
        else
        {
            if(motionState != MotionState.Damage)
            {
                death.LaunchOneShot();
            }
        }
    }

    public void IdleUpdate()    // 動ける状態なら
    {
        overStamina.Update();

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
            case MotionState.Idle:
                IdleUpdate();

                DirrectionManager();
                break;

            case MotionState.Walk:
                IdleUpdate();

                animator.speed = velocitySum;   // 歩きモーションのスピードをスティックに応じて変える
                assignSpeed = speed.entity;

                DirrectionManager();
                break;
            case MotionState.Run:
                IdleUpdate();

                stamina.Update(-dashCost);
                velocitySum = 1;
                assignSpeed = dashSpeed.entity;

                DirrectionManager();
                break;

            case MotionState.Attack:
                rigor = true;
                break;

            case MotionState.Damage:
                IdleUpdate();
                rigor = true;
                break;

            case MotionState.Death:
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

        Vector3 addPos; 
        Vector3 newPos;
        if (inputting == true)  // 入力されていれば
        {                       // 向きを制御
            norCircle.AdjustByCenter();
            addPos.x = norCircle.moveObject.transform.position.x + beforeinputVelocity.normalized.x;
            addPos.z = norCircle.moveObject.transform.position.z + beforeinputVelocity.y;
            addPos = new Vector3(beforeinputVelocity.x, transform.position.y, beforeinputVelocity.y);
            newPos = norCircle.moveObject.transform.position + (addPos.normalized * norCircle.radius);
            norCircle.moveObject.transform.position = newPos;
            dirrection = new Vector3(norCircle.moveObject.transform.position.x - gameObject.transform.position.x, 0, norCircle.moveObject.transform.position.z - gameObject.transform.position.z).normalized;

            beforeinputVelocity = inputMoveVelocity.entity;
        }

        norCircle.moveObject.transform.position = new Vector3(norCircle.moveObject.transform.position.x, gameObject.transform.transform.position.y, norCircle.moveObject.transform.position.z);

        smooth.Update(dirrection);  
        
        norCircle.Limit();
    }

    public void InputMotionReset()
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

    public void StateChange(MotionState m, bool affectAlive = false)
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

    public void AddScore(int score)
    {
        this.score += score;
    }




    /// <summary>
    /// 倒された側が行う<br/>
    /// 最後に攻撃を与えたプレイヤーが得点を得る
    /// </summary>
    /// <param name="you"></param>
    public void ChangeScoreByKill()
    {
        Debug.Log("Death");
        if(lastAttacker == null)
        {
            Debug.Log("自滅");
        }

        if(score > 0)
        {
            lastAttacker.AddScore(score / 2);
            score /= 2;
            Debug.Log(this.score);
            Debug.Log(lastAttacker.score);
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

