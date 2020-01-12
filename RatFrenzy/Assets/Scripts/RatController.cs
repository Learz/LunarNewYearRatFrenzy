using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RatController : GenericController
{
    public AudioClip[] slapSounds, hurtSounds;
    public float groundSpeed, airSpeed, jumpHeight, fallSpeed, groundDrag, airDrag, speedOverride;
    public GameObject rat;
    public GameObject tracker;
    public bool isFixed, controlledJump;
    public RatActions actionButton;


    [HideInInspector]
    public bool isSliding { get; private set; } = false;
    [HideInInspector]
    public bool isAttacking { get; private set; } = false;
    [HideInInspector]
    public bool canGetPoint = true;

    private float speed;
    private Animator rAnim;
    private bool isFallBoosted;
    private RaycastHit hit;
    private float rotationMultiplier = 6f;
    private float slideTime = 0.5f;
    private float slideTimer = 0;
    private bool isBoosting;
    public float boostMeter;
    private float boostSpeed = 3f, boostDepleteSpeed = 20f;

    public enum RatActions
    {
        None,
        Attack,
        Slide,
        Boost
    }

    protected override void Start()
    {
        base.Start();
        if (rb == null) rb = GetComponent<Rigidbody>();
        rAnim = rat.GetComponent<Animator>();
        if (actionButton == RatActions.Boost) winCondition.SetScore(identity, boostMeter);
        /*tracker.GetComponent<MeshRenderer>().material.SetColor("_UnlitColor", ColorPalette.GetColor(Player.Color.Blue));
        rat.GetComponentInChildren<Renderer>().material.SetColor("_EmissiveColor", ColorPalette.GetColor(Player.Color.Blue)*150);*/
    }

    void Update()
    {
        MoveRat();

        tracker.transform.eulerAngles = new Vector3(0, 180, 0);
        if (Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, out hit, Mathf.Infinity, 1 << 10))
        {
            tracker.SetActive(true);
            tracker.transform.position = hit.point + new Vector3(0, 0.005f, 0);
            tracker.transform.eulerAngles = new Vector3(-hit.normal.z * 65, 180, hit.normal.x * 65);
        }
        else
        {
            tracker.SetActive(false);
        }

        if (isSliding && isGrounded) slideTimer += Time.deltaTime;
        if (slideTimer >= slideTime)
        {
            isSliding = false;
            slideTimer = 0;
        }
        if (isBoosting)
        {
            boostMeter -= Time.deltaTime * boostDepleteSpeed;
            if (boostMeter <= 0)
            {
                MultiplySpeed(1 / boostSpeed);
                isBoosting = false;
                boostMeter = 0;
            }
            winCondition.SetScore(identity, boostMeter);
        }

        rAnim.SetBool("isJumping", isJumping);
        rAnim.SetBool("isAttacking", isAttacking);
        rAnim.SetBool("isSliding", isSliding);
    }

    private void FixedUpdate()
    {
        //if (rb.velocity.y <= 0) rb.detectCollisions = true;
    }

    protected override void JumpPressed()
    {
        if (!isSliding && !isJumping && isGrounded)
        {
            isFallBoosted = false;
            speed = airSpeed;
            rb.drag = airDrag;
            rb.AddForce(Vector3.up * jumpHeight);
            isJumping = true;
        }

    }

    protected override void JumpReleased()
    {
        if (!isFallBoosted && controlledJump && rb.velocity.y > 0)
        {
            rb.AddForce(Vector3.down * rb.velocity.y * fallSpeed);
            isFallBoosted = true;
        }
    }

    protected override void InteractPressed()
    {
        base.InteractPressed();
        switch (actionButton)
        {
            case RatActions.Attack:
                isAttacking = true;
                break;
            case RatActions.Slide:
                isSliding = true;
                break;
            case RatActions.Boost:
                if (isBoosting) break;
                if (boostMeter > 0)
                {
                    isBoosting = true;
                    MultiplySpeed(boostSpeed);
                }
                break;
        }
    }

    protected override void InteractReleased()
    {
        base.InteractReleased();
        isSliding = false;
        if (isBoosting) MultiplySpeed(1 / boostSpeed);
        isBoosting = false;
        slideTimer = 0;
    }

    private void MoveRat()
    {
        Vector3 dir;
        Vector3 movement = new Vector3(mgr.move.x, 0.0f, mgr.move.y);
        dir = Camera.main.transform.TransformDirection(movement);
        dir.y = 0;
        dir.Normalize();

        if (isFixed)
        {
            rAnim.SetFloat("velocity", speedOverride);
        }
        else
        {
            rb.AddForce(dir * (speed * 60 * Time.deltaTime));
            rAnim.SetFloat("velocity", rb.velocity.magnitude);
            if (dir.magnitude > 0f)
            {
                //rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(new Vector3(rb.velocity.x, 0.0f, rb.velocity.z)), 10.0f));
                rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dir), speed * rotationMultiplier * Time.deltaTime));
            }
        }
    }
    public void MultiplySpeed(float mul)
    {
        groundSpeed *= mul;
        speed *= mul;
    }

    protected override void PlayerJoined(PlayerInput input)
    {
        base.PlayerJoined(input);
    }

    public override void AddPoint()
    {
        if (canGetPoint) base.AddPoint();
    }
    public void AddBoost(float ammount)
    {
        boostMeter = Mathf.Clamp(boostMeter + ammount, 0, 100);
        winCondition.SetScore(identity, boostMeter);
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        if (isGrounded)
        {
            rb.drag = groundDrag;
            speed = groundSpeed;
            isFallBoosted = false;
        }
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
