﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SetBackPlayer : MonoBehaviour
{
    public float setBackAmount, cooldown;

    private float timer;

    private void Update()
    {
        timer -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "SetBackObstacle" && timer <= 0)
        {
            timer = cooldown;
            transform.RotateAround(other.transform.parent.position, Vector3.up, setBackAmount);
            //player.gameObject.transform.DOMoveZ(other.gameObject.transform.position.z - setBackAmount, 0.5f);
        }
    }
}