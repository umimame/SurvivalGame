using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PublicInput : MonoBehaviour
{
    [SerializeField] private PlayerInput input;
    public UnityEvent positiveAction;
    public UnityEvent negativeAction;
    public UnityEvent pauseAction;
    void Start()
    {
        input = GetComponent<PlayerInput>();
    }

    #region PlayerInputÇ…ìoò^Ç≥ÇÍÇÈä÷êî
    private void OnPositive(InputValue inputValue)
    {
        positiveAction?.Invoke();
    }

    private void OnNegative(InputValue inputValue)
    {
        negativeAction?.Invoke();
    }

    private void OnPause(InputValue inputValue)
    {
        pauseAction?.Invoke();
    }
    #endregion
}

