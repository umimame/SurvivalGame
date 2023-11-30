//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.5.1
//     from Assets/Resources/Script/KeyMap/KeyMap.inputactions
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

public partial class @KeyMap: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @KeyMap()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""KeyMap"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""98f0fa2e-17bb-414d-9fba-20966ab5ac81"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""ab288dd3-3faf-43a0-906c-6ff2c1bcdb50"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""InputViewPoint"",
                    ""type"": ""Value"",
                    ""id"": ""5fe2b712-9fb7-4b9f-ba0e-dacabe884339"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Running"",
                    ""type"": ""Value"",
                    ""id"": ""a2430b69-b197-43aa-a9ee-52e8722cea9f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Attack1"",
                    ""type"": ""Button"",
                    ""id"": ""c04d27fc-35f0-4190-a6fa-dca1a94fcb71"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Damage"",
                    ""type"": ""Button"",
                    ""id"": ""6be77c0d-82f3-4e47-8ca4-4bec4f03d932"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""0c2fba32-5ff7-4d8c-80c3-bf9453d7a22b"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""4da8e1c2-b7d2-484e-86b0-12f26ee7b264"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""b966baa6-7126-43f1-9fe5-2c520f7149f8"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keybord"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""82481a77-b49c-4451-a67d-67e3d878edfb"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keybord"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""62148378-e887-4627-9e18-6aaf1bf15680"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keybord"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""bfa5cdc2-bacb-47cf-bda2-ab64779c8e9b"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keybord"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""41aadf68-8454-4b3b-9022-cb9dd6c1e1d0"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""Running"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e80bef43-66cb-4b69-b010-bc8777387155"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keybord"",
                    ""action"": ""Running"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a050fd40-dc2e-4ade-9b1f-1197351afe99"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""Attack1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""932b160b-f19e-493d-8fd9-121521ef608e"",
                    ""path"": ""<Keyboard>/j"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keybord"",
                    ""action"": ""Attack1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""679cfc41-ea37-4fad-989e-01591314d869"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""Damage"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""da82542c-0cb5-4c09-b20b-33dfa3adedbc"",
                    ""path"": ""<Keyboard>/g"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keybord"",
                    ""action"": ""Damage"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3b046c92-92d5-4762-a740-6c19252e839e"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""InputViewPoint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d2c9efcc-78ad-4001-a1d4-411552e10d53"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""InputViewPoint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5fc72ef5-4c90-4808-aed7-efa441222b1a"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keybord"",
                    ""action"": ""InputViewPoint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""GamePad"",
            ""bindingGroup"": ""GamePad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Keybord"",
            ""bindingGroup"": ""Keybord"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Move = m_Player.FindAction("Move", throwIfNotFound: true);
        m_Player_InputViewPoint = m_Player.FindAction("InputViewPoint", throwIfNotFound: true);
        m_Player_Running = m_Player.FindAction("Running", throwIfNotFound: true);
        m_Player_Attack1 = m_Player.FindAction("Attack1", throwIfNotFound: true);
        m_Player_Damage = m_Player.FindAction("Damage", throwIfNotFound: true);
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

    // Player
    private readonly InputActionMap m_Player;
    private List<IPlayerActions> m_PlayerActionsCallbackInterfaces = new List<IPlayerActions>();
    private readonly InputAction m_Player_Move;
    private readonly InputAction m_Player_InputViewPoint;
    private readonly InputAction m_Player_Running;
    private readonly InputAction m_Player_Attack1;
    private readonly InputAction m_Player_Damage;
    public struct PlayerActions
    {
        private @KeyMap m_Wrapper;
        public PlayerActions(@KeyMap wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Player_Move;
        public InputAction @InputViewPoint => m_Wrapper.m_Player_InputViewPoint;
        public InputAction @Running => m_Wrapper.m_Player_Running;
        public InputAction @Attack1 => m_Wrapper.m_Player_Attack1;
        public InputAction @Damage => m_Wrapper.m_Player_Damage;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @InputViewPoint.started += instance.OnInputViewPoint;
            @InputViewPoint.performed += instance.OnInputViewPoint;
            @InputViewPoint.canceled += instance.OnInputViewPoint;
            @Running.started += instance.OnRunning;
            @Running.performed += instance.OnRunning;
            @Running.canceled += instance.OnRunning;
            @Attack1.started += instance.OnAttack1;
            @Attack1.performed += instance.OnAttack1;
            @Attack1.canceled += instance.OnAttack1;
            @Damage.started += instance.OnDamage;
            @Damage.performed += instance.OnDamage;
            @Damage.canceled += instance.OnDamage;
        }

        private void UnregisterCallbacks(IPlayerActions instance)
        {
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @InputViewPoint.started -= instance.OnInputViewPoint;
            @InputViewPoint.performed -= instance.OnInputViewPoint;
            @InputViewPoint.canceled -= instance.OnInputViewPoint;
            @Running.started -= instance.OnRunning;
            @Running.performed -= instance.OnRunning;
            @Running.canceled -= instance.OnRunning;
            @Attack1.started -= instance.OnAttack1;
            @Attack1.performed -= instance.OnAttack1;
            @Attack1.canceled -= instance.OnAttack1;
            @Damage.started -= instance.OnDamage;
            @Damage.performed -= instance.OnDamage;
            @Damage.canceled -= instance.OnDamage;
        }

        public void RemoveCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    private int m_GamePadSchemeIndex = -1;
    public InputControlScheme GamePadScheme
    {
        get
        {
            if (m_GamePadSchemeIndex == -1) m_GamePadSchemeIndex = asset.FindControlSchemeIndex("GamePad");
            return asset.controlSchemes[m_GamePadSchemeIndex];
        }
    }
    private int m_KeybordSchemeIndex = -1;
    public InputControlScheme KeybordScheme
    {
        get
        {
            if (m_KeybordSchemeIndex == -1) m_KeybordSchemeIndex = asset.FindControlSchemeIndex("Keybord");
            return asset.controlSchemes[m_KeybordSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnInputViewPoint(InputAction.CallbackContext context);
        void OnRunning(InputAction.CallbackContext context);
        void OnAttack1(InputAction.CallbackContext context);
        void OnDamage(InputAction.CallbackContext context);
    }
}
