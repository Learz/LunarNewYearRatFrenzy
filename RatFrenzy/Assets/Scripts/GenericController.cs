using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

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
    public AudioClip collisionSound, deathSound, jumpSound;
    public AudioClip[] squeakSounds;
    public int audioChannels = 5;
    public bool vibrateOnCollision;

    [HideInInspector]
    public bool isJumping { get; protected set; }
    [HideInInspector]
    public bool isGrounded { get; protected set; }
    [HideInInspector]
    public RatManager mgr { get; protected set; }
    [HideInInspector]
    public bool isDead { get; protected set; }

    protected int grounds;
    protected ParticleSystem ps;
    protected Cinemachine.CinemachineImpulseSource impulse;

    AudioSource[] audioSources;
    int nextChannel = 0;

    protected virtual void Awake()
    {
        if (identity != Player.Identity.Leader) return;
        if (GameManager.instance != null) identity = GameManager.instance.GetLeader();
        else identity = Player.Identity.Player1;
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        audioSources = new AudioSource[audioChannels];
        for (int i = 0; i < audioChannels; i++)
        {
            audioSources[i] = gameObject.AddComponent<AudioSource>();
        }
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
        if (isDead) return;
        PlaySound(deathSound, Time.timeScale * Random.Range(0.9f, 1f));
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

        // Moved CapsuleCollider related stuff to RatController
        // GetComponent<CapsuleCollider>().enabled = false;
    }
    public virtual void Respawn()
    {
        // GetComponent<CapsuleCollider>().enabled = true;
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
                /*foreach (string str in mat.GetTexturePropertyNames())
                {
                    Debug.Log(rend.renderer.gameObject.name + " property : " + str);
                }*/
                col.a = mat.GetColor(property).a;

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
        PlaySound(squeakSounds, 1 + mgr.squeakPitch);
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
            PlaySound(collisionSound, Time.timeScale * Random.Range(0.9f, 1.2f), 0.1f * collision.relativeVelocity.magnitude);
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
    public void VibrateGamepad(float intensity, float duration)
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
        float amount;
        while (t > 0)
        {
            t -= Time.deltaTime / duration;
            amount = Mathf.Lerp(0, 1, t);
            pad.SetMotorSpeeds(amount, amount / 2);
            yield return null;
        }
        pad.SetMotorSpeeds(0, 0);
    }

    public void PlaySound(AudioClip sound, float pitch = 1f, float volume = 1f, bool loop = false)
    {
        if (sound != null)
        {
            AudioSource curAudioSource;
            int loopChecker = 0;

            do
            {
                curAudioSource = audioSources[nextChannel];
                nextChannel = (nextChannel + 1) % audioChannels;
                loopChecker++;
            } while (curAudioSource.clip != sound && curAudioSource.isPlaying && loopChecker < audioChannels);

            curAudioSource.clip = sound;
            curAudioSource.pitch = Time.timeScale * pitch;
            curAudioSource.volume = volume;
            curAudioSource.loop = loop;
            curAudioSource.Play();
        }
    }

    public void PlaySound(AudioClip[] sound, float pitch = 1f, float volume = 1f)
    {
        PlaySound(sound[Random.Range(0, sound.Length)], pitch, volume);
    }

    public AudioSource FadeInSound(AudioClip sound, float fadeTime, float pitch = 1f, float volume = 1f, bool loop = false)
    {
        if (sound != null)
        {
            AudioSource curAudioSource;
            int loopChecker = 0;

            do
            {
                curAudioSource = audioSources[nextChannel];
                nextChannel = (nextChannel + 1) % audioChannels;
                loopChecker++;
            } while (curAudioSource.clip != sound && curAudioSource.isPlaying && loopChecker < audioChannels);

            curAudioSource.clip = sound;
            curAudioSource.pitch = Time.timeScale * pitch;
            curAudioSource.loop = loop;
            curAudioSource.volume = 0;
            curAudioSource.Play();
            curAudioSource.DOFade(volume, fadeTime).SetUpdate(UpdateType.Normal, true);
            return curAudioSource;
        }
        return null;
    }

    public void StopSound(AudioClip sound)
    {
        foreach (AudioSource channel in audioSources)
        {
            if (channel.clip == sound)
            {
                channel.Stop();
            }
        }
    }

    public void StopSound(AudioClip sound, float fadeTime)
    {
        foreach (AudioSource channel in audioSources)
        {
            if (channel.clip == sound)
            {
                channel.DOFade(0, fadeTime).SetUpdate(UpdateType.Normal, true).OnComplete(() =>
                {
                    channel.Stop();
                });
            }
        }
    }

    public void StopChannel(AudioSource channel, float fadeTime)
    {
        channel.DOFade(0, fadeTime).SetUpdate(UpdateType.Normal, true).OnComplete(() =>
        {
            channel.Stop();
        });
    }

    protected void OnDestroy()
    {
        if (mgr == null) return;
        foreach (Gamepad pad in Gamepad.all)
        {
            if (mgr.deviceId == pad.deviceId)
            {
                pad.SetMotorSpeeds(0, 0);
            }
        }
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
        UnlitColor,
    }
}
