using Doozy.Engine.Nody;
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
    public GraphController graph;
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
    private int[] scores;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null) throw new System.Exception("Game Manager already exists!");
        if (graph == null) graph = GetComponent<GraphController>();
        instance = this;
        DontDestroyOnLoad(this);
        players = new PlayerInput[4];
        scores = new int[4];
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
    public PlayerInput GetPlayerInput(PlayerIdentity id)
    {
        if (players[(int)id] != null) return players[(int)id];
        else return null;
    }
    public List<PlayerIdentity> GetPlayerIdentities()
    {
        List<PlayerIdentity> list = new List<PlayerIdentity>();
        foreach (PlayerInput player in players)
        {
            if (player != null) list.Add((PlayerIdentity)player.playerIndex);
        }
        return list;
    }

    public void AddPoints(PlayerIdentity id) => scores[(int)id]++;
    public void AddPoints(PlayerIdentity id, int ammount) => scores[(int)id] += ammount;

    public int GetScore(PlayerIdentity id) => scores[(int)id];
    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        // TODO : LoadScreen with Input mapping chart
    }
    public void LoadSceneID(int index)
    {
        SceneManager.LoadScene(index);
        // TODO : LoadScreen with Input mapping chart
        //graph.Graph.ActiveNode.
        //graph.GoToNodeByName("InGame");
    }

    // Instantiate a test manager for scene prototyping
    public static void CreateTestManager()
    {
        if (instance != null) return;
        Debug.Log("Creating game manager");
        Instantiate(Resources.Load("GameManager"));
    }
    public void OnReturnToMenu()
    {
        Debug.Log("Unloading active scene");
        SceneManager.LoadSceneAsync(SceneManager.sceneCountInBuildSettings - 1);
    }
}
public class PlayerEvent : UnityEvent<PlayerInput> { };