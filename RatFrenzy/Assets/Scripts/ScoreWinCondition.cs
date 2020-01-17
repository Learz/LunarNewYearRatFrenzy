using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScoreWinCondition : GenericWinCondition
{
    private List<Player.Identity> playersAlive;
    protected override void Start()
    {
        base.Start();
        playersAlive = GameManager.instance.GetPlayerIdentities();
    }

    public override void AddPoint(Player.Identity id)
    {
        base.AddPoint(id);
        if (scores[(int)id] == maxScore) EndGame(id);
        Debug.Log("Point added for " + id);
    }

    public override void EliminatePlayer(Player.Identity id)
    {
        base.EliminatePlayer(id);
        playersAlive.Remove(id);
        if (playersAlive.Count == 0)
        {
            int winner = 0;
            for (int i = 0; i < scores.Length; i++)
            {
                if (scores[i] > scores[winner]) winner = i;
            }
            EndGame((Player.Identity)winner);
        }
    }

    protected override void EndGame()
    {
        List<Player.Identity> winners = new List<Player.Identity>();
        int currentHighScore = 0;
        for (int i = 0; i < scores.Length; i++)
        {
            if (scores[i] > currentHighScore)
            {
                winners.Clear();
                winners.Add((Player.Identity)i);
                currentHighScore = scores[i];
            }
            if (scores[i] == currentHighScore)
            {
                winners.Add((Player.Identity)i);
            }
        }
        EndGame(winners);
    }

    protected override void OnPlayerJoined(PlayerInput input)
    {
        if (playersAlive == null) playersAlive = new List<Player.Identity>();
        playersAlive.Add((Player.Identity)input.playerIndex);
    }
}
