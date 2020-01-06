using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallController : GenericController
{

    public float speed, jumpHeight;
    public Rigidbody rb;
    public GameObject ball;
    public GameObject rat;

    private Animator rAnim;
    private Vector3 vel;
    private float ratYOffset;

    private const float defaultDrag = 20;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        if (rb == null) rb = GetComponent<Rigidbody>();
        rAnim = rat.GetComponent<Animator>();

        ratYOffset = 0.45f;
    }

    void Update()
    {
        MoveBall();
        AnimateRat();
    }

    protected override void JumpPressed()
    {
        if (!isJumping && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpHeight);
            isJumping = true;
            rAnim.SetBool("isJumping", isJumping);
        }

    }

    protected override void InteractPressed()
    {

    }

    protected override void Grab()
    {

    }

    //Moves the Sphere RigidBody
    private void MoveBall()
    {
        Vector3 dir;
        Vector3 movement = new Vector3(mgr.move.x, 0.0f, mgr.move.y);
        dir = Camera.main.transform.TransformDirection(movement);
        dir.y = 0;
        rb.AddForce(dir * speed * Time.deltaTime);
        Debug.DrawRay(transform.position, dir, Color.green);
    }

    //Animates the angle and mecanim state of the rat
    private void AnimateRat()
    {
        rat.transform.position = ball.transform.position - new Vector3(0, ratYOffset, 0);


        if (rb.velocity.magnitude > 0.01)
        {
            vel = rb.velocity;
        }

        float angle = Mathf.Atan2(vel.x, vel.z) * Mathf.Rad2Deg;
        rat.transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);

        rAnim.SetFloat("velocity", rb.velocity.magnitude);
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        rAnim.SetBool("isJumping", isJumping);
    }

}
