using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SetBackPlayer : MonoBehaviour
{
    public float setBackAmount, cooldown;
    public Transform anchor;

    private float timer;

    private void Update()
    {
        timer -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (timer <= 0 && ((other.tag == "JumpObstacle") || (other.tag == "DuckObstacle" && (!GetComponent<RatController>().isSliding || GetComponent<RatController>().isJumping))))
        {
            timer = cooldown;
            transform.RotateAround(anchor.position, Vector3.up, setBackAmount);
            //player.gameObject.transform.DOMoveZ(other.gameObject.transform.position.z - setBackAmount, 0.5f);
        }
    }
}
