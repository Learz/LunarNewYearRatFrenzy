using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(RatController))]
public class SetBackPlayer : MonoBehaviour
{
    public float setBackAmount, cooldown;
    public Transform anchor;
    public AudioClip hitSound;
    public AudioSource audioSource;

    private float timer;
    private RatController player;

    private void Start()
    {
        player = GetComponent<RatController>();
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (!player.canGetPoint && timer <= 0) player.canGetPoint = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!player.isDead && timer <= 0 && ((other.tag == "JumpObstacle") || (other.tag == "DuckObstacle" && (!GetComponent<RatController>().isSliding || GetComponent<RatController>().isJumping))))
        {
            player.canGetPoint = false;
            timer = cooldown;
            transform.RotateAround(anchor.position, Vector3.up, setBackAmount);
            player.PlaySound(player.hurtSounds, Random.Range(0.9f,1.1f));
            player.PlaySound(hitSound, Random.Range(0.9f, 1.1f), 0.2f);
            /*if (audioSource != null && hitSound != null)
            {
                audioSource.clip = hitSound;
                audioSource.pitch = Time.timeScale * Random.Range(0.9f, 1.1f);
                audioSource.volume = 0.2f;
                audioSource.Play();
            }*/
            player.VibrateGamepad(1 , 0.25f);
            //player.gameObject.transform.DOMoveZ(other.gameObject.transform.position.z - setBackAmount, 0.5f);
        }
    }
}
