using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GenericController : MonoBehaviour
{
    public Player.Identity identity;
    public GenericWinCondition winCondition;
    public List<RendererProperties> renderers;
    public int numberOfRenderersToHideOnKill;
    public List<Light> lights;
    public bool respawn;
    public float respawnTime = 2f;
    public Rigidbody rb;
    protected Vector3 respawnPosition;
    protected Quaternion respawnRotation;
    public Cinemachine.CinemachineTargetGroup targetGroup;
    private AudioSource collisionSound;

    [HideInInspector]
    public bool isJumping { get; protected set; }
    [HideInInspector]
    public bool isGrounded { get; protected set; }

    protected bool isDead;
    protected RatManager mgr;
    protected int grounds;
    protected ParticleSystem ps;
    protected Cinemachine.CinemachineImpulseSource impulse;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        collisionSound = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        if (respawn) SetRespawnPosition(transform.position, transform.rotation);
        if (winCondition == null) winCondition = FindObjectOfType<GenericWinCondition>();
        ps = GetComponent<ParticleSystem>();
        impulse = GetComponent<Cinemachine.CinemachineImpulseSource>();

        // Create Manager if it doesn't exist. Used for testing scenes.
        if (GameManager.instance == null) GameManager.CreateTestManager();
        GameManager.instance.playerJoined.AddListener(PlayerJoined);

        mgr = GameManager.instance.GetRatManager(identity);
        if (mgr == null) this.gameObject.SetActive(false);
        else
        {
            if (targetGroup != null) targetGroup.AddMember(this.transform, 1, 2);
            mgr.onJumpDown.AddListener(JumpPressed);
            mgr.onJumpUp.AddListener(JumpReleased);
            mgr.onInteractDown.AddListener(InteractPressed);
            mgr.onInteractUp.AddListener(InteractReleased);
            UpdateColor();
        }
    }
    public virtual void Kill()
    {
        if (isDead) return;
        isDead = true;
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        if (ps != null) ps.Play();
        if (impulse != null) impulse.GenerateImpulse();
        for (int i = 0; i < numberOfRenderersToHideOnKill; i++)
        {
            renderers[i].renderer.enabled = false;
        }
        if (respawn) Invoke("Respawn", respawnTime);
    }
    public virtual void Respawn()
    {
        isDead = false;
        transform.position = respawnPosition;
        transform.rotation = respawnRotation;
        for (int i = 0; i < numberOfRenderersToHideOnKill; i++)
        {
            renderers[i].renderer.enabled = true;
        }
        rb.isKinematic = false;
    }
    public virtual void SetRespawnPosition(Vector3 pos, Quaternion rot)
    {
        respawnPosition = pos;
        respawnRotation = rot;
    }
    protected virtual void UpdateColor()
    {
        foreach (RendererProperties rend in renderers)
        {
            string property = "";
            int multiply = 1;
            switch (rend.property)
            {
                case RendererProperties.Property.BaseColor:
                    property = "_BaseColor";
                    break;
                case RendererProperties.Property.EmissiveColor:
                    property = "_EmissiveColor";
                    multiply = 150;
                    break;
                case RendererProperties.Property.UnlitColor:
                    property = "_UnlitColor";
                    break;
            }
            foreach (Material mat in rend.renderer.materials)
            {
                Color col = mgr.GetPlayerColor();
                col.a = mat.color.a;
                mat.SetColor(property, col * multiply);
            }
        }
        foreach (Light light in lights)
        {
            light.color = mgr.GetPlayerColor();
        }
    }
    protected virtual void JumpPressed()
    {
        Debug.Log("Jump Pressed");
    }
    protected virtual void JumpReleased()
    {
        Debug.Log("Jump Released");
    }
    protected virtual void InteractPressed()
    {
        Debug.Log("Interact Pressed");
    }
    protected virtual void InteractReleased()
    {
        Debug.Log("Interact Released");
    }
    // This method enables the controller when the corresponding player joins. This is used for testing.
    protected virtual void PlayerJoined(PlayerInput input)
    {
        if (input.playerIndex == (int)identity)
        {
            if (targetGroup != null)
            {
                targetGroup.AddMember(this.transform, 1, 2);
            }
            mgr = input.GetComponent<RatManager>();
            this.gameObject.SetActive(true);
            mgr.onJumpDown.AddListener(JumpPressed);
            mgr.onJumpUp.AddListener(JumpReleased);
            mgr.onInteractDown.AddListener(InteractPressed);
            mgr.onInteractUp.AddListener(InteractReleased);
            mgr.color = (Player.Color)System.Enum.ToObject(typeof(Player.Color), Random.Range(0, System.Enum.GetValues(typeof(Player.Color)).Length - 1));
            GameManager.instance.ShowPlayerHud(identity);
            UpdateColor();


        }
    }
    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 10) grounds++;
        isGrounded = grounds == 0 ? false : true;
        if (isGrounded) isJumping = false;
        if (collisionSound != null)
        {
            if (collision.relativeVelocity.magnitude > 2)
            {
                collisionSound.pitch = Time.timeScale * Random.Range(0.9f, 1.2f);
                collisionSound.volume = 0.1f * collision.relativeVelocity.magnitude;
                collisionSound.Play();

            }
        }
    }

    protected virtual void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 10) grounds--;
        isGrounded = grounds == 0 ? false : true;
    }

    public virtual void AddPoint()
    {

        winCondition.AddPoint(identity);
    }
}
[System.Serializable]
public class RendererProperties
{
    public Renderer renderer;
    public Property property;
    public enum Property
    {
        BaseColor,
        EmissiveColor,
        UnlitColor
    }
}