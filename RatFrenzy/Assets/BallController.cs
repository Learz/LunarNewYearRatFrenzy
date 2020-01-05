using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallController : MonoBehaviour
{
    public GameManager.PlayerIdentity identity;
    public float speed, sumoSpeed, jumpHeight, sumoJumpHeight;
    public bool isSumo;
    public GameObject ball;
    public GameObject rat;

    public RatManager mgr;
    private bool isJumping;
    private Rigidbody rb;
    private Animator rAnim;
    private Vector3 vel;
    private float realSpeed, realJumpHeight, ratYOffset;

    private const float defaultDrag = 20;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rAnim = rat.GetComponent<Animator>();
        if (mgr == null)
        {
            // Create Manager if it doesn't exist. Used for testing scenes.
            if (GameManager.instance == null) GameManager.CreateTestManager();
            GameManager.instance.playerJoined.AddListener(PlayerJoined);

            mgr = GameManager.instance.GetRatManager(identity);
            if (mgr == null) transform.parent.gameObject.SetActive(false);
            else
            {
                mgr.onJump.AddListener(Jump);
                mgr.onInteract.AddListener(Attack);
            }
        }

        rAnim.SetBool("isSumo", isSumo);
        if (!isSumo)
        {
            ball.SetActive(false);
            rb.drag = defaultDrag;
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

    void FixedUpdate()
    {
        MoveBall();
        AnimateRat();
    }

    public void Jump()
    {
        if (!isJumping)
        {
            if (!isSumo)
            {
                rb.drag = 1;
                realSpeed = speed;
            }

            rb.AddForce(Vector3.up * realJumpHeight);
            isJumping = true;
            rAnim.SetBool("isJumping", isJumping);
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
        rat.transform.position = transform.position - new Vector3(0, ratYOffset, 0);


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
        rAnim.SetBool("isJumping", isJumping);
        if (!isSumo)
        {
            rb.drag = defaultDrag;
            realSpeed = speed * 10;
        }
    }

    // This method enables the controller when the corresponding player joins. This is used for testing.
    private void PlayerJoined(PlayerInput input)
    {
        if (input.playerIndex == (int)identity)
        {
            mgr = input.GetComponent<RatManager>();
            transform.parent.gameObject.SetActive(true);
            mgr.onJump.AddListener(Jump);
            mgr.onInteract.AddListener(Attack);
        }
    }
}
