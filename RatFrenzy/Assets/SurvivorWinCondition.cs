using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static GameManager;

public class SurvivorWinCondition : MonoBehaviour
{
    private List<PlayerIdentity> playersAlive;
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.instance == null) GameManager.CreateTestManager();
        GameManager.instance.playerJoined.AddListener(OnPlayerJoined);
        playersAlive = GameManager.instance.GetPlayerIdentities();
    }
    public void EliminatePlayer(PlayerIdentity id)
    {
        playersAlive.Remove(id);
        Debug.Log(id + " eliminated");
        if (playersAlive.Count <= 1) EndGame();
    }
    void OnPlayerJoined(PlayerInput input)
    {
        if (playersAlive == null) playersAlive = new List<PlayerIdentity>();
        playersAlive.Add((PlayerIdentity)input.playerIndex);
    }
    void EndGame()
    {
        Debug.Log(playersAlive[0] + " wins!");
        GameManager.instance.AddPoints(playersAlive[0]);
        GameManager.instance.DisplayScore();
    }
}
