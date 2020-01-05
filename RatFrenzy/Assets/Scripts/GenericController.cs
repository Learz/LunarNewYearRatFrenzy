using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GenericController : MonoBehaviour
{
    public GameManager.PlayerIdentity identity;
    protected RatManager mgr;
    protected bool isJumping, isGrounded;

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
                mgr.onJump.AddListener(Jump);
                mgr.onInteract.AddListener(Attack);
            }
        }
    }

    protected virtual void Jump()
    {

    }

    protected virtual void Attack()
    {

    }

    protected virtual void Grab()
    {

    }
    // This method enables the controller when the corresponding player joins. This is used for testing.
    protected virtual void PlayerJoined(PlayerInput input)
    {
        if (input.playerIndex == (int)identity)
        {
            mgr = input.GetComponent<RatManager>();
            this.gameObject.SetActive(true);
            mgr.onJump.AddListener(Jump);
            mgr.onInteract.AddListener(Attack);
        }
    }
    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 10 && !isGrounded) isGrounded = true;
        isJumping = false;
    }

    protected virtual void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 10 && isGrounded) isGrounded = false;
    }
}
