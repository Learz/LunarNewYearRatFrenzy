using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Doozy.Engine.UI;

public class RatManager : MonoBehaviour
{

    public Vector2 move { get; private set; }
    public bool jump { get; private set; }
    public bool interact { get; private set; }
    public RatInputEvent onMove = new RatInputEvent(),
        onJumpDown = new RatInputEvent(),
        onJumpUp = new RatInputEvent(),
        onInteractDown = new RatInputEvent(),
        onInteractUp = new RatInputEvent();
    private PlayerInput playerInput;
    public UIButton escape;

    private void Start()
    {
        DontDestroyOnLoad(this);
        playerInput = GetComponent<PlayerInput>();
        this.name = "Player " + (playerInput.playerIndex + 1);
    }
    private void OnMove(InputValue value)
    {
        move = value.Get<Vector2>();
        onMove.Invoke();
    }
    private void OnMoveCancelled()
    {
        move = Vector2.zero;
    }
    private void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            onJumpDown.Invoke();
            return;
        }
        onJumpUp.Invoke();
    }
    private void OnInteract(InputValue value)
    {
        if (value.isPressed) onInteractDown.Invoke();
        else onInteractUp.Invoke();

    }
    private void OnPause(InputValue value)
    {
        if (value.isPressed)
            escape.ExecuteClick();
    }

}

[System.Serializable]
public class RatInputEvent : UnityEvent { };
