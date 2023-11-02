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
    [field: SerializeField, NonEditable] public Vector3 velocityPlan {  get; set; }
    [field: SerializeField] public UnityAction velocityPlanAction { get; set; }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        co = GetComponent<Collider>();
        PlanReset();
    }

    private void Update()
    {
        PlanReset();
        VelocitySolution();
    }

    public void PlanReset()
    {
        velocityPlan = Vector3.zero;
    }

    /// <summary>
    /// velocityPlanÇòMÇÈä÷êîÇìoò^ÇµÅAÇªÇÃå„à⁄ìÆÇ≥ÇπÇÈ
    /// </summary>
    private void VelocitySolution()
    {
        velocityPlanAction?.Invoke();
        rb.velocity = velocityPlan;
    }
}
