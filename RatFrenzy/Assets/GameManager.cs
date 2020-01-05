using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public PlayerInputManager inputManager;
    public PlayerEvent playerJoined = new PlayerEvent();
    public PlayerEvent playerLeft = new PlayerEvent();
    public int numPlayers;
    public List<PlayerInput> players;
    // Start is called before the first frame update
    void Start()
    {
        if (instance != null) throw new System.Exception("Game Manager already exists!");

        instance = this;
        DontDestroyOnLoad(this);
    }
    void OnPlayerJoined(PlayerInput input)
    {
        Debug.Log("Player " + (input.playerIndex + 1) + " has joined!");
        playerJoined.Invoke(input);
        players.Add(input);
    }
    void OnPlayerLeft(PlayerInput input)
    {
        Debug.Log("Player " + (input.playerIndex + 1) + " has left!");
        playerLeft.Invoke(input);
        players.Remove(input);
    }

}
public class PlayerEvent : UnityEvent<PlayerInput> { };