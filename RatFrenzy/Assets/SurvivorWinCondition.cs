using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static GameManager;

public class SurvivorWinCondition : MonoBehaviour
{
    public enum SurvivorType
    {
        AllEliminated = 0,
        LastManStanding = 1,
    }

    public SurvivorType survivorType;

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
        Debug.Log(id + " eliminated");
        if (playersAlive.Count-1 == (int)survivorType) EndGame(playersAlive[0]);
        playersAlive.Remove(id);
    }
    void OnPlayerJoined(PlayerInput input)
    {
        if (playersAlive == null) playersAlive = new List<PlayerIdentity>();
        playersAlive.Add((PlayerIdentity)input.playerIndex);
    }
    void EndGame(PlayerIdentity winner)
    {
        Debug.Log(winner + " wins!");
        GameManager.instance.AddPoints(playersAlive[0]);
        GameManager.instance.DisplayScore();
    }
}
