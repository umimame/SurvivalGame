using AddClass;
using System;
using UnityEngine;


[CreateAssetMenu(fileName = "GravityManager", menuName = "ScriptableObject/GravityManager")]
[Serializable] public class GravityProfile : ScriptableObject
{
    [field: SerializeField] public Vector3 gravityScale { get; private set; }
    [field: SerializeField] public AnimationCurve easing { get; private set; }
}


[Serializable] public class GravityOperator
{
    [field: SerializeField] public GravityProfile gravityProfile { get; set; }
    [field: SerializeField] public Traffic traffic { get; set; }
    public bool active
    {
        get { return traffic.active; }
        set { traffic.active = value; }
    }


    [field: SerializeField] public bool reversal;
    [field: SerializeField] public VariedTime time { get; private set; } = new VariedTime();
    [field: SerializeField, NonEditable] public EntityAndPlan<Vector3> currentGravity; 


    public void Initialize()
    {
        if (gravityProfile == null) { return; }
        traffic.Initialize();
        traffic.activeAction += ActiveAction;
        traffic.nonActiveAction += NonActiveAction;
        Clear();
    }

    public void Clear()
    {
        time.Initialize();
        currentGravity.entity = Eva(time.value);
    }

    public void Update()
    {
        if (gravityProfile == null) {
            Debug.Log("Gravity");
            return; }
        currentGravity.entity = Eva(time.value);
        traffic.Update();
        time.Update();
    }
    public Vector3 Eva(float time)
    {
        Vector3 returnVec = Vector3.zero;
        returnVec.x = gravityProfile.gravityScale.x * gravityProfile.easing.Evaluate(time);
        returnVec.y = gravityProfile.gravityScale.y * gravityProfile.easing.Evaluate(time);
        returnVec.z = gravityProfile.gravityScale.z * gravityProfile.easing.Evaluate(time);


        return returnVec;
    }

    private void ActiveAction()
    {
        currentGravity.Assign();
    }
    private void NonActiveAction()
    {
        currentGravity.plan = Vector3.zero;
    }



}