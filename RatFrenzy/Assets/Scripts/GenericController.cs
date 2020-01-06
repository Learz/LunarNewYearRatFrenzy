﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GenericController : MonoBehaviour
{
    public GameManager.PlayerIdentity identity;
    protected RatManager mgr;
    protected bool isJumping, isGrounded;
    protected int grounds;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        if (mgr == null)
        {
            // Create Manager if it doesn't exist. Used for testing scenes.
            if (GameManager.instance == null) GameManager.CreateTestManager();
            GameManager.instance.playerJoined.AddListener(PlayerJoined);

            mgr = GameManager.instance.GetRatManager(identity);
            if (mgr == null) this.gameObject.SetActive(false);
            else
            {
                mgr.onJumpDown.AddListener(JumpPressed);
                mgr.onJumpUp.AddListener(JumpReleased);
                mgr.onInteractDown.AddListener(InteractPressed);
                mgr.onInteractUp.AddListener(InteractReleased);
                mgr.onInteract.AddListener(Interact);
            }
        }
    }

    protected virtual void JumpPressed()
    {
        Debug.Log("Jump Pressed");
    }
    protected virtual void JumpReleased()
    {
        Debug.Log("Jump Released");
    }
    protected virtual void InteractPressed()
    {
        Debug.Log("Interact Pressed");
    }
    protected virtual void InteractReleased()
    {
        Debug.Log("Interact Released");
    }
    protected virtual void Interact(bool isPressed)
    {
        if (isPressed) Debug.Log("Interact down");
        else Debug.Log("Interact up");
    }
    // This method enables the controller when the corresponding player joins. This is used for testing.
    protected virtual void PlayerJoined(PlayerInput input)
    {
        if (input.playerIndex == (int)identity)
        {
            mgr = input.GetComponent<RatManager>();
            this.gameObject.SetActive(true);
            mgr.onJumpDown.AddListener(JumpPressed);
            mgr.onInteractDown.AddListener(InteractPressed);
        }
    }
    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 10) grounds++;
        isGrounded = grounds == 0 ? false : true;
        if (isGrounded) isJumping = false;
    }

    protected virtual void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 10) grounds--;
        isGrounded = grounds == 0 ? false : true;
    }
}
