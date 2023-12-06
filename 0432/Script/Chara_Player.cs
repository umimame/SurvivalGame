using AddClass;
using GenericChara;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Chara_Player : Chara
{
    public enum MotionState
    {
        Idle,
        Walk,
        Run,
        Attack1,
        Attack2,
        Damage,
        Death,
        None,
    }

    [SerializeField] private PlayerInput input;
    [field: SerializeField] public int score { get; private set; }
    [field: SerializeField] public Parameter stamina { get; private set; }
    [SerializeField] private Parameter dashSpeed;
    [SerializeField] private Curve dashEasing = new Curve();
    [field: SerializeField] public float dashCost { get; private set; }
    [field: SerializeField] public Interval overStamina {  get; private set; }

    [SerializeField] private Camera cam;
    [SerializeField] private FPSViewPoint viewPointManager;

    [SerializeField, NonEditable] private EntityAndPlan<Vector2> inputMoveVelocity;
    [SerializeField, NonEditable] private bool moveInputting;       // 移動の入力
    [SerializeField, NonEditable] private bool run;         // 走り入力
    [SerializeField, NonEditable] private bool rigor;       // 硬直状態（入力を受け付けない）
    [SerializeField, NonEditable] private Vector3 dirrection;

    [SerializeField] private Animator animator;
    [field: SerializeField, NonEditable] public MotionState motionState;
    private float velocitySum;

    
    [SerializeField] private MotionWithCollider attack1 = new MotionWithCollider();
    [SerializeField] private MotionWithCollider attack2 = new MotionWithCollider();
    private List<Motion> interruptByDamageMotions = new List<Motion>(); // 被弾モーションに割り込まれるモーションを登録
    [SerializeField] private Motion damage = new Motion();
    [SerializeField] private Motion death = new Motion();
    

    void Awake()
    {

    }

    protected override void Spawn()
    {
        base.Spawn();

        hp.Initialize();
        speed.Initialize();
        pow.Initialize();
        dashSpeed.Initialize();
        stamina.Initialize();
        overStamina.Initialize(true, false);

        death.exist.OneShotReset();

        StateChange(MotionState.Idle);
        alive = true;
    }

    protected override void Start()
    {
        score = 0;
        stamina.Initialize();
        overStamina.Initialize(true, false);
        overStamina.activeAction += stamina.Update;

        gameObject.tag = gameObject.transform.parent.tag;
        //viewCircle.Initialize(gameObject);
        input = GetComponent<PlayerInput>();
        base.Start();

        underAttackAction += Damage;

        //  Motionの設定
        attack1.Initialize(animator, Anims.attack1, this);
        attack1.enableAction += () => StateChange(MotionState.Attack1);
        attack1.endAction += () => StateChange(MotionState.Idle);
        attack1.endAction += () => rigor = false;

        attack2.Initialize(animator, Anims.attack2, this);
        attack2.enableAction += () => StateChange(MotionState.Attack2);
        attack2.endAction += () => StateChange(MotionState.Idle);
        attack2.endAction += () => rigor = false;

        damage.Initialize(animator, Anims.damege);
        damage.startAction += () => Debug.Log("Damage");
        damage.startAction += () => InputMotionReset();
        damage.startAction += () => StateChange(MotionState.Damage, true);
        damage.startAction += () => animator.Play(Anims.damege, 0, 0.0f);       // 連続で再生できる
        damage.enableAction += () => StateChange(MotionState.Damage, true);
        damage.endAction += () => StateChange(MotionState.Idle);
        damage.endAction += () => rigor = false;

        interruptByDamageMotions.Add(attack1.motion);
        interruptByDamageMotions.Add(attack2.motion);
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
    public void InitialUpdate()
    {
        invincible.Update();

        stamina.ReturnRange();
        if(stamina.overZero == true) {
            if(overStamina.active == true)
            {
                overStamina.Reset();
            }
        }

        animator.speed = 1;                                                             // アニメーションスピードのリセット
        if (hp.entity <= 0) { alive = false; }                                          // 生存boolのリセット
        moveInputting = (inputMoveVelocity.entity != Vector2.zero) ? true : false;      // 入力boolのリセット
        rigor = false;                                                                  // 硬直boolのリセット
        if (motionState != MotionState.Run) { dashEasing.Clear(); }                     // Curveのリセット

        if (alive == true)
        {
            if (rigor == false)
            {
                if (moveInputting == false)
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

    public void InputMoveUpdate()    // 動ける状態なら
    {
        overStamina.Update();

    }
    protected override void Update()
    {
        InitialUpdate();
        base.Update();
        MotionUpdate();

        viewPointManager.LookAtViewPoint(transform, true, false, true); // 高さは本体を中心にする

        Vector3 addVelo = Vector3.zero;
        addVelo.x = AddFunction.GetFPSMoveVec2(cam, inputMoveVelocity.plan).x;
        addVelo.z = AddFunction.GetFPSMoveVec2(cam, inputMoveVelocity.plan).y;
        AddAssignedMoveVelocity(addVelo);

        if (velocitySum > 1) { velocitySum = 1; }
        switch (motionState)
        {
            case MotionState.Idle:
                InputMoveUpdate();

                break;

            case MotionState.Walk:
                InputMoveUpdate();

                animator.speed = velocitySum;   // 歩きモーションのスピードをスティックに応じて変える
                assignSpeed = speed.entity;

                break;
            case MotionState.Run:
                InputMoveUpdate();

                stamina.Update(-dashCost);
                velocitySum = 1;
                animator.speed = dashEasing.currentValue;   // 走りモーションのスピードを加速度に応じて変える
                assignSpeed = dashEasing.Update() * dashSpeed.entity;

                break;

            case MotionState.Attack1:
                rigor = true;
                break;

            case MotionState.Attack2:
                rigor = true;
                break;

            case MotionState.Damage:
                InputMoveUpdate();
                rigor = true;
                break;

            case MotionState.Death:
                rigor = true;
                break;
        }
        animator.SetInteger(Anims.AnimIdx, (int)motionState);


        RigorReset();
    }

    public void MotionUpdate()
    {

        attack1.Update();
        attack2.Update();
        death.Update();
        damage.Update();
    }

    /// <summary>
    /// 硬直状態(rigor == true)の時に行われる
    /// </summary>
    public void RigorReset()
    {

        if (rigor == true)
        {
            inputMoveVelocity.plan = Vector2.zero;
           viewPointManager.inputViewPoint.plan = Vector2.zero;
        }
        else
        {
            inputMoveVelocity.Assign();  
           viewPointManager.inputViewPoint.Assign();

        }
    }


    public void InputMotionReset()
    {
        foreach(Motion m in interruptByDamageMotions)
        {
            if(m.exist.state != ExistState.Disable)
            {
                m.cutIn?.Invoke();
                if(m != interruptByDamageMotions[interruptByDamageMotions.Count - 1])    // 実行中のモーションなら
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
            Chara_Player _lastAttacker = (Chara_Player)lastAttacker;
            _lastAttacker.AddScore(score / 2);
            score /= 2;
            Debug.Log(this.score);
            Debug.Log(_lastAttacker.score);
        }
    }

    #region PlayerInputに自動で登録されるEvent

    public void OnMove(InputValue value)
    {
        inputMoveVelocity.entity = value.Get<Vector2>();
        velocitySum = Mathf.Abs(value.Get<Vector2>().x) + Mathf.Abs(value.Get<Vector2>().y);

        
    }

    public void OnInputViewPoint(InputValue value)
    {
        viewPointManager.inputViewPoint.entity = value.Get<Vector2>();
    }

    public void OnRunning(InputValue value)
    {
        run = Convert.ToBoolean(value.Get<float>());
    }

    public void OnAttack1(InputValue value)
    {
        if(rigor == true) { return; }
        attack1.Launch(50, 3);
    }

    public void OnAttack2(InputValue value)
    {
        if (rigor == true) { return; }
        attack2.Launch(100, 1);
    }


    public void OnDamage(InputValue value)
    {
        UnderAttack(50, UnderAttackType.Normal);

    }
    #endregion


}

