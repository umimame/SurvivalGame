using AddClass;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Engine : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider co;
    [field: SerializeField] public SpriteRenderer img { get; private set; }
    [field: SerializeField] public Easing gravityEasing { get; private set; }
    [field: SerializeField] public Vector3 gravityScale { get; set; }   // Inspector‚Åd—Í•ûŒü‚ğw’è‚·‚é
    [field: SerializeField, NonEditable] public Vector3 velocityPlan {  get; set; }
    [field: SerializeField] public UnityAction velocityPlanAction { get; set; }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (co == null) { co = GetComponent<Collider>(); }
        PlanReset();
        gravityEasing.Initialize();
    }

    private void Update()
    {
        PlanReset();
        VelocitySolution();
    }

    public void SetGravity(GravityManager gm)
    {
        gravityEasing.curve = gm.easing;
        gravityScale = gm.gravityScale;
    }

    public void PlanReset()
    {
        velocityPlan = Vector3.zero;
    }

    /// <summary>
    /// velocityPlan‚ğ˜M‚éŠÖ”‚ğ“o˜^‚µA‚»‚ÌŒãˆÚ“®‚³‚¹‚é
    /// </summary>
    private void VelocitySolution()
    {
        velocityPlanAction?.Invoke();
        GravitySolution();
        rb.velocity = velocityPlan;
    }

    private void GravitySolution()
    {
        //if (gravityActive == false) { return; }

        gravityActive = true;
        gravityEasing.Update();
        velocityPlan += gravityScale * gravityEasing.evaluteValue;
    }

    public bool gravityActive
    {
        get { return gravityEasing.active; }
        set { gravityEasing.active = value; }
    }
}
