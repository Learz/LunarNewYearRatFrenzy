using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public PlayerInputManager inputManager;
    public PlayerEvent playerJoined = new PlayerEvent();
    public PlayerEvent playerLeft = new PlayerEvent();
    public int numPlayers;
    public PlayerInput[] players;
    public enum PlayerIdentity
    {
        Player1 = 0,
        Player2 = 1,
        Player3 = 2,
        Player4 = 3
    };
    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null) throw new System.Exception("Game Manager already exists!");

        instance = this;
        DontDestroyOnLoad(this);
        players = new PlayerInput[4];
    }
    void OnPlayerJoined(PlayerInput input)
    {
        Debug.Log("Player " + (input.playerIndex + 1) + " has joined!");
        players[input.playerIndex] = (input);
        playerJoined.Invoke(input);
    }
    void OnPlayerLeft(PlayerInput input)
    {
        Debug.Log("Player " + (input.playerIndex + 1) + " has left!");
        players[input.playerIndex] = null;
        playerLeft.Invoke(input);
    }
    public RatManager GetRatManager(PlayerIdentity id)
    {
        if (players[(int)id] != null) return players[(int)id].GetComponent<RatManager>();
        else return null;
    }
    public static void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        // TODO : LoadScreen with Input mapping chart
    }
    public static void LoadSceneID(int index)
    {
        SceneManager.LoadScene(index);
        // TODO : LoadScreen with Input mapping chart
    }

    // Instantiate a test manager for scene prototyping
    public static void CreateTestManager()
    {
        if (instance != null) return;
        Debug.Log("Creating game manager");
        Instantiate(Resources.Load("GameManager"));
    }
}
public class PlayerEvent : UnityEvent<PlayerInput> { };