using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Chara_Player : Chara
{
    [SerializeField] private PlayerInput input;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }


    public void OnMove(InputValue value)
    {
        inputVelocityPlan = new Vector3(value.Get<Vector2>().x, 0.0f, value.Get<Vector2>().y) * speed.entity;
        Debug.Log("value");
    }



}
