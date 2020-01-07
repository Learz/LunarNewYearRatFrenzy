using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class GenericWinCondition : MonoBehaviour
{
    public int pointGoal;
    public float timeLimit;
    public bool startCountdownOnStart;

    void Start()
    {
        if (startCountdownOnStart) StartTimeoutCountdown(timeLimit);
    }

    public virtual void EliminatePlayer(Player.Identity id)
    {

    }
    public virtual void AddMinigamePoint(Player.Identity id, int points)
    {

    }
    public virtual void StartTimeoutCountdown() => StartTimeoutCountdown(timeLimit);
    public virtual void StartTimeoutCountdown(float limit)
    {

    }
}
