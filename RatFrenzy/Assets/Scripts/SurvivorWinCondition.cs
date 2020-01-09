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
        if (playersAlive.Count - 1 == (int)survivorType)
        {
            if (survivorType == SurvivorType.LastManStanding) playersAlive.Remove(id);
            EndGame(playersAlive[0]);
        }
        else
        {
            playersAlive.Remove(id);
        }
        Debug.Log(playersAlive.Count + " players alive");

    }
    protected override void OnPlayerJoined(PlayerInput input)
    {
        if (playersAlive == null) playersAlive = new List<Player.Identity>();
        playersAlive.Add((Player.Identity)input.playerIndex);
    }

}
