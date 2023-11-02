using System;
using UnityEngine;

public class Chara : MonoBehaviour
{
    [field: SerializeField] public Parameter hp;
    [field: SerializeField] public Parameter speed;
    [field: SerializeField] public Parameter pow;
    [SerializeField] protected Engine engine;
    [SerializeField] protected Vector3 inputVelocityPlan;

    protected virtual void Start()
    {
        Initialize();
        engine = GetComponent<Engine>();
        engine.velocityPlanAction += AddVelocityPlan;
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
        engine.velocityPlan += inputVelocityPlan;
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