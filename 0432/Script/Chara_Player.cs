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
    private Vector3 direction;
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

        attack1.Initialize(animator, Anims.attack1);
        attack1.enableAction += () => StateChange(CharaState.Attack);  // stateを変えるラムダ式
        attack1.endAction += () => StateChange(CharaState.Idle);  
        attack1.endAction += () => rigor = false;

        interruptByDamageMotions.Add(attack1);

        damage.Initialize(animator, Anims.damege);
        damage.startAction += () => Damage(50);
        damage.startAction += () => inputMotionReset();
        damage.startAction += () => StateChange(CharaState.Damage, true);
        damage.enableAction += () => StateChange(CharaState.Damage, true);
        damage.endAction += () => StateChange(CharaState.Idle);
        damage.endAction += () => rigor = false;

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
        inputting = (inputMoveVelocity.plan != Vector2.zero) ? true : false;
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
                    if (run == false) { StateChange(CharaState.Walk); }
                    else { StateChange(CharaState.Run); }
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
        switch (motionState)
        {
            case CharaState.Idle:

                break;

            case CharaState.Walk:
                animator.speed = velocitySum;   // 歩きモーションのスピードをスティックに応じて変える
                assignSpeed = speed.entity;

                break;
            case CharaState.Run:
                assignSpeed = dashSpeed.entity;

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
            direction = (norCircle.moveObject.transform.position - gameObject.transform.position).normalized;

            beforeinputVelocity = inputMoveVelocity.plan;
        }
        smooth.Update(direction);

        norCircle.Limit();

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

    public void inputMotionReset()
    {
        foreach(Motion m in interruptByDamageMotions)
        {
            m.exist.Initialize();
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

    public void Damage(float damage)
    {
        if (alive == false) { return; }
        hp.entity -= damage;
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

        attack1.Start();
    }


    public void OnDamage(InputValue value)
    {
        if(alive == false) { return; }
        damage.Start();

    }
    #endregion


}


[Serializable] public class Motion
{
    [SerializeField] private float motionTime;
    [SerializeField] private float adjustMotionTime;
    [field: SerializeField] public Interval interval { get; set; }

    [field: SerializeField] public Exist exist { get; set; }
    [SerializeField] private EasingAnimator easAnim;
    public void Initialize(Animator animator, string clipName)
    {
        motionTime = AddFunction.GetAnimationClipLength(animator, clipName);
        motionTime += adjustMotionTime;

        easAnim.Initialize(motionTime, animator);
        interval.Initialize(false, true, motionTime);

        exist.start += easAnim.Reset;

        exist.enable += interval.Update;
        exist.enable += easAnim.Update;
        interval.limitAction += exist.Finish;
    }

    public void Update()
    {
        exist.Update();
    }

    public void Start()
    {
        exist.Start();
    }
    public void StartOneShot()
    {
        exist.StartOneShot();
    }

    public bool active
    {
        get
        {
            return !interval.active;
        }
    }

    public Action initializeAction
    {
        get { return exist.initialize; }
        set { exist.initialize = value; }
    }
    public Action startAction
    {
        get { return exist.start; }
        set { exist.start = value; }
    }

    public Action enableAction
    {
        get { return exist.enable; }
        set { exist.enable = value; }
    }
    public Action endAction
    {
        get { return interval.limitAction; }
        set { interval.limitAction = value; }
    }

}
