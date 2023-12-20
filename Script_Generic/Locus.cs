using AddClass;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Locus", menuName = "ScriptableObject/Locus")]
public class Locus : ScriptableObject
{
    [field: SerializeField] public Vec3Curve addPos { get; private set; }
    [SerializeField, Button("ZeroFill","ZeroFill", 0)] private int addPosZeroFill;
    [SerializeField, Button("Clear", "Clear", 0)] private int addPosClear;
    [field: SerializeField] public Vec3Curve assignEulerAngle { get; private set; }
    [SerializeField, Button("ZeroFill", "ZeroFill", 1)] private int assignEulerAngleZeroFill;
    [SerializeField, Button("Clear", "Clear", 1)] private int assignEulerAngleClear;
    [field: SerializeField] public Vec3Curve assignEulerAngleForModel { get; private set; }
    [SerializeField, Button("ZeroFill", "ZeroFill", 2)] private int assignEulerAngleForModelZeroFill;
    [SerializeField, Button("Clear", "Clear", 2)] private int assignEulerAngleForModelClear;


    public void ZeroFill(int target)
    {
        switch (target)
        {
            case 0:
                addPos.Reset();
                addPos.ZeroFill();
                break;
            case 1:
                assignEulerAngle.Reset();
                assignEulerAngle.ZeroFill();
                break;
            case 2:
                assignEulerAngleForModel.Reset();
                assignEulerAngleForModel.ZeroFill();
                break;
        }
    }
    public void Clear(int target)
    {
        switch (target)
        {
            case 0:
                addPos.Reset();
                addPos.Clear();
                break;
            case 1:
                assignEulerAngle.Reset();
                assignEulerAngle.Clear();
                break;
            case 2:
                assignEulerAngleForModel.Reset();
                assignEulerAngleForModel.Clear();
                break;
        }
    }
}

[Serializable] public class LocusOperator
{
    [field: SerializeField] private Locus locus;
    [field: SerializeField, NonEditable] public Vector3 posEva { get; private set; }
    [field: SerializeField, NonEditable] public Vector3 rotEva { get; private set; }
    [field: SerializeField, NonEditable] public Vector3 modelRotEva { get; private set; }
    [field: SerializeField, NonEditable] public VariedTime currentTime { get; private set; } = new VariedTime();

    public void Initialize(IncreseType increseType = default)
    {
        Reset();
        if (increseType != default) { currentTime.increseType = increseType; }
    }

    public void Reset()
    {
        currentTime.Initialize();

        posEva = locus.addPos.Eva(currentTime.value);
        rotEva = locus.assignEulerAngle.Eva(currentTime.value);
        modelRotEva = locus.assignEulerAngleForModel.Eva(currentTime.value);
    }


    /// <summary>
    /// timeÇÃëùâ¡<br/>
    /// EvaluteÇë„ì¸å„Ç…åƒÇ—èoÇ∑
    /// </summary>
    public void Update()
    {
        posEva = locus.addPos.Eva(currentTime.value);
        rotEva = locus.assignEulerAngle.Eva(currentTime.value);
        modelRotEva = locus.assignEulerAngleForModel.Eva(currentTime.value);

        currentTime.Update();
    }

    /// <summary>
    /// äÑçáÇ≈EvaluteÇéZèoÇ∑ÇÈ
    /// </summary>
    /// <param name="ratio"></param>
    public void Update(float ratio)
    {
        posEva = locus.addPos.Eva(currentTime.value);
        rotEva = locus.assignEulerAngle.Eva(currentTime.value);
        modelRotEva = locus.assignEulerAngleForModel.Eva(currentTime.value);

        currentTime.Update(ratio);

    }

}

[Serializable] public class LocusMotion
{
    [SerializeField] private Traffic traffic = new Traffic();
    [SerializeField] private LocusOperator motionLocus = new LocusOperator();   // motionTimeÇÃäÑçáÇéûä‘Ç…ë„ì¸Ç∑ÇÈ
    [SerializeField] private Interval motionTime = new Interval();
    
    public LocusMotion()
    {

    }

    public void Initialize()
    {
        motionLocus.Initialize(IncreseType.Manual);
        motionTime.Initialize(false, true);
        traffic.Initialize();
        traffic.enableAction += EnableAction;
        traffic.disableAction += DisableAction;
        motionTime.reachAction += OverMotionTimeAction;
    }

    public void Update()
    {
        traffic.Update();
    }

    private void EnableAction()
    {
        motionLocus.Update(motionTime.ratio);
        motionTime.Update();
    }

    private void DisableAction()
    {
        Reset();
    }

    public void Reset()
    {

        motionTime.Reset();
        motionLocus.Reset();
    }

    public void OverMotionTimeAction()
    {
        traffic.Close();
        Reset();
    }

    public void Launch()
    {
        traffic.Launch();
    }

    public Vector3 velocity
    {
        get { return motionLocus.posEva; }
    }
    public Vector3 eulerAngle
    {
        get { return motionLocus.rotEva; }
    }
    public Vector3 addEulerAngle
    {
        get { return motionLocus.modelRotEva; }
    }
    public Action reachAction
    {
        get { return motionTime.reachAction; }
        set { motionTime.reachAction = value; }
    }
}