﻿using Doozy.Engine.Nody;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
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
    public MiniGameList miniGames;
    public PlayerHUD[] playerHUDs;
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
        DontDestroyOnLoad(EventSystem.current);
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
    public RatManager GetRatManager(Player.Identity id)
    {
        if (players[(int)id] != null) return players[(int)id].GetComponent<RatManager>();
        else return null;
    }
    public PlayerInput GetPlayerInput(Player.Identity id)
    {
        if (players[(int)id] != null) return players[(int)id];
        else return null;
    }
    public List<Player.Identity> GetPlayerIdentities()
    {
        List<Player.Identity> list = new List<Player.Identity>();
        foreach (PlayerInput player in players)
        {
            if (player != null) list.Add((Player.Identity)player.playerIndex);
        }
        return list;
    }

    public void AddPoints(Player.Identity id) => scores[(int)id]++;
    public void AddPoints(Player.Identity id, int ammount) => scores[(int)id] += ammount;

    public int GetScore(Player.Identity id) => scores[(int)id];
    public void DisplayScore()
    {
        graph.GoToNodeByName("DisplayScore");
    }
    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        // TODO : LoadScreen with Input mapping chart
    }
    public void LoadSceneID(int index)
    {
        SceneManager.LoadScene(index);
        // TODO : LoadScreen with Input mapping chart
    }
    public void LoadSceneName(string name)
    {
        SceneManager.LoadScene(name);
    }
    public List<string> GetScenes()
    {
        List<string> scenes = new List<string>();
        for (int i = 2; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            scenes.Add(Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i)));
        }
        return scenes;
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
        SceneManager.LoadSceneAsync(1);
    }
    public void UpdateAliveStatus(Player.Identity id, bool isAlive)
    {
        playerHUDs[(int)id].SetAliveStatus(isAlive);
    }
    public void UpdateMinigameScore(Player.Identity id, int score)
    {
        playerHUDs[(int)id].SetScore(score);
    }
    public void SetDisplayType(PlayerHUD.DisplayType type)
    {
        foreach (PlayerHUD hud in playerHUDs) hud.SetDisplayType(type);
        graph.GoToNodeByName("InGame");
    }
    public void SetMaxScore(int score)
    {
        foreach (PlayerHUD hud in playerHUDs) hud.maxScore = score;
    }
    public static IEnumerator MiniGameCountDown(float timeLeft)
    {
        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(1);
            timeLeft--;
        }
    }
}
public class PlayerEvent : UnityEvent<PlayerInput> { };

// Pour plus d'abstraction
public class Player
{
    public enum Identity
    {
        Player1 = 0,
        Player2 = 1,
        Player3 = 2,
        Player4 = 3
    }
    public enum Color
    {
        Red = 8,
        Blue = 17,
        Yellow = 11,
        Green = 12,
        Pink = 29,
        Purple = 28,
        Orange = 9,
        Teal = 16
    }
    public enum CharacterPose
    {
        One,
        Two,
        Three,
        Four
    }
}

public class ColorPalette
{
    private static readonly Color[] pal = new Color[32]
    {
        new Color(190, 74, 47) ,
        new Color(215,118,67) ,
        new Color(234,212,170) ,
        new Color(228,166,114) ,
        new Color(184,111,80) ,
        new Color(115,62,57) ,
        new Color(62,39,49) ,
        new Color(162,38,51) ,
        new Color(228, 59, 68) ,
        new Color(247, 118, 34) ,
        new Color(254,174,52) ,
        new Color(254, 231, 97) ,
        new Color(99, 199, 77) ,
        new Color(62,137,72) ,
        new Color(38,92,66) ,
        new Color(25,60,62) ,
        new Color(18, 78, 137) ,
        new Color(0, 153, 219) ,
        new Color(44,232,245) ,
        new Color(255,255,255) ,
        new Color(192,203,220) ,
        new Color(139,155,180) ,
        new Color(90,105,136) ,
        new Color(58,68,102) ,
        new Color(38,43,68) ,
        new Color(24,20,37) ,
        new Color(255,0,68) ,
        new Color(104,56,108) ,
        new Color(181,80,136) ,
        new Color(246,117,122) ,
        new Color(232,183,150) ,
        new Color(194,133,105)
    };

    public static Color GetColor(int col)
    {
        return new Color(pal[col].r / 255, pal[col].g / 255, pal[col].b / 255);
    }

    public static Color GetColor(Player.Color col)
    {
        return new Color(pal[(int)col].r / 255, pal[(int)col].g / 255, pal[(int)col].b / 255);
    }
}