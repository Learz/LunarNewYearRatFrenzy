using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static GameManager;

public class GenericWinCondition : MonoBehaviour
{
    public PlayerHUD.DisplayType displayType;
    public int maxScore;
    public float timeLimit;
    public bool startCountdownOnStart;
    protected int[] scores;
    protected virtual void Start()
    {
        if (GameManager.instance == null) GameManager.CreateTestManager();
        GameManager.instance.playerJoined.AddListener(OnPlayerJoined);

        if (startCountdownOnStart) StartTimeoutCountdown(timeLimit);
        GameManager.instance.ConfigureGameHud(displayType, maxScore);
        scores = new int[4];
    }

    public virtual void EliminatePlayer(Player.Identity id)
    {
        GameManager.instance.UpdateAliveStatus(id, false);
    }
    public virtual void AddPoint(Player.Identity id)
    {
        scores[(int)id]++;
        Debug.Log("Score : " + scores[(int)id]);
        GameManager.instance.UpdateMinigameScore(id, scores[(int)id]);
    }
    public virtual void AddPoints(Player.Identity id, int points)
    {
        scores[(int)id] += points;
        GameManager.instance.UpdateMinigameScore(id, scores[(int)id]);
    }
    public virtual void StartTimeoutCountdown() => StartTimeoutCountdown(timeLimit);
    public virtual void StartTimeoutCountdown(float limit) => StartCoroutine(GameManager.MiniGameCountDown(limit));

    protected virtual void OnPlayerJoined(PlayerInput input)
    {
        //GameManager.instance.ConfigureGameHud(displayType);
    }
    protected virtual void EndGame(Player.Identity winner)
    {
        Debug.Log(winner + " wins!");
        GameManager.instance.AddPoints(winner);
        GameManager.instance.DisplayScore();
    }
}
