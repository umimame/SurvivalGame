using My;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;

public class Chara_Player : Chara
{
    enum MotionState
    {
        Idle,
        Move,
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
    [SerializeField] private MotionState motionState;
    [SerializeField] private bool run;
    private float sum;

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

        switch (motionState)
        {
            case MotionState.Idle:
                animator.SetBool("Idle", true); // ì¸óÕÇ≥ÇÍÇƒÇ¢Ç»ÇØÇÍÇŒë“ã@èÛë‘
                break;

            case MotionState.Move:
                if (run == true)
                {
                    assignSpeed = dashSpeed.entity;
                }
                else { assignSpeed = speed.entity; }
                animator.SetBool("Run", run);
                animator.SetFloat("Move", sum);
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
            //norCircle.moveObject.transform.position = Vector3.MoveTowards(norCircle.moveObject.transform.position, newPos, 2 * Time.deltaTime);

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
        if(alive == true)
        {
            if (inputting == false) { motionState = MotionState.Idle; }

        }
        animator.SetBool("Idle", false);
    }

    public void OnMove(InputValue value)
    {
        if (alive == true)
        {
            inputSurfaceVelocityPlan = value.Get<Vector2>();

            inputting = (inputSurfaceVelocityPlan != Vector2.zero) ? true : false;
            sum = Mathf.Abs(value.Get<Vector2>().x) + Mathf.Abs(value.Get<Vector2>().y);

            motionState = MotionState.Move;
        }
    }

    public void OnRunning(InputValue value)
    {
        if (alive == true)
        {
            run = Convert.ToBoolean(value.Get<float>());
        }
    }
}
