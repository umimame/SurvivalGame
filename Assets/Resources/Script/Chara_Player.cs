using AddClass;
using GenericChara;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
public class Chara_Player : Chara
{
    /// <summary>
    /// �v�f�ɂ���Ĕ{��(%)���قȂ�
    /// </summary>
    public enum BMI
    {
        Default = 100,
        LightSkinny = 130,
        Skinny = 150,
        Hunger = 170,
    }
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
    public enum InputState
    {
        Idle,
        Move,
        Rigor,
    }

    [SerializeField] private PlayerInput input;
    [field: SerializeField] public PlayerUI UI { get; private set; }
    public PlayerRespawnPos playerRespawnPos { get; set; }
    public GameScene_Operator sceneOperator { get; set; }
    [field: SerializeField, NonEditable] public EntityAndPlan<int> score { get; private set; }
    [field: SerializeField, NonEditable] public BMI bmi { get; private set; }
    [field: SerializeField] public Parameter stamina { get; private set; }
    [SerializeField] private Parameter dashSpeed;
    [SerializeField] private Curve dashEasing = new Curve();
    [field: SerializeField] public EntityAndPlan<float> dashCost { get; private set; }
    [SerializeField] private EntityAndPlan<float> power;
    [field: SerializeField] public Interval overStamina { get; private set; }

    [SerializeField] private Camera cam;
    [SerializeField] private TPSViewPoint viewPointManager;

    [SerializeField, NonEditable] private EntityAndPlan<Vector2> inputMoveVelocity;
    [SerializeField, NonEditable] private bool moveInputting;       // �ړ��̓���
    [SerializeField, NonEditable] private bool run;                 // �������
    [SerializeField, NonEditable] private bool rigor;               // �d����ԁi���͂��󂯕t���Ȃ��j
    [field: SerializeField, NonEditable] public EntityAndPlan<float> leaveButton { get; private set; }  // ���ɃX�R�A��a�������
    [SerializeField, NonEditable] private Vector3 dirrection;

    [SerializeField] private Animator animator;
    [field: SerializeField, NonEditable] public MotionState motionState { get; private set; }
    [field: SerializeField, NonEditable] public InputState inputState { get; private set; }
    private float velocitySum;

    [SerializeField] private Vector3 savedVelocity;
    [SerializeField] private Curve inertia = new Curve();


    [SerializeField] private MotionWithCollider attack1 = new MotionWithCollider();
    [SerializeField] private MotionWithCollider attack2 = new MotionWithCollider();
    private List<Motion> interruptByDamageMotions = new List<Motion>(); // ��e���[�V�����Ɋ��荞�܂�郂�[�V������o�^
    private List<Motion> interruptByDeathMotions = new List<Motion>();  // ���S���[�V�����Ɋ��荞�܂�郂�[�V������o�^
    private List<List<Motion>> interruptMotionsSolution = new List<List<Motion>>();

    [SerializeField] private Motion damage = new Motion();
    [SerializeField] private Motion death = new Motion();


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

    private void RespawnAction()
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
        transform.position = playerRespawnPos.PlayerRespawn();

    }

    protected override void Start()
    {
        reSpawnAction += RespawnAction;

        stamina.Initialize();
        overStamina.Initialize(true, false);
        overStamina.activeAction += stamina.Update;

        gameObject.tag = gameObject.transform.parent.tag;
        input = GetComponent<PlayerInput>();
        base.Start();

        underAttackAction += Damage;

        //  Motion�̐ݒ�
        attack1.Initialize(animator, Anims.attack1, this);
        attack1.enableAction += () => StateChange(MotionState.Attack1);
        attack1.endAction += () => StateChange(MotionState.Idle);
        attack1.endAction += () => rigor = false;

        attack2.Initialize(animator, Anims.attack2, this);
        attack2.enableAction += () => StateChange(MotionState.Attack2);
        attack2.endAction += () => StateChange(MotionState.Idle);
        attack2.endAction += () => rigor = false;

        damage.Initialize(animator, Anims.damege);
        damage.startAction += () => InputMotionReset();
        damage.startAction += () => StateChange(MotionState.Damage, true);
        damage.startAction += () => animator.Play(Anims.damege, 0, 0.0f);       // �A���ōĐ��ł���
        damage.enableAction += () => StateChange(MotionState.Damage, true);
        damage.endAction += () => StateChange(MotionState.Idle);
        damage.endAction += () => rigor = false;

        interruptByDamageMotions.Add(attack1.motion);
        interruptByDamageMotions.Add(attack2.motion);
        interruptByDamageMotions.Add(damage);

        death.Initialize(animator, Anims.die);
        death.startAction += () => StateChange(MotionState.Death);
        death.startAction += sceneOperator.toResult.AddBlows;
        death.startAction += ChangeScoreByKill;
        death.enableAction += () => StateChange(MotionState.Death);
        death.startAction += () => StateChange(CharaState.Death);

        interruptByDeathMotions.Add(damage);

        interruptMotionsSolution.Add(interruptByDamageMotions);
        interruptMotionsSolution.Add(interruptByDeathMotions);

        invincible.Initialize(true, false);


    }

    /// <summary>
    /// �O�񏈗�<br/>
    /// Update�̍ŏ��ɍs����
    /// </summary>
    public void InitialUpdate()
    {
        invincible.Update();

        stamina.ReturnRange();
        if (stamina.overZero == true) {
            if (overStamina.active == true)
            {
                overStamina.Reset();
            }
        }

        animator.speed = 1;                                                             // �A�j���[�V�����X�s�[�h�̃��Z�b�g
        if (hp.entity <= 0) { alive = false; }                                          // ����bool�̃��Z�b�g
        moveInputting = (inputMoveVelocity.entity != Vector2.zero) ? true : false;      // ����bool�̃��Z�b�g
        rigor = false;                                                                  // �d��bool�̃��Z�b�g
        if (motionState != MotionState.Run) { dashEasing.Clear(); }                     // Curve�̃��Z�b�g

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
                            if (stamina.overZero == false) { StateChange(MotionState.Run); }

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
                if (motionState != MotionState.Damage)
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
        InitialUpdate();
        base.Update();
        MotionUpdate();
        AssignBMI();

        viewPointManager.AssignCamEulerAngle(transform, false, true, false); // �����͖{�̂𒆐S�ɂ���

        Vector3 addVelo = Vector3.zero;
        addVelo.x = AddFunction.GetFPSMoveVec2(cam, inputMoveVelocity.plan).x;
        addVelo.z = AddFunction.GetFPSMoveVec2(cam, inputMoveVelocity.plan).y;
        AddAssignedMoveVelocity(addVelo);

        if (velocitySum > 1) { velocitySum = 1; }
        switch (motionState)
        {
            case MotionState.Idle:
                inputState = InputState.Idle;
                InputMoveUpdate();

                break;

            case MotionState.Walk:
                inputState = InputState.Move;
                InputMoveUpdate();

                animator.speed = velocitySum;   // �������[�V�����̃X�s�[�h���X�e�B�b�N�ɉ����ĕς���
                assignSpeed = speed.entity;

                break;
            case MotionState.Run:
                inputState = InputState.Move;
                InputMoveUpdate();

                stamina.Update(-dashCost.plan);
                velocitySum = 1;
                animator.speed = dashEasing.currentValue;   // ���胂�[�V�����̃X�s�[�h�������x�ɉ����ĕς���
                assignSpeed = dashEasing.Update() * dashSpeed.entity;

                break;

            case MotionState.Attack1:
                inputState = InputState.Rigor;
                rigor = true;
                break;

            case MotionState.Attack2:
                inputState = InputState.Rigor;
                rigor = true;
                break;

            case MotionState.Damage:
                inputState = InputState.Rigor;
                InputMoveUpdate();
                rigor = true;
                break;

            case MotionState.Death:
                inputState = InputState.Rigor;
                rigor = true;
                break;
        }
        animator.SetInteger(Anims.AnimIdx, (int)motionState);


        TimeOverOperator();
        InputStateUpdate();
    }

    public void AssignBMI()
    {
        float differentScore = sceneOperator.DifferenceOfTopScore(score.plan + score.entity);   // �g�b�v�v���C���[�Ƃ̃X�R�A��

        if (differentScore == 0)
        {
            bmi = BMI.Default;
        }
        else if (differentScore >= 70)
        {
            bmi = BMI.Hunger;
        }
        else if (differentScore >= 50)
        {
            bmi = BMI.Skinny;
        }
        else if (differentScore >= 30)
        {
            bmi = BMI.LightSkinny;
        }

        AdjustStatusByBMI();
    }

    public void AdjustStatusByBMI()
    {
        dashCost.plan = dashCost.entity / ((float)bmi * 0.01f);
        power.plan = power.entity * (float)bmi * 0.01f;
    }

    public void InputStateUpdate()
    {

        switch (motionState)    // Motion�ɂ����InputState��ς���
        {
            case MotionState.Idle:
                inputState = InputState.Idle;
                break;

            case MotionState.Walk:
                inputState = InputState.Move;
                break;
            case MotionState.Run:
                inputState = InputState.Move;
                break;

            case MotionState.Attack1:
                inputState = InputState.Rigor;
                break;

            case MotionState.Attack2:
                inputState = InputState.Rigor;
                break;

            case MotionState.Damage:
                inputState = InputState.Rigor;
                break;

            case MotionState.Death:
                inputState = InputState.Rigor;
                break;
        }

        switch (inputState)
        {
            case InputState.Idle:
                moveVelocity.plan += savedVelocity * inertia.Update();
                break;

            case InputState.Move:
                savedVelocity = moveVelocity.plan;
                inertia.Clear();
                break;

            case InputState.Rigor:
                moveVelocity.plan += savedVelocity * inertia.Update();
                break;
        }
    }

    public void MotionUpdate()
    {

        attack1.Update();
        attack2.Update();
        damage.Update();
        death.Update();
    }


    /// <summary>
    /// ���Ԑ؂�Ȃ���͂����ׂđł��؂�
    /// </summary>
    private void TimeOverOperator()
    {
        if(sceneOperator.timeOver == true)
        {
            inputMoveVelocity.Default();
            viewPointManager.InputZeroAssign();
            leaveButton.Default();
            moveVelocity.Default();
            run = false;
        }
        else
        {
            RigorReset();

        }
    }

    /// <summary>
    /// �d�����(rigor == true)�̎��ɍs����<br/>
    /// ���͂̑��
    /// </summary>
    public void RigorReset()
    {

        if (rigor == true)
        {
            inputMoveVelocity.PlanDefault();
            viewPointManager.inputViewPoint.Assign();
            leaveButton.PlanDefault();
        }
        else
        {
            inputMoveVelocity.Assign();
            viewPointManager.inputViewPoint.Assign();
            leaveButton.Assign();
        }
    }


    public void InputMotionReset()
    {
        for (int i = 0; i < interruptMotionsSolution.Count; ++i)
        {
            for (int j = 0; j < interruptMotionsSolution[i].Count; ++j)
            {
                if (interruptMotionsSolution[i][j].exist.state != ExistState.Disable)
                {
                    interruptMotionsSolution[i][j].cutIn?.Invoke();
                    if (interruptMotionsSolution[i][j] != interruptByDamageMotions[interruptByDamageMotions.Count - 1])    // ���s���̃��[�V�����Ȃ�
                    {
                        interruptMotionsSolution[i][j].Reset();
                    }
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
            if (alive == true)
            {
                motionState = m;
            }
        }
    }

    public void Damage(UnderAttackType type)
    {
        if (type == UnderAttackType.Normal)
        {
            damage.Launch();
            invincible.Reset();

        }
    }

    public void AddScore(int score)
    {
        this.score.plan += score;
    }




    /// <summary>
    /// �|���ꂽ�����s��<br/>
    /// �Ō�ɍU����^�����v���C���[�����_�𓾂�
    /// </summary>
    /// <param name="you"></param>
    public void ChangeScoreByKill()
    {
        Debug.Log("Death");
        if (lastAttacker == null)
        {
            Debug.Log("����");
        }

        if (score.plan > 0)
        {
            Chara_Player _lastAttacker = (Chara_Player)lastAttacker;
            _lastAttacker.AddScore(score.plan / 2);
            score.plan /= 2;
            Debug.Log(this.score);
            Debug.Log(_lastAttacker.score);
        }
    }

    public float ChangeScoreByLeave()
    {
        float returnScore = score.plan / 2;
        score.plan /= 2;

        return returnScore;
    }

    public int leavedScore
    {
        get { return score.entity; }
        set {  score.entity = value; }
    }

    public int sumScore
    {
        get { return score.entity + score.plan; }

    }


    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag(SurvivalGameTags.Nest) == true)
        {
            NestManager newNest = other.GetComponent<NestManager>();
            newNest.ControleNest(this, Convert.ToBoolean(leaveButton.plan));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(SurvivalGameTags.Nest) == true)
        {
            NestManager newNest = other.GetComponent<NestManager>();
            newNest.ControleNest(this, false);
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
        if (sceneOperator.timeOver == true) { return; }
        if (rigor == true) { return; }
        attack1.Launch(power.plan, 3);
    }

    public void OnAttack2(InputValue value)
    {
        if (sceneOperator.timeOver == true) { return; }
        if (rigor == true) { return; }
        attack2.Launch(power.plan * 2, 1);
    }

    public void OnLeave(InputValue value)
    {
        if(sceneOperator.timeOver == true) { return; }
        leaveButton.entity = value.Get<float>();
    }


    public void OnDamage(InputValue value)
    {
        //UnderAttack(50, UnderAttackType.Normal);

    }
    #endregion


}

public static class SurvivalGameTags
{
    public static string Nest = nameof(Nest);
}