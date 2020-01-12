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
    public AudioSource audioSource;
    public AudioClip collisionSound, deathSound;
    public AudioClip[] squeakSounds;
    public bool vibrateOnCollision;

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
        audioSource = GetComponent<AudioSource>();
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
            mgr.onSqueak.AddListener(SqueakPressed);
            UpdateColor();
        }
    }
    public virtual void Kill()
    {
        if (audioSource != null && deathSound != null)
        {
            audioSource.clip = deathSound;
            audioSource.pitch = Time.timeScale * Random.Range(0.9f, 1f);
            audioSource.volume = 1f;
            audioSource.Play();
        }
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
        VibrateGamepad(0.1f, 1f);
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
    protected virtual void SqueakPressed()
    {
        if (audioSource != null && squeakSounds != null)
        {
            Debug.Log("Squeaking");
            audioSource.clip = squeakSounds[Random.Range(0, squeakSounds.Length)];
            audioSource.pitch = 1 + mgr.squeakPitch;
            audioSource.volume = 1f;
            audioSource.Play();
        }
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
            mgr.onSqueak.AddListener(SqueakPressed);
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

        if (collision.relativeVelocity.magnitude > 2)
        {
            if (audioSource != null && collisionSound != null)
            {
                audioSource.clip = collisionSound;
                audioSource.pitch = Time.timeScale * Random.Range(0.9f, 1.2f);
                audioSource.volume = 0.1f * collision.relativeVelocity.magnitude;
                audioSource.Play();
            }
            if (vibrateOnCollision) VibrateGamepad(collision.relativeVelocity.magnitude / 50, collision.relativeVelocity.magnitude / 35);
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
    protected void VibrateGamepad(float intensity, float duration)
    {
        foreach (Gamepad pad in Gamepad.all)
        {
            if (mgr.deviceId == pad.deviceId)
            {
                StartCoroutine(Vibrate(pad, intensity, duration));
            }
        }
    }
    protected IEnumerator Vibrate(Gamepad pad, float intensity, float duration)
    {
        float t = 01;
        float ammount;
        while (t > 0)
        {
            t -= Time.deltaTime / duration;
            ammount = Mathf.Lerp(0, 1, t);
            Debug.Log(ammount);
            pad.SetMotorSpeeds(ammount, ammount / 2);
            yield return null;
        }
        pad.SetMotorSpeeds(0, 0);
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