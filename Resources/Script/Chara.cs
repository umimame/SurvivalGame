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
    [SerializeField] protected Vector2 inputSurfaceVelocityPlan;

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
        assign.x = inputSurfaceVelocityPlan.x * assignSpeed;
        assign.z = inputSurfaceVelocityPlan.y * assignSpeed;
        engine.velocityPlan += assign;
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