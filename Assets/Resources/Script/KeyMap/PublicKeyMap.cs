//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.5.1
//     from Assets/Assets/Resources/Script/KeyMap/PublicKeyMap.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PublicKeyMap: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PublicKeyMap()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PublicKeyMap"",
    ""maps"": [
        {
            ""name"": ""Public"",
            ""id"": ""a61f06e4-f89c-4b70-a65e-c6b832f39171"",
            ""actions"": [
                {
                    ""name"": ""Positive"",
                    ""type"": ""Button"",
                    ""id"": ""a2591c1b-6e89-432e-b9c6-60ce7f0d6be2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Negative"",
                    ""type"": ""Button"",
                    ""id"": ""66b6ca05-ba55-4292-a888-174ba5657345"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""5ea9bb97-c4a5-458f-9d41-b3c7e7888fc6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""3ea206dd-51cc-4171-ab26-8669252002f6"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Public"",
                    ""action"": ""Positive"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""843cbca9-c1e0-4b70-8f21-229d9eb8616c"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Public"",
                    ""action"": ""Negative"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d8a95c67-c80d-4e8f-94de-6581a105ab4e"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Public"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Public"",
            ""bindingGroup"": ""Public"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Public
        m_Public = asset.FindActionMap("Public", throwIfNotFound: true);
        m_Public_Positive = m_Public.FindAction("Positive", throwIfNotFound: true);
        m_Public_Negative = m_Public.FindAction("Negative", throwIfNotFound: true);
        m_Public_Pause = m_Public.FindAction("Pause", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Public
    private readonly InputActionMap m_Public;
    private List<IPublicActions> m_PublicActionsCallbackInterfaces = new List<IPublicActions>();
    private readonly InputAction m_Public_Positive;
    private readonly InputAction m_Public_Negative;
    private readonly InputAction m_Public_Pause;
    public struct PublicActions
    {
        private @PublicKeyMap m_Wrapper;
        public PublicActions(@PublicKeyMap wrapper) { m_Wrapper = wrapper; }
        public InputAction @Positive => m_Wrapper.m_Public_Positive;
        public InputAction @Negative => m_Wrapper.m_Public_Negative;
        public InputAction @Pause => m_Wrapper.m_Public_Pause;
        public InputActionMap Get() { return m_Wrapper.m_Public; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PublicActions set) { return set.Get(); }
        public void AddCallbacks(IPublicActions instance)
        {
            if (instance == null || m_Wrapper.m_PublicActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PublicActionsCallbackInterfaces.Add(instance);
            @Positive.started += instance.OnPositive;
            @Positive.performed += instance.OnPositive;
            @Positive.canceled += instance.OnPositive;
            @Negative.started += instance.OnNegative;
            @Negative.performed += instance.OnNegative;
            @Negative.canceled += instance.OnNegative;
            @Pause.started += instance.OnPause;
            @Pause.performed += instance.OnPause;
            @Pause.canceled += instance.OnPause;
        }

        private void UnregisterCallbacks(IPublicActions instance)
        {
            @Positive.started -= instance.OnPositive;
            @Positive.performed -= instance.OnPositive;
            @Positive.canceled -= instance.OnPositive;
            @Negative.started -= instance.OnNegative;
            @Negative.performed -= instance.OnNegative;
            @Negative.canceled -= instance.OnNegative;
            @Pause.started -= instance.OnPause;
            @Pause.performed -= instance.OnPause;
            @Pause.canceled -= instance.OnPause;
        }

        public void RemoveCallbacks(IPublicActions instance)
        {
            if (m_Wrapper.m_PublicActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPublicActions instance)
        {
            foreach (var item in m_Wrapper.m_PublicActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PublicActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PublicActions @Public => new PublicActions(this);
    private int m_PublicSchemeIndex = -1;
    public InputControlScheme PublicScheme
    {
        get
        {
            if (m_PublicSchemeIndex == -1) m_PublicSchemeIndex = asset.FindControlSchemeIndex("Public");
            return asset.controlSchemes[m_PublicSchemeIndex];
        }
    }
    public interface IPublicActions
    {
        void OnPositive(InputAction.CallbackContext context);
        void OnNegative(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
    }
}
