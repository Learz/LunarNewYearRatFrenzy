using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatManager : MonoBehaviour
{
    [SerializeField]
    public RatControls controls;
    private Vector2 move;
    private bool jump, interact;
    // Start is called before the first frame update
    void Start()
    {

        controls.DefaultRat.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.DefaultRat.Move.performed += ctx => PlayerMoving();
        controls.DefaultRat.Jump.performed += ctx => Jump();

        // Ça c'est facultatif
        controls.DefaultRat.Jump.performed += ctx => jump = true;
        controls.DefaultRat.Jump.canceled += ctx => jump = false;
        controls.DefaultRat.Interact.performed += ctx => Interact();
        controls.DefaultRat.Interact.performed += ctx => interact = true;
        controls.DefaultRat.Interact.canceled += ctx => interact = false;

    }
    // Update is called once per frame
    void Update()
    {

    }
    private void PlayerMoving()
    {
        Debug.Log("Moved : " + move);
    }
    void Jump()
    {
        Debug.Log("Jump");
    }
    void Interact()
    {
        Debug.Log("Interact");
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
