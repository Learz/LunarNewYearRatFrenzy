using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static GameManager;

public class SpeedUp : MonoBehaviour
{
    public SpawnObject spawner;
    private List<Player.Identity> players;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.instance == null) GameManager.CreateTestManager();
        GameManager.instance.playerJoined.AddListener(OnPlayerJoined);
        players = GameManager.instance.GetPlayerIdentities();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnPlayerJoined(PlayerInput input)
    {
        if (players == null) players = new List<Player.Identity>();
        players.Add((Player.Identity)input.playerIndex);
    }
}
