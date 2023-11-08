using My;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Events;
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
    [SerializeField, NonEditable] private bool rigor;       // 硬直
    private Vector3 direction;
    [SerializeField] private Animator animator;
    [SerializeField] private CharaState motionState;
    private float velocitySum;
    [SerializeField] private Motion attack1;

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
        attack1.updateAction += () => motionState = CharaState.Attack;
        attack1.updateAction += () => rigor = true;
        attack1.endAction += () => motionState = CharaState.Idle;  // stateを変えるラムダ式
        attack1.endAction += () => rigor = false;
    }

    protected override void Update()
    {
        Reset();
        base.Update();

        attack1.Update();
        switch (motionState)
        {
            case CharaState.Idle:
                animator.SetInteger(Anims.AnimIdx, (int)motionState);

                break;

            case CharaState.Walk:
                animator.SetInteger(Anims.AnimIdx, (int)motionState);
                assignSpeed = speed.entity;

                break;
            case CharaState.Run:
                animator.SetInteger(Anims.AnimIdx, (int)motionState);
                assignSpeed = dashSpeed.entity;

                break;

            case CharaState.Attack:
                animator.SetInteger(Anims.AnimIdx, (int)motionState);

                break;
        }

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

            animator.speed = velocitySum;
            beforeinputVelocity = inputSurfaceVelocityPlan;
        }
        smooth.Update(direction);

        norCircle.Limit();

        RigorReset();
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
        inputting = (inputSurfaceVelocityPlan != Vector2.zero) ? true : false;
        rigor = false;

        if(alive == true)
        {
            if (rigor == false)
            {


                if (inputting == false)
                {
                    ChangeState(CharaState.Idle);
                }
                else
                {
                    if (run == false) { ChangeState(CharaState.Walk); }
                    else { ChangeState(CharaState.Run); }
                }
            }
        }
    }

    /// <summary>
    /// 硬直状態(rigor == true)の時に行われる
    /// </summary>
    public void RigorReset()
    {

        if (rigor == true)
        {
            inputSurfaceVelocityPlan = Vector2.zero;
        }
    }

    public void ChangeState(CharaState m)
    {
        motionState = m;
    }

    public void OnMove(InputValue value)
    {
        if( rigor == true) { return; }
        if(alive == false) { return; }

        inputSurfaceVelocityPlan = value.Get<Vector2>();
        velocitySum = Mathf.Abs(value.Get<Vector2>().x) + Mathf.Abs(value.Get<Vector2>().y);

        return;
        
    }

    public void OnRunning(InputValue value)
    {
        if (rigor == true) { return; }
        if (alive == false) { return; }

        run = Convert.ToBoolean(value.Get<float>());
            
        
    }

    public void OnAttack1(InputValue value)
    {
        if (rigor == true) { return; }
        if (alive == false) { return; }

        ChangeState(CharaState.Attack);
        attack1.Start();
        Debug.Log("Attack");
    }
}


[Serializable] public class Motion
{
    [SerializeField] private float motionTime;
    [SerializeField] private float adjustMotionTime;
    [field: SerializeField] public Interval interval { get; set; }

    [SerializeField] private Exist exist;
    [SerializeField] private EasingAnimator easAnim;
    public void Initialize(Animator animator, string clipName)
    {
        motionTime = AddFunction.GetAnimationClipLength(animator, clipName);
        motionTime += adjustMotionTime;

        easAnim.Initialize(motionTime, animator);
        interval.Initialize(false, true, motionTime);

        exist.start += easAnim.Reset;

        exist.update += interval.Update;
        exist.update += easAnim.Update;
        interval.action += exist.Finish;
    }

    public void Update()
    {
        exist.Enable();
    }

    public void Start()
    {
        exist.Initialize();
        exist.Start();
    }

    public bool active
    {
        get
        {
            return !interval.active;
        }
    }

    public Action updateAction
    {
        get { return exist.update; }
        set { exist.update = value; }
    }
    public Action endAction
    {
        get { return interval.action; }
        set { interval.action = value; }
    }

}
