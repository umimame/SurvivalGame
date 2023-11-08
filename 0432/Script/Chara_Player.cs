using My;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
    }

    [SerializeField] private Parameter dashSpeed;
    [SerializeField] private PlayerInput input;
    [SerializeField] private CircleClamp norCircle;
    [SerializeField] private SmoothRotate smooth;

    [SerializeField] private Vector2 beforeinputVelocity;
    [SerializeField] private bool inputting;
    private Vector3 direction;
    [SerializeField] private Animator animator;
    [SerializeField] private CharaState motionState;
    [SerializeField] private bool run;
    private float sum;
    [SerializeField] private float motionTime;
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
    }

    protected override void Update()
    {
        Prerequisite();
        base.Update();
        Debug.Log(motionState);
        switch (motionState)
        {
            case CharaState.Idle:
                animator.SetInteger("AnimIdx", (int)motionState);

                break;

            case CharaState.Walk:
                animator.SetInteger("AnimIdx", (int)motionState);
                assignSpeed = speed.entity;

                break;
            case CharaState.Run:
                animator.SetInteger("AnimIdx", (int)motionState);
                assignSpeed = dashSpeed.entity;

                break;

            case CharaState.Attack:
                animator.SetInteger("AnimIdx", (int)motionState);

                break;
        }

        if (inputting == true)  // ì¸óÕÇ≥ÇÍÇƒÇ¢ÇÍÇŒ
        {                       // å¸Ç´Çêßå‰
            norCircle.AdjustByCenter();
            Vector3 addPos;
            addPos.x = norCircle.moveObject.transform.position.x + beforeinputVelocity.normalized.x;
            addPos.y = transform.position.y;
            addPos.z = norCircle.moveObject.transform.position.z + beforeinputVelocity.y;
            addPos = new Vector3(beforeinputVelocity.x, transform.position.y, beforeinputVelocity.y);
            Vector3 newPos = norCircle.moveObject.transform.position + (addPos.normalized * norCircle.radius);
            norCircle.moveObject.transform.position = newPos;
            direction = (norCircle.moveObject.transform.position - gameObject.transform.position).normalized;

            animator.speed = sum;
            beforeinputVelocity = inputSurfaceVelocityPlan;
        }
        smooth.Update(direction);

        norCircle.Limit();
    }


    /// <summary>
    /// ëOíÒèàóù<br/>
    /// UpdateÇÃç≈èâÇ…çsÇÌÇÍÇÈ
    /// </summary>
    public void Prerequisite()
    {
        dashSpeed.Update();
        animator.speed = 1;
        if (hp.entity <= 0) { alive = false; }
        inputting = (inputSurfaceVelocityPlan != Vector2.zero) ? true : false;

        if(alive == true)
        {
            if (inputting == false) { 
                ChangeState(CharaState.Idle);
            }
            else
            {
                if (run == false) { ChangeState(CharaState.Walk); }
                else { ChangeState(CharaState.Run); }
            }
        }
    }

    public void ChangeState(CharaState m)
    {
        motionState = m;
    }

    public void OnMove(InputValue value)
    {
        if (alive == true) 
        {
            inputSurfaceVelocityPlan = value.Get<Vector2>();

            sum = Mathf.Abs(value.Get<Vector2>().x) + Mathf.Abs(value.Get<Vector2>().y);

        }
    }

    public void OnRunning(InputValue value)
    {
        if (alive == true)
        {
            run = Convert.ToBoolean(value.Get<float>());
            
        }
    }

    public void OnAttack1(InputValue value)
    {
        if(alive == true)
        {
            ChangeState(CharaState.Attack);
            motionTime = AddFunction.GetAnimationClipLength(animator.runtimeAnimatorController.animationClips, "attack1");
            Debug.Log("Attack");
        }
    }
}


[Serializable] public class Motion
{
    [SerializeField] private float motionTime;
    [SerializeField] private float nowTime;
}

public enum MotionState
{
    Start,
    Active,
    End,
}
