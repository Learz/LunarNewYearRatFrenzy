using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static GameManager;

public class SurvivorWinCondition : GenericWinCondition
{
    public enum SurvivorType
    {
        AllEliminated = 0,
        LastManStanding = 1,
    }

    public SurvivorType survivorType;

    private List<Player.Identity> playersAlive;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        playersAlive = GameManager.instance.GetPlayerIdentities();
    }
    public override void EliminatePlayer(Player.Identity id)
    {
        base.EliminatePlayer(id);
        Debug.Log(id + " eliminated");
        if (playersAlive.Count-1 == (int)survivorType) EndGame(playersAlive[0]);
        playersAlive.Remove(id);
    }
    protected override void OnPlayerJoined(PlayerInput input)
    {
        if (playersAlive == null) playersAlive = new List<Player.Identity>();
        playersAlive.Add((Player.Identity)input.playerIndex);
    }
    void EndGame(Player.Identity winner)
    {
        Debug.Log(winner + " wins!");
        GameManager.instance.AddPoints(playersAlive[0]);
        GameManager.instance.DisplayScore();
    }
}
