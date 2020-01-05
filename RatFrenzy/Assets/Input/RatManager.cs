using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class RatManager : MonoBehaviour
{

    public Vector2 move { get; private set; }
    public bool jump { get; private set; }
    public bool interact { get; private set; }
    private float cooldown = 0.5f;
    private bool interactCooldown;
    public RatInputEvent onMove = new RatInputEvent(), onJump = new RatInputEvent(), onInteract = new RatInputEvent();
    private PlayerInput playerInput;

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
    private void OnJump()
    {
        onJump.Invoke();
    }
    private void OnInteract()
    {
        if (!interactCooldown)
        {
            onInteract.Invoke();
            Invoke("InteractCooldown", cooldown);
            interactCooldown = true;
        }

    }
    private void InteractCooldown()
    {
        interactCooldown = false;
    }

}

[System.Serializable]
public class RatInputEvent : UnityEvent { };