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
        onMoveCancelled = new RatInputEvent(),
        onJumpDown = new RatInputEvent(),
        onJumpUp = new RatInputEvent(),
        onInteractDown = new RatInputEvent(),
        onInteractUp = new RatInputEvent();
    private PlayerInput playerInput;
    public int poseIndex;
    public UIButton escape;
    public Player.Color color;
    public int deviceId { get; private set; }

    private void Start()
    {
        DontDestroyOnLoad(this);
        playerInput = GetComponent<PlayerInput>();
        deviceId = playerInput.devices[0].deviceId;
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
        onMoveCancelled.Invoke();
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

    public Color GetPlayerColor()
    {
        return ColorPalette.GetColor(color);
    }
    public Sprite GetPoseSprite()
    {
        return GameManager.instance.playerPoses[poseIndex];
    }
    private void OnApplicationFocus(bool focus)
    {
        if (!focus) move = Vector2.zero;
    }
}

[System.Serializable]
public class RatInputEvent : UnityEvent { };
