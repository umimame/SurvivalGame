using AddClass;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InductionProfile", menuName = "ScriptableObject/InductionProfile")]
public class InductionProfile : ScriptableObject
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
