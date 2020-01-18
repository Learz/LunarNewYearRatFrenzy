using DG.Tweening;
using Doozy.Engine.Nody;
using System;
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
    public Sprite[] playerPoses;
    public UnityEngine.UI.Text timeLeftLabel;
    public AudioSource audioSource;
    public string roundWinMessage;
    private int[] scores;
    private int roundCounter;
    private int showScoreEveryXRounds = 5, winningScore = 10;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null) throw new System.Exception("Game Manager already exists!");
        if (graph == null) graph = GetComponent<GraphController>();
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
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
        numPlayers++;
        playerJoined.Invoke(input);
    }
    void OnPlayerLeft(PlayerInput input)
    {
        Debug.Log("Player " + (input.playerIndex + 1) + " has left!");
        players[input.playerIndex] = null;
        numPlayers--;
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
    public Sprite GetPoseSprite(Player.Identity id)
    {
        return playerPoses[GetRatManager(id).poseIndex];
    }

    public void AddPoint(Player.Identity id)
    {
        scores[(int)id]++;
        playerHUDs[(int)id].ShowWinAnimation();
    }
    public void EndGame()
    {
        roundCounter++;
        if (roundCounter % showScoreEveryXRounds == 0)
        {
            int highScore = 0;
            for (int i = 0; i < 4; i++)
            {
                if (scores[i] > highScore) highScore = scores[i];
            }
            if (highScore >= winningScore) graph.GoToNodeByName("WinScreen");
            else graph.GoToNodeByName("DisplayScore");
        }
        else graph.GoToNodeByName("ShowWinner");
    }
    public Player.Identity GetLeader()
    {
        int leader = 0;
        int highScore = 0;
        for (int i = 0; i < 4; i++)
        {
            if (scores[i] > highScore) { leader = i; highScore = scores[i]; }
        }
        return ((Player.Identity)leader);
    }
    public List<Player.Identity> GetLeaders()
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
        return winners;
    }

    //public void AddPoints(Player.Identity id, int ammount) => scores[(int)id] += ammount;

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
        Instantiate(Resources.Load("Root"));
        instance.graph.GoToNodeByName("InGame");
    }
    /*public void GameListenerEvent(string gameEvent)
    {
        if (gameEvent == "OnReturnToMenu") OnReturnToMenu();
        else if (gameEvent == "SetMoonGravity") SetMoonGravity();
    }*/
    private void OnReturnToMenu()
    {
        Debug.Log("Unloading active scene");
        SceneManager.LoadSceneAsync(1);
        scores = new int[4];
        roundCounter = 0;
        Physics.gravity = new Vector3(0, -9.81f, 0);
        RemoveAllPlayers();

    }
    private void SetMoonGravity()
    {
        Debug.Log("Setting moon gravity");
        Physics.gravity = new Vector3(0, -1.62f, 0);
    }
    public void UpdateAliveStatus(Player.Identity id, bool isAlive)
    {
        playerHUDs[(int)id].SetAliveStatus(isAlive);
    }
    public void UpdateMinigameScore(Player.Identity id, int score)
    {
        playerHUDs[(int)id].SetScore(score);
    }
    public void UpdateTimeLeft(float timeLeft)
    {
        timeLeftLabel.text = (timeLeft <= 0) ? "" : "" + timeLeft;
    }
    public void ConfigureGameHud(PlayerHUD.DisplayType type, int maxScore)
    {
        foreach (PlayerHUD hud in playerHUDs) hud.ConfigureGameHud(type, maxScore);
        graph.GoToNodeByName("InGame");

    }
    public void ShowPlayerHud(Player.Identity id)
    {
        playerHUDs[(int)id].gameObject.SetActive(true);
    }
    public void SetMaxScore(int score)
    {
        foreach (PlayerHUD hud in playerHUDs) hud.maxScore = score;
    }
    public void RestrictUIInputToLoser()
    {

    }
    public void RemoveAllPlayers()
    {
        foreach (PlayerInput player in players)
            if (player != null) Destroy(player.gameObject);
    }
    public void PlayMusic(AudioClip clip)
    {
        if (audioSource.clip == clip) return;
        if (audioSource.isPlaying)
        {
            audioSource.DOFade(0, 0.2f).SetUpdate(UpdateType.Normal, true).OnComplete(() =>
             {
                 audioSource.clip = clip;
                 audioSource.Play();
                 audioSource.DOFade(0.4f, 0.2f).SetUpdate(UpdateType.Normal, true);

             });
        }
        else
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
    public void StopMusic()
    {
        audioSource.DOFade(0, 0.5f).SetUpdate(UpdateType.Normal, true).OnComplete(() =>
        {
            audioSource.clip = null;
            audioSource.volume = 0.4f;
        });

    }
    private void OnApplicationQuit()
    {
        foreach (Gamepad pad in Gamepad.all) pad.SetMotorSpeeds(0, 0);

    }
    public void ResetRoundCounter() => roundCounter = 0;
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
        Player4 = 3,
        Leader
    }
    public enum Color
    {
        Red = 8,
        Blue = 17,
        DarkBlue = 16,
        Yellow = 11,
        Green = 12,
        Pink = 29,
        Purple = 28,
        Orange = 9
        //White = 19
        //Black = 25
    }
    public enum CharacterPose
    {
        One,
        Two,
        Three,
        Four
    }
}
public static class Extensions
{

    public static T Next<T>(this T src) where T : struct
    {
        if (!typeof(T).IsEnum) throw new System.Exception(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

        T[] Arr = (T[])Enum.GetValues(src.GetType());
        int j = Array.IndexOf<T>(Arr, src) + 1;
        return (Arr.Length == j) ? Arr[0] : Arr[j];
    }
    public static T Prev<T>(this T src) where T : struct
    {
        if (!typeof(T).IsEnum) throw new System.Exception(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

        T[] Arr = (T[])Enum.GetValues(src.GetType());
        int j = Array.IndexOf<T>(Arr, src) - 1;
        return (j < 0) ? Arr[Arr.Length - 1] : Arr[j];
    }
}
public class ColorPalette
{
    private static readonly Color[] pal = new Color[32]
    {
        new Color(190, 74, 47) ,    //0
        new Color(215,118,67) ,     //1
        new Color(234,212,170) ,    //2
        new Color(228,166,114) ,    //3
        new Color(184,111,80) ,     //4
        new Color(115,62,57) ,      //5
        new Color(62,39,49) ,       //6
        new Color(162,38,51) ,      //7
        new Color(228, 59, 68) ,    //8
        new Color(247, 118, 34) ,   //9
        new Color(254,174,52) ,     //10
        new Color(255, 218, 36) ,   //11
        new Color(99, 199, 77) ,    //12
        new Color(62,137,72) ,      //13
        new Color(38,92,66) ,       //14
        new Color(25,60,62) ,       //15 
        new Color(0, 21, 255) ,    //16
        new Color(0, 153, 219) ,    //17
        new Color(44,232,245) ,     //18
        new Color(255,255,255) ,    //19
        new Color(192,203,220) ,    //20
        new Color(139,155,180) ,    //21
        new Color(90,105,136) ,     //22
        new Color(58,68,102) ,      //23
        new Color(38,43,68) ,       //24
        new Color(0,0,0) ,          //25
        new Color(255,0,68) ,       //26
        new Color(104,56,108) ,     //27
        new Color(181,80,136) ,     //28
        new Color(255,69,246) ,     //29
        new Color(232,183,150) ,    //30
        new Color(194,133,105)      //31
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