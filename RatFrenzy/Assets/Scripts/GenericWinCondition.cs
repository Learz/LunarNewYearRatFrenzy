using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GenericWinCondition : MonoBehaviour
{
    public int pointGoal;
    public float timeLimit;
    public bool startCountdownOnStart;

    void Start()
    {
        if (startCountdownOnStart) StartTimeoutCountdown(timeLimit);
    }

    public virtual void EliminatePlayer(GameManager.PlayerIdentity id)
    {

    }
    public virtual void AddMinigamePoint(GameManager.PlayerIdentity id, int points)
    {

    }
    public virtual void StartTimeoutCountdown() => StartTimeoutCountdown(timeLimit);
    public virtual void StartTimeoutCountdown(float limit)
    {

    }
}
