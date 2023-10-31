using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Engine : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider co;
    [field: SerializeField, NonEditable] public Vector3 velocityPlan {  get; set; }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        co = GetComponent<Collider>();
        velocityPlan = Vector3.zero;
    }

    private void Update()
    {
        
    }
}
