// GENERATED AUTOMATICALLY FROM 'Assets/Input/RatControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @RatControls : IInputActionCollection, IDisposable
{
    private InputActionAsset asset;
    public @RatControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""RatControls"",
    ""maps"": [
        {
            ""name"": ""DefaultRat"",
            ""id"": ""e5b8d4c6-5cea-4381-a0d5-a25136f88a1c"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Button"",
                    ""id"": ""6b6bb6d3-5362-48e3-9f84-e74174f3249c"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MoveCancelled"",
                    ""type"": ""Button"",
                    ""id"": ""6df78c06-a075-4af5-8b41-05b5af6bb44b"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=1)""
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""a7082025-6b63-423c-9ee1-c1e5eb085bd3"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""6aa138a9-b590-4b11-b879-b5714e7d2ce0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""14aeb30f-4799-47ac-ba51-9389bb855844"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone"",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""5428de0e-2d97-4419-b960-0e6ec5a188ab"",
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
                    ""id"": ""2dc5f912-9f8c-4590-b0a8-e2dcdc4e6525"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""5b59c43b-df07-4c38-b415-1bfce7e80076"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""fd175309-2643-45d5-91ea-4b7853721f38"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""948dc783-db68-4f1b-89a1-ab3d7fe4f485"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""6d67ad87-9106-47e6-a99e-fbc3321baf0f"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b86028a4-96f6-4228-9cca-d4533a2d51d6"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""69ad9952-08e2-4801-a23d-83519c68f7e6"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6a93fc5e-9fbe-42d8-8f0d-53a99a054d5d"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""294b29d6-786c-435d-a171-a241202cf7d8"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e66853a2-3ad7-4a2e-8269-e22fe0eb71ac"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone"",
                    ""groups"": """",
                    ""action"": ""MoveCancelled"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""cb96be3d-cc1c-4fda-ab3b-7db0709ae197"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveCancelled"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""08bef79b-a473-4b57-b236-06230b2e05f4"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveCancelled"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""5e1dcb16-9fc2-42ca-b8ff-52ab68e6f85e"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveCancelled"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""a2c738a3-4e65-4c87-a281-8b85a01997dc"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveCancelled"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""42b22caf-1bdb-43ad-bd2c-b75856294496"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveCancelled"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // DefaultRat
        m_DefaultRat = asset.FindActionMap("DefaultRat", throwIfNotFound: true);
        m_DefaultRat_Move = m_DefaultRat.FindAction("Move", throwIfNotFound: true);
        m_DefaultRat_MoveCancelled = m_DefaultRat.FindAction("MoveCancelled", throwIfNotFound: true);
        m_DefaultRat_Jump = m_DefaultRat.FindAction("Jump", throwIfNotFound: true);
        m_DefaultRat_Interact = m_DefaultRat.FindAction("Interact", throwIfNotFound: true);
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

    // DefaultRat
    private readonly InputActionMap m_DefaultRat;
    private IDefaultRatActions m_DefaultRatActionsCallbackInterface;
    private readonly InputAction m_DefaultRat_Move;
    private readonly InputAction m_DefaultRat_MoveCancelled;
    private readonly InputAction m_DefaultRat_Jump;
    private readonly InputAction m_DefaultRat_Interact;
    public struct DefaultRatActions
    {
        private @RatControls m_Wrapper;
        public DefaultRatActions(@RatControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_DefaultRat_Move;
        public InputAction @MoveCancelled => m_Wrapper.m_DefaultRat_MoveCancelled;
        public InputAction @Jump => m_Wrapper.m_DefaultRat_Jump;
        public InputAction @Interact => m_Wrapper.m_DefaultRat_Interact;
        public InputActionMap Get() { return m_Wrapper.m_DefaultRat; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DefaultRatActions set) { return set.Get(); }
        public void SetCallbacks(IDefaultRatActions instance)
        {
            if (m_Wrapper.m_DefaultRatActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_DefaultRatActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_DefaultRatActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_DefaultRatActionsCallbackInterface.OnMove;
                @MoveCancelled.started -= m_Wrapper.m_DefaultRatActionsCallbackInterface.OnMoveCancelled;
                @MoveCancelled.performed -= m_Wrapper.m_DefaultRatActionsCallbackInterface.OnMoveCancelled;
                @MoveCancelled.canceled -= m_Wrapper.m_DefaultRatActionsCallbackInterface.OnMoveCancelled;
                @Jump.started -= m_Wrapper.m_DefaultRatActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_DefaultRatActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_DefaultRatActionsCallbackInterface.OnJump;
                @Interact.started -= m_Wrapper.m_DefaultRatActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_DefaultRatActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_DefaultRatActionsCallbackInterface.OnInteract;
            }
            m_Wrapper.m_DefaultRatActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @MoveCancelled.started += instance.OnMoveCancelled;
                @MoveCancelled.performed += instance.OnMoveCancelled;
                @MoveCancelled.canceled += instance.OnMoveCancelled;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
            }
        }
    }
    public DefaultRatActions @DefaultRat => new DefaultRatActions(this);
    public interface IDefaultRatActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnMoveCancelled(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
    }
}
