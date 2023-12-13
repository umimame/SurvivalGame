using AddClass;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputOperator : MonoBehaviour
{
    [field: SerializeField] public PlayerInput playerInput { get; private set; }
    [field: SerializeField] public List<InputVecOrFloat<Vector3>> vInputs = new List<InputVecOrFloat<Vector3>>();
    [field: SerializeField] public List<InputVecOrFloat<float>> fInputs = new List<InputVecOrFloat<float>>();
    [field: SerializeField, NonEditable] public InputVecOrFloat<Vector3> moveInput { get; private set; } = new InputVecOrFloat<Vector3>();
    
    public void Initialize()
    {
        playerInput = GetComponent<PlayerInput>();


        foreach(var i in vInputs)
        {
            i.Initialize();
        }
        foreach (var i in fInputs)
        {
            i.Initialize();
        }

    }
    public void Update()
    {

        foreach (var i in vInputs)
        {
            i.Update();
        }
        foreach (var i in fInputs)
        {
            i.Update();
        }
    }

    public void SetList()
    {

        TypeFinder t = gameObject.AddComponent<TypeFinder>();
        vInputs = t.GetAndInList<InputVecOrFloat<Vector3>>(GetType());

        fInputs = t.GetAndInList<InputVecOrFloat<float>>(GetType());
        Destroy(t);
    }

    #region PlayerInputEvent
    public void OnMove(InputValue value)
    {
        Vector3 newVec = Vector3.zero;
        newVec.x = value.Get<Vector2>().x;
        newVec.z = value.Get<Vector2>().y;

        moveInput.entity = newVec;

    }
    #endregion

}


[Serializable]
public class Inputting
{
    public enum InputType
    {
        None,
        Vector3,
        Float,
    }
    [field: SerializeField, NonEditable] public bool inputting { get; protected set; }
    public virtual void Initialize() { }
    public virtual void Update() { }
}
/// <summary>
/// T‚ÍVector3‚Ü‚½‚Ífloat<br/>
/// EntityAndPlan‚Æbool‚ðŠÜ‚Þ
/// </summary>
/// <typeparam name="T"></typeparam>
[Serializable] public class InputVecOrFloat<T> : Inputting where T : struct
{
    [field: SerializeField, NonEditable] public EntityAndPlan<T> input { get; set; } = new EntityAndPlan<T>();
    [field: SerializeField] public ValueInRange floatRange = new ValueInRange();
    [SerializeField, NonEditable] private InputType thisType;

    public override void Initialize()
    {
        if (typeof(T).Equals(typeof(Vector3)))
        {
            thisType = InputType.Vector3;
        }
        else if (typeof(T).Equals(typeof(float)))
        {
            thisType = InputType.Float;
        }
    }

    public override void Update()
    {
        switch (thisType)
        {
            case InputType.Vector3:
                if ((Vector3)(object)input.entity != Vector3.zero) 
                { 
                    inputting = true;
                    return;
                }
                break;

            case InputType.Float:
                if ((float)(object)input.entity != 0.0f) 
                { 
                    if(floatRange.min == 0.0f &&  floatRange.max == 0.0f)
                    {
                        inputting = true;
                    }
                    else
                    {
                        inputting = floatRange.JudgeRange((float)(object)input.entity);
                    }
                    return;
                }
                break;

            default:
                break;
        }

        inputting = false;
    }

    public void Assign()
    {
        input.Assign();
    }

    public T entity
    {
        get { return input.entity; }
        set { input.entity = value; }
    }
    public T plan
    {
        get { return input.plan; }
        set { input.plan = value; }
    }
    
}