﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SportsballGoal : MonoBehaviour
{
    public Player.Identity identity;
    public GenericWinCondition winCondition;
    public GenericController controller;
    public ParticleSystem particles;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        SportsBall ball = other.GetComponent<SportsBall>();
        if (ball != null)
        {
            ball.Goal();
            particles.Play();
            if (audioSource != null) audioSource.Play();
            if (controller.mgr != null)
            {
                winCondition.EliminatePlayer(identity);
                controller.Kill();
            }
        }
    }
}
