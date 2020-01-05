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


    private bool isJumping;
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
        if (!isJumping)
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

        if (rb.velocity.magnitude > 0.1f)
        {
            //rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(new Vector3(rb.velocity.x, 0.0f, rb.velocity.z)), 10.0f));
            rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dir), 10.0f));
        }

        
        if (!isFixed)
        {
            rb.AddForce(dir * speed);
            rAnim.SetFloat("velocity", rb.velocity.magnitude);
        }
        else
        {
            rAnim.SetFloat("velocity", fixedSpeed);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 10)
        {
            isJumping = false;
            rAnim.SetBool("isJumping", isJumping);
            rb.drag = groundDrag;
            speed = groundSpeed;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 10)
        {
            speed = airSpeed;
            rb.drag = airDrag;
        }
        
    }
}
