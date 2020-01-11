using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Doozy.Engine.UI;
using Doozy.Engine.Nody;

public class PlayerSelection : MonoBehaviour
{
    public static PlayerSelection instance { get; private set; }
    public PlayerUI[] players;
    public TMPro.TMP_Text countdown;
    private Coroutine co;
    private int readyPlayers = 0, numPlayers = 0;
    public UIButton startGame;
    public List<Player.Color> selectedColors;
    // Start is called before the first frame update
    void Start()
    {
        if (instance != null) throw new System.Exception("More than one Player Selection Manager");
        GameManager.instance.playerJoined.AddListener(OnPlayerJoined);
        GameManager.instance.playerLeft.AddListener(OnPlayerLeft);
        selectedColors = new List<Player.Color>();
        instance = this;
    }
    void OnPlayerJoined(PlayerInput input)
    {
        //players[input.playerIndex].PlayerJoined(input);
        numPlayers++;
    }
    void OnPlayerLeft(PlayerInput input)
    {
        //players[input.playerIndex].PlayerLeft();
        numPlayers--;
    }
    public void PlayerReady()
    {
        readyPlayers = 0;
        foreach (PlayerUI player in players)
        {
            if (player.playerIsReady) readyPlayers++;
        }
        if (readyPlayers >= 2 && readyPlayers == numPlayers)
        {
            co = StartCoroutine(Countdown());
        }
    }
    public void PlayerNotReady()
    {
        countdown.text = "";
        if (co != null) StopCoroutine(co);
    }
    IEnumerator Countdown()
    {
        int timeLeft = 3;
        while (timeLeft > 0)
        {
            countdown.text = "" + timeLeft;
            yield return new WaitForSeconds(1);
            timeLeft--;
        }
        countdown.text = "";
        Debug.Log("Game started");

        startGame.ExecuteClick();
        co = null;
    }
    private void OnDisable()
    {
        selectedColors.Clear();
        if (co == null) return;
        StopCoroutine(co);
        co = null;
        countdown.text = "";

    }
}
