using My;
using System;
using UnityEngine;

public class Chara : MonoBehaviour
{
    [field: SerializeField] public Parameter hp;
    [field: SerializeField] public Parameter speed;
    protected float assignSpeed;
    [field: SerializeField] public Parameter pow;
    protected Engine engine;
    [field: SerializeField, NonEditable] public bool alive { get; protected set; }  //  生存
    [SerializeField] protected EntityAndPlan<Vector2> inputMoveVelocity;
    protected Action<UnderAttackType> underAttackAction;

    [SerializeField] protected Interval invincible;
    protected virtual void Start()
    {
        Initialize();
        engine = GetComponent<Engine>();
        engine.velocityPlanAction += AddVelocityPlan;
        alive = true;
    }
    protected virtual void Update()
    {
        hp.Update();
        speed.Update();
        pow.Update();
    }
    public void Initialize()
    {
        hp.Initialize();
        speed.Initialize();
        pow.Initialize();
    }

    public void AddVelocityPlan()
    {
        Vector3 assign = Vector3.zero;
        assign.x = inputMoveVelocity.plan.x * assignSpeed;
        assign.z = inputMoveVelocity.plan.y * assignSpeed;
        engine.velocityPlan += assign;
    }

    /// <summary>
    /// 引数はダメージ量と被弾モーションを行うかどうか
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="damageMotion"></param>
    public bool UnderAttack(float damage, UnderAttackType type = UnderAttackType.None)
    {
        if (alive == false) { return false; }
        if (invincible.active == false) { return false; }

        hp.entity -= damage;
        Debug.Log(hp.entity);

        if (type != UnderAttackType.None)
        {
            underAttackAction?.Invoke(type);
        }


        return true;
    }

}

/// <summary>
/// 数値の中身と最大値を含む<br/>
/// インスタンス化不要
/// </summary>
[Serializable] public class Parameter
{
    [field: SerializeField, NonEditable] public float entity { get; set; }
    [field: SerializeField] public float max { get; set; }

    public void Initialize()
    {
        entity = max;
    }

    public void Update()
    {
        if(entity > max) { entity = max; }
    }
}