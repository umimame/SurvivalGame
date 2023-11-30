using AddClass;
using System;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.InputSystem;
using GenericChara;
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

    [SerializeField] private PlayerInput input;
    [field: SerializeField] public int score { get; private set; }
    [field: SerializeField] public Parameter stamina { get; private set; }
    [SerializeField] private Parameter dashSpeed;
    [field: SerializeField] public float dashCost { get; private set; }
    [field: SerializeField] public Interval overStamina {  get; private set; }

    [SerializeField] private Camera cam;
    [SerializeField] private FPSViewPoint viewPointManager;

    [SerializeField, NonEditable] private EntityAndPlan<Vector2> inputMoveVelocity;
    [SerializeField, NonEditable] private bool moveInputting;       // �ړ��̓���
    [SerializeField, NonEditable] private bool viewPointInputting;  // ���_�̓���
    [SerializeField, NonEditable] private bool run;         // �������
    [SerializeField, NonEditable] private bool rigor;       // �d����ԁi���͂��󂯕t���Ȃ��j
    [SerializeField, NonEditable] private Vector3 dirrection;

    [SerializeField] private Animator animator;
    [SerializeField, NonEditable] private MotionState motionState;
    private float velocitySum;


    [SerializeField] private MotionWithCollider _attack1;
    private List<Motion> interruptByDamageMotions = new List<Motion>(); // ��e���[�V�����Ɋ��荞�܂�郂�[�V������o�^
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
        //viewCircle.Initialize(gameObject);
        input = GetComponent<PlayerInput>();
        base.Start();

        underAttackAction += Damage;

        //  Motion�̐ݒ�

        _attack1.Initialize(animator, Anims.attack1, this);
        _attack1.enableAction += () => StateChange(MotionState.Attack);
        _attack1.endAction += () => StateChange(MotionState.None);
        _attack1.endAction += () => rigor = false;

        damage.Initialize(animator, Anims.damege);
        damage.startAction += () => Debug.Log("Damage");
        damage.startAction += () => InputMotionReset();
        damage.startAction += () => StateChange(MotionState.Damage, true);
        damage.startAction += () => animator.Play(Anims.damege, 0, 0.0f);       // �A���ōĐ��ł���
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
    /// �O�񏈗�<br/>
    /// Update�̍ŏ��ɍs����
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

        animator.speed = 1;                                                             // �A�j���[�V�����X�s�[�h�̃��Z�b�g
        if (hp.entity <= 0) { alive = false; }                                          // ����bool�̃��Z�b�g
        moveInputting = (inputMoveVelocity.entity != Vector2.zero) ? true : false;      // ����bool�̃��Z�b�g
        viewPointInputting = (viewPointManager.inputViewPoint.entity != Vector2.zero) ? true : false;    // ���_bool�̃��Z�b�g
        rigor = false;                                                                  // �d��bool�̃��Z�b�g

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

    public void InputMoveUpdate()    // �������ԂȂ�
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

        //viewPointManager.VerticalOffset(transform);
        viewPointManager.LookAtViewPoint(transform, true, false, true); // �����͖{�̂𒆐S�ɂ���

        Vector3 cameraForward = new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z).normalized;
        Vector3 movePos = cameraForward * inputMoveVelocity.plan.y + cam.transform.right * inputMoveVelocity.plan.x;
        moveVelocity.plan = new Vector2(movePos.x, movePos.z);

        if (velocitySum > 1) { velocitySum = 1; }
        switch (motionState)
        {
            case MotionState.Idle:
                InputMoveUpdate();

                break;

            case MotionState.Walk:
                InputMoveUpdate();

                animator.speed = velocitySum;   // �������[�V�����̃X�s�[�h���X�e�B�b�N�ɉ����ĕς���
                assignSpeed = speed.entity;

                break;
            case MotionState.Run:
                InputMoveUpdate();

                stamina.Update(-dashCost);
                velocitySum = 1;
                assignSpeed = dashSpeed.entity;

                break;

            case MotionState.Attack:
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



    /// <summary>
    /// �d�����(rigor == true)�̎��ɍs����
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
                if(m != interruptByDamageMotions[1])    // ���s���̃��[�V�����Ȃ�
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
    /// �|���ꂽ�����s��<br/>
    /// �Ō�ɍU����^�����v���C���[�����_�𓾂�
    /// </summary>
    /// <param name="you"></param>
    public void ChangeScoreByKill()
    {
        Debug.Log("Death");
        if(lastAttacker == null)
        {
            Debug.Log("����");
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

    #region PlayerInput�Ɏ����œo�^�����Event

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
        _attack1.Launch(50, 3);
    }


    public void OnDamage(InputValue value)
    {
        UnderAttack(50, UnderAttackType.Normal);

    }
    #endregion


}

