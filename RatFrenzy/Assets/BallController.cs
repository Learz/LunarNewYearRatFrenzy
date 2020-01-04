using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public RatManager mgr;
    public float speed, sumoSpeed, jumpHeight, sumoJumpHeight;
    public bool isSumo;
    public GameObject ball;
    public GameObject rat;

    private bool isJumping;
    private Rigidbody rb;
    private Animator rAnim;
    private Vector3 vel;
    private float realSpeed, realJumpHeight, ratYOffset;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rAnim = rat.GetComponent<Animator>();
        if (mgr == null) throw new System.Exception("Error: Rat manager not defined");

        rAnim.SetBool("isSumo", isSumo);
        if (!isSumo)
        {
            ball.SetActive(false);
            rb.drag = 20;
            realSpeed = speed * 10;
            realJumpHeight = jumpHeight;
            ratYOffset = 0.5f;
            gameObject.layer = 9;
        }
        else
        {
            realSpeed = sumoSpeed;
            realJumpHeight = sumoJumpHeight;
            ratYOffset = 0.45f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        MoveBall();
        AnimateRat();
    }
    public void Jump()
    {
        if (!isJumping)
        {
            rb.AddForce(Vector3.up * realJumpHeight);
            isJumping = true;
        }

    }

    public void Attack()
    {

    }

    public void Grab()
    {

    }

    //Moves the Sphere RigidBody
    private void MoveBall()
    {
        Vector3 dir;
        Vector3 movement = new Vector3(mgr.move.x, 0.0f, mgr.move.y);
        dir = Camera.main.transform.TransformDirection(movement);
        dir.y = 0;
        rb.AddForce(dir * realSpeed);
        Debug.DrawRay(transform.position, dir, Color.green);
    }

    //Animates the angle and mecanim state of the rat
    private void AnimateRat()
    {
        rat.transform.position = transform.position - new Vector3(0,ratYOffset,0);

        
        if (rb.velocity.magnitude > 0.01)
        {
            vel = rb.velocity;
        }

        float angle = Mathf.Atan2(vel.x, vel.z) * Mathf.Rad2Deg;
        rat.transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);

        rAnim.SetFloat("velocity", rb.velocity.magnitude);

    }

    private void OnCollisionEnter(Collision collision)
    {
        isJumping = false;
    }
}
