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
    [SerializeField, NonEditable] private bool moveInputting;       // 移動の入力
    [SerializeField, NonEditable] private bool run;                 // 走り入力
    [SerializeField, NonEditable] private bool moveRigor;           // 硬直状態（移動入力を受け付けない）
    [SerializeField, NonEditable] private bool viewRigor;           // 硬直状態（移動入力を受け付けない）
    [field: SerializeField, NonEditable] public EntityAndPlan<float> leaveButton { get; private set; }  // 巣にスコアを預ける入力
    [SerializeField, NonEditable] private Vector3 dirrection;

    [SerializeField] private Animator animator;
    [field: SerializeField, NonEditable] public MotionState motionState { get; private set; }
    [field: SerializeField, NonEditable] public InputState inputState { get; private set; }
    private float velocitySum;

    [SerializeField] private Vector3 savedVelocity;
    [SerializeField] private Curve inertia = new Curve();


    [SerializeField] private MotionWithCollider attack1 = new MotionWithCollider();
    [SerializeField] private MotionWithCollider attack2 = new MotionWithCollider();
    private List<Motion> interruptByDamageMotions = new List<Motion>(); // 被弾モーションに割り込まれるモーションを登録
    private List<Motion> interruptByDeathMotions = new List<Motion>();  // 死亡モーションに割り込まれるモーションを登録
    private List<List<Motion>> interruptMotionsSolution = new List<List<Motion>>();

    [SerializeField] private Motion step = new Motion();
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

        //  Motionの設定
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

        step.Initialize(animator, Anims.attack2);


        damage.Initialize(animator, Anims.damege);
        damage.startAction += () => InputMotionReset();
        damage.startAction += () => StateChange(MotionState.Damage, true);
        damage.startAction += () => moveRigor = true;
        damage.startAction += () => animator.Play(Anims.damege, 0, 0.0f);       // 連続で再生できる
        damage.enableAction += () => StateChange(MotionState.Damage, true);
        damage.endAction += ThinkNextMotion;
        damage.endAction += () => moveRigor = false;

        interruptByDamageMotions.Add(attack1.motion);
        interruptByDamageMotions.Add(attack2.motion);
        interruptByDamageMotions.Add(damage);

        death.Initialize(animator, Anims.die);
        death.startAction += () => StateChange(MotionState.Death);
        death.startAction += () => StateChange(CharaState.Death);
        death.startAction += sceneOperator.toResult.AddBlows;
        death.startAction += ChangeScoreByKill;
        death.startAction += () => moveRigor = true;

        interruptByDeathMotions.Add(damage);

        interruptMotionsSolution.Add(interruptByDamageMotions);
        interruptMotionsSolution.Add(interruptByDeathMotions);

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
            if (motionState != MotionState.Damage)
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
                    if (interruptMotionsSolution[i][j] != interruptByDamageMotions[interruptByDamageMotions.Count - 1])    // 実行中のモーションなら
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
    /// 倒された側が行う<br/>
    /// 最後に攻撃を与えたプレイヤーが得点を得る
    /// </summary>
    /// <param name="you"></param>
    public void ChangeScoreByKill()
    {
        Debug.Log("Death");
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
        attack1.Launch(power.plan, 3);
    }

    public void OnAttack2(InputValue value)
    {
        if (sceneOperator.timeOver == true) { return; }
        if (moveRigor == true) { return; }
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