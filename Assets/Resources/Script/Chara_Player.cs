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
    /// 要素によって倍率(%)が異なる
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
        Step,
        Digs,
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
    [SerializeField] private ParticleSystem bloodParticle;
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
    [SerializeField, NonEditable] private bool moveInputting;       // 移動の入力
    [SerializeField, NonEditable] private bool run;                 // 走り入力
    [SerializeField, NonEditable] private bool moveRigor;           // 移動入力の受け付け
    [SerializeField, NonEditable] private bool viewRigor;           // 視点入力の受け付け
    [SerializeField, NonEditable] private bool attackRigor;         // 攻撃による硬直
    [SerializeField, NonEditable] private bool damageRigor;         // ダメージによる硬直
    [field: SerializeField, NonEditable] public EntityAndPlan<float> leaveButton { get; private set; }  // 巣にスコアを預ける入力
    [SerializeField, NonEditable] private Vector3 dirrection;

    [SerializeField] private Animator animator;
    [field: SerializeField, NonEditable] public MotionState motionState { get; private set; }
    [field: SerializeField, NonEditable] public InputState inputState { get; private set; }
    private float velocitySum;

    [SerializeField] private Vector3 savedVelocity;
    [SerializeField] private Curve inertia = new Curve();

    [SerializeField, NonEditable] private List<Motion> motionsByButton = new List<Motion>();
    [SerializeField] private MotionWithCollider attack1 = new MotionWithCollider();
    [SerializeField] private MotionWithCollider attack2 = new MotionWithCollider();
    [SerializeField] private Motion step = new Motion();
    [SerializeField] private Motion digs = new Motion();

    private List<Motion> interruptByStepMotion = new List<Motion>();    // ステップモーションに割り込まれるモーションを登録
    private List<Motion> interruptByDamageMotion = new List<Motion>();  // 被弾モーションに割り込まれるモーションを登録
    private List<Motion> interruptByDeathMotion = new List<Motion>();   // 死亡モーションに割り込まれるモーションを登録
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
        moveRigor = false;
        viewRigor = false;
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
        moveRigor = false;
        viewRigor = false;
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

        // Motionの設定
        motionsByButton = new List<Motion>() { attack1.motion, attack2.motion, step };

        attack1.Initialize(animator, Anims.attack1, this);
        attack1.startAction += () => moveRigor = true;
        attack1.enableAction += () => StateChange(MotionState.Attack1);
        attack1.endAction += ThinkNextMotion;
        attack1.endAction += () => moveRigor = false;
        attack1.withinThreshold += () => viewRigor = true;
        attack1.outThreshold += () => viewRigor = false;
        attack1.motion.AssignCutInMotion();

        attack2.Initialize(animator, Anims.attack2, this);
        attack2.startAction += () => moveRigor = true;
        attack2.enableAction += () => StateChange(MotionState.Attack2);
        attack2.endAction += ThinkNextMotion;
        attack2.endAction += () => moveRigor = false;
        attack2.withinThreshold += () => viewRigor = true;
        attack2.outThreshold += () => viewRigor = false;
        attack2.motion.AssignCutInMotion();

        interruptByStepMotion = new List<Motion>() { attack1.motion, attack2.motion };
        step.Initialize(animator, Anims.Digs);
        step.startAction += InputMotionReset;
        step.enableAction += () => StateChange(MotionState.Step);
        step.startAction += () => moveRigor = true;
        step.startAction += () => animator.Play("Digs", 0, 0.0f);
        step.endAction += ThinkNextMotion;
        step.endAction += () => moveRigor = false;

        digs.Initialize(animator, Anims.Digs);
        digs.enableAction += () => StateChange(MotionState.Digs);
        digs.startAction += () => moveRigor = true;
        digs.startAction += () => animator.Play("Digs", 0, 0.0f);
        digs.endAction += ThinkNextMotion;
        digs.endAction += () => moveRigor = false;

        damage.Initialize(animator, Anims.damege);
        damage.startAction += InputMotionReset;
        damage.startAction += () => StateChange(MotionState.Damage, true);
        damage.startAction += () => moveRigor = damageRigor = true;
        damage.startAction += () => animator.Play(Anims.damege, 0, 0.0f);       // 連続で再生できる
        damage.enableAction += () => StateChange(MotionState.Damage, true);
        damage.endAction += ThinkNextMotion;
        damage.endAction += () => moveRigor = damageRigor = false;

        interruptByDamageMotion.Add(attack1.motion);
        interruptByDamageMotion.Add(attack2.motion);
        interruptByDamageMotion.Add(damage);

        death.Initialize(animator, Anims.die);
        death.startAction += () => StateChange(MotionState.Death);
        death.startAction += sceneOperator.toResult.AddBlows;
        death.startAction += ChangeScoreByKill;
        death.startAction += () => moveRigor = damageRigor = true;

        interruptByDeathMotion.Add(damage);

        interruptMotionsSolution.Add(interruptByStepMotion);
        interruptMotionsSolution.Add(interruptByDamageMotion);
        interruptMotionsSolution.Add(interruptByDeathMotion);

        invincible.Initialize(true, false);


    }

    /// <summary>
    /// 前提処理<br/>
    /// Updateの最初に行われる
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

        animator.speed = 1;                                                             // アニメーションスピードのリセット
        if (hp.entity <= 0) { alive = false; }                                          // 生存boolのリセット
        moveInputting = (inputMoveVelocity.entity != Vector2.zero) ? true : false;      // 入力boolのリセット
        if (motionState != MotionState.Run) { dashEasing.Clear(); }                     // Curveのリセット


        ThinkNextMotion();
    }

    /// <summary>
    /// 各モーション終了後に行われる
    /// </summary>
    private void ThinkNextMotion()
    {
        if (alive == true)
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
        else
        {
                death.LaunchOneShot();
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
        AssignBMI();

        viewPointManager.AssignCamEulerAngle(transform, false, true, false); // 高さは本体を中心にする

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

                animator.speed = velocitySum;   // 歩きモーションのスピードをスティックに応じて変える
                assignSpeed = speed.entity;

                break;
            case MotionState.Run:
                inputState = InputState.Move;
                InputMoveUpdate();

                stamina.Update(-dashCost.plan);
                velocitySum = 1;
                animator.speed = dashEasing.currentValue;   // 走りモーションのスピードを加速度に応じて変える
                assignSpeed = dashEasing.Update() * dashSpeed.entity;

                break;

            case MotionState.Attack1:
                break;

            case MotionState.Attack2:
                break;

            case MotionState.Step:
                break;

            case MotionState.Digs:
                break;

            case MotionState.Damage:
                InputMoveUpdate();
                break;

            case MotionState.Death:
                break;
        }
        animator.SetInteger(Anims.AnimIdx, (int)motionState);


        TimeOverOperator();
        InputStateUpdate();
    }

    public void AssignBMI()
    {
        float differentScore = sceneOperator.DifferenceOfTopScore(score.plan + score.entity);   // トッププレイヤーとのスコア差

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

        switch (motionState)    // MotionによってInputStateを変える
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
        step.Update();
        damage.Update();
        death.Update();
    }


    /// <summary>
    /// 時間切れなら入力をすべて打ち切る
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
            RigorOperator();

        }
    }

    /// <summary>
    /// 硬直状態(rigor == true)の時に行われる<br/>
    /// 入力の代入
    /// </summary>
    public void RigorOperator()
    {


        if (moveRigor == true)
        {
            inputMoveVelocity.PlanDefault();
            leaveButton.PlanDefault();
        }
        else
        {
            inputMoveVelocity.Assign();
            leaveButton.Assign();
        }

        if(viewRigor == true)
        {
            viewPointManager.inputViewPoint.PlanDefault();
        }
        else
        {
            viewPointManager.inputViewPoint.Assign();

        }

        if (alive == false)
        {

            inputMoveVelocity.PlanDefault();
            leaveButton.PlanDefault();
        }
    }

    /// <summary>
    /// モーション割込み
    /// </summary>
    public void InputMotionReset()
    {
        for (int i = 0; i < interruptMotionsSolution.Count; ++i)
        {
            for (int j = 0; j < interruptMotionsSolution[i].Count; ++j)
            {
                if (interruptMotionsSolution[i][j].exist.state != ExistState.Disable)
                {
                    interruptMotionsSolution[i][j].cutIn?.Invoke();
                    if (interruptMotionsSolution[i][j] != interruptByDamageMotion[interruptByDamageMotion.Count - 1])    // 実行中のモーションなら
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
            ParticleSystem blood = Instantiate(bloodParticle);
            blood.transform.position = transform.position;
        }
    }

    public void AddScore(int score)
    {
        this.score.plan += score;
    }




    /// <summary>
    /// 倒された側が行う<br/>
    /// 最後に攻撃を与えたプレイヤーが得点を得る
    /// </summary>
    /// <param name="you"></param>
    public void ChangeScoreByKill()
    {
        if (lastAttacker == null)
        {
            Debug.Log("自滅");
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


    public void UnderFootColliderStay(Collider other)
    {
        if(other.CompareTag(SurvivalGameTags.Nest) == true)
        {
            NestManager newNest = other.GetComponent<NestManager>();
            newNest.ControleNest(this, Convert.ToBoolean(leaveButton.plan));
        }
    }

    public void UnderFootColliderExit(Collider other)
    {
        if (other.CompareTag(SurvivalGameTags.Nest) == true)
        {
            NestManager newNest = other.GetComponent<NestManager>();
            newNest.ControleNest(this, false);
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
        if (sceneOperator.timeOver == true) { return; }
        if (moveRigor == true) { return; }
        attack1.motion.DurationActive();
        attack1.Launch(power.plan, 3);
    }

    public void OnAttack2(InputValue value)
    {
        if (sceneOperator.timeOver == true) { return; }
        if (moveRigor == true) { return; }
        attack2.motion.DurationActive();
        attack2.Launch(power.plan * 2, 1);
    }

    public void OnLeave(InputValue value)
    {
        if(sceneOperator.timeOver == true) { return; }
        leaveButton.entity = value.Get<float>();
    }


    public void OnStep(InputValue value)
    {

        if (sceneOperator.timeOver == true) { return; }
        if (moveRigor == true) { return; }
        step.DurationActive();
        step.Launch();

    }
    #endregion


}

public static class SurvivalGameTags
{
    public static string Nest = nameof(Nest);
}