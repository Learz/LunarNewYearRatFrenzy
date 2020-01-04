using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RatManager : MonoBehaviour
{
    [SerializeField]
    public RatControls controls;
    public Vector2 move { get; private set; }
    public bool jump { get; private set; }
    public bool interact { get; private set; }
    public RatInputEvent onJump = new RatInputEvent(), onInteract = new RatInputEvent();
    // Start is called before the first frame update
    void Start()
    {

        controls.DefaultRat.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.DefaultRat.Move.performed += ctx => PlayerMoving();
        controls.DefaultRat.Move.canceled += ctx => move = Vector2.zero;
        controls.DefaultRat.Jump.performed += ctx => Jump();

        // Ça c'est facultatif
        controls.DefaultRat.Jump.performed += ctx => jump = true;
        controls.DefaultRat.Jump.canceled += ctx => jump = false;
        controls.DefaultRat.Interact.performed += ctx => Interact();
        controls.DefaultRat.Interact.performed += ctx => interact = true;
        controls.DefaultRat.Interact.canceled += ctx => interact = false;

    }
    private void PlayerMoving()
    {
        Debug.Log("Moved : " + move);
    }
    void Jump()
    {
        Debug.Log("Jump");
        onJump.Invoke();
    }
    void Interact()
    {
        Debug.Log("Interact");
        onInteract.Invoke();
    }
    private void OnEnable()
    {
        if (controls == null) controls = new RatControls();
        controls.DefaultRat.Enable();
    }
    private void OnDisable()
    {
        controls.DefaultRat.Disable();
    }
}

[System.Serializable]
public class RatInputEvent : UnityEvent { };