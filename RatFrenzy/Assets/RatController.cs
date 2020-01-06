using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RatController : GenericController
{
    public float groundSpeed, airSpeed, jumpHeight, groundDrag, airDrag, fixedSpeed;
    public Rigidbody rb;
    public GameObject rat;
    public bool isFixed;

    private float speed;
    private Animator rAnim;

    protected override void Start()
    {
        base.Start();
        if (rb == null) rb = GetComponent<Rigidbody>();
        rAnim = rat.GetComponent<Animator>();
    }

    void Update()
    {
        MoveRat();
    }

    protected override void Jump()
    {
        if (!isJumping && isGrounded)
        {
            speed = airSpeed;
            rb.drag = airDrag;
            rb.AddForce(Vector3.up * jumpHeight);
            isJumping = true;
            rAnim.SetBool("isJumping", isJumping);
        }

    }

    protected override void Attack()
    {

    }

    protected override void Grab()
    {

    }

    private void MoveRat()
    {
        Vector3 dir;
        Vector3 movement = new Vector3(mgr.move.x, 0.0f, mgr.move.y);
        dir = Camera.main.transform.TransformDirection(movement);
        dir.y = 0;

        if (!isFixed)
        {
            rb.AddForce(dir * (speed * 60 * Time.deltaTime));
            rAnim.SetFloat("velocity", rb.velocity.magnitude);
        }
        else
        {
            rAnim.SetFloat("velocity", fixedSpeed);
        }

        if (dir.magnitude > 0f)
        {
            //rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(new Vector3(rb.velocity.x, 0.0f, rb.velocity.z)), 10.0f));
            rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dir), 10.0f * 60 * Time.deltaTime));
        }

    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        if (isGrounded)
        {
            rb.drag = groundDrag;
            speed = groundSpeed;
        }
        rAnim.SetBool("isJumping", isJumping);
    }

    protected override void OnCollisionExit(Collision collision)
    {
        base.OnCollisionExit(collision);
        if (!isGrounded)
        {
            speed = airSpeed;
            rb.drag = airDrag;
        }
        
    }
}
