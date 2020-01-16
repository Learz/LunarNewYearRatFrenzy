using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RatController : GenericController
{
    public AudioClip slideSound;
    public AudioClip[] slapSounds, hurtSounds;
    public float groundSpeed, airSpeed, jumpHeight, fallSpeed, groundDrag, airDrag, speedOverride;
    public GameObject rat;
    public GameObject tracker;
    public bool isFixed, controlledJump;
    public RatActions actionButton;
    public Collider hurtBox;
    public Transform holdingPoint;
    public float turningSpeed = 6f;
    public ParticleSystem slideParticles;


    [HideInInspector]
    public bool isSliding { get; private set; } = false;
    [HideInInspector]
    public bool isAttacking { get; private set; } = false;
    [HideInInspector]
    public bool canGetPoint = true;
    [HideInInspector]
    public HoldableObject heldObject;

    float speed;
    Animator rAnim;
    bool isFallBoosted;
    RaycastHit hit;
    float slideTime = 0.5f;
    float slideTimer = 0;
    bool isBoosting;
    float boostMeter;
    float boostSpeed = 3f, boostDepleteSpeed = 20f;
    Vector3 dir;
    AudioSource currentSlideChannel;

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

    protected override void Update()
    {
        base.Update();
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

        if (isSliding && isGrounded)
        {
            if (slideTimer == 0) currentSlideChannel = FadeInSound(slideSound, 0.5f, Random.Range(0.9f, 1.1f), 0.5f, true);
            slideTimer += Time.deltaTime;
        }
        if (slideTimer >= slideTime)
        {
            isSliding = false;
            StopChannel(currentSlideChannel, 0.5f);
            if (slideParticles != null) slideParticles.Stop();
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
        rAnim.SetBool("isSliding", isSliding);
    }

    protected override void JumpPressed()
    {
        base.JumpPressed();
        if (!isSliding && !isJumping && isGrounded && jumpHeight > 0)
        {
            PlaySound(jumpSound, Random.Range(0.9f, 1.1f));
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
        if (heldObject != null)
        {
            heldObject.Drop();
        }
        switch (actionButton)
        {
            case RatActions.Attack:
                isAttacking = true;
                rAnim.SetTrigger("attack");
                Collider[] hits = Physics.OverlapBox(hurtBox.transform.position, hurtBox.transform.localScale / 2);
                if (hits != null)
                {
                    foreach (Collider hit in hits)
                    {
                        RatController player = hit.GetComponent<RatController>();
                        if (player != null && player.identity != identity)
                        {
                            PlaySound(slapSounds, Random.Range(0.9f, 1.1f));
                            player.GetHurt(Vector3.up * 500 + dir * 100);
                        }
                    }
                }
                break;
            case RatActions.Slide:
                rAnim.SetTrigger("slide");
                isSliding = true;
                if (isGrounded && slideParticles != null) slideParticles.Play();
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
        StopChannel(currentSlideChannel, 0.5f);
        if (isBoosting) MultiplySpeed(1 / boostSpeed);
        isBoosting = false;
        slideTimer = 0;
        if (slideParticles != null) slideParticles.Stop();
    }

    private void MoveRat()
    {
        if (isFixed)
        {
            rAnim.SetFloat("velocity", speedOverride);
        }
        else
        {
            if (mgr.move.magnitude > 0f)
            {
                Vector3 movement = new Vector3(mgr.move.x, 0.0f, mgr.move.y);
                dir = Camera.main.transform.TransformDirection(movement);
                dir.y = 0;
                dir.Normalize();
                rb.AddForce(dir * (speed * 60 * Time.deltaTime));
                //rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(new Vector3(rb.velocity.x, 0.0f, rb.velocity.z)), 10.0f));
                rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dir), speed * turningSpeed * Time.deltaTime));
            }

            rAnim.SetFloat("velocity", rb.velocity.magnitude);
        }
    }
    public void MultiplySpeed(float mul)
    {
        groundSpeed *= mul;
        speed *= mul;
    }

    public void GetHurt()
    {
        PlaySound(hurtSounds, Random.Range(0.9f, 1.1f));
        VibrateGamepad(0.5f, 0.25f);
        PlaySound(hurtSounds, Random.Range(0.9f, 1.1f));
        if (heldObject) heldObject.Drop();
    }

    public void GetHurt(Vector3 knockbackDir)
    {
        GetHurt();
        rb.AddForce(knockbackDir);
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
    public override void Kill()
    {
        base.Kill();
        GetComponent<CapsuleCollider>().enabled = false;
        if (slideParticles != null) slideParticles.Stop();
        StopChannel(currentSlideChannel, 0.5f);
    }
    public override void Respawn()
    {
        GetComponent<CapsuleCollider>().enabled = true;
        base.Respawn();
    }
    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        if (isGrounded)
        {
            rb.drag = groundDrag;
            speed = groundSpeed;
            isFallBoosted = false;
            if (isSliding) slideParticles.Play();
        }
    }

    protected override void OnCollisionExit(Collision collision)
    {
        base.OnCollisionExit(collision);
        if (!isGrounded)
        {
            speed = airSpeed;
            rb.drag = airDrag;
            if (isSliding) slideParticles.Stop();
        }

    }
}
