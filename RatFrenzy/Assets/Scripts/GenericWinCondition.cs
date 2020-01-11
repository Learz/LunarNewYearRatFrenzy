using Doozy.Engine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static GameManager;

public class GenericWinCondition : MonoBehaviour
{
    public PlayerHUD.DisplayType displayType;
    public int maxScore;
    public float timeLimit;
    public bool startTimeLimitOnStart;
    public float initialCountdown;
    protected int[] scores;
    protected bool gameEnded;
    protected virtual void Start()
    {
        if (GameManager.instance == null) GameManager.CreateTestManager();
        GameManager.instance.playerJoined.AddListener(OnPlayerJoined);

        if (initialCountdown > 0) StartCoroutine(InitialCountDown(initialCountdown));
        if (startTimeLimitOnStart) StartTimeoutCountdown(timeLimit);

        GameManager.instance.ConfigureGameHud(displayType, maxScore);
        scores = new int[4];
    }

    public virtual void EliminatePlayer(Player.Identity id)
    {
        GameManager.instance.UpdateAliveStatus(id, false);
    }
    public virtual void AddPoint(Player.Identity id)
    {
        scores[(int)id]++;
        Debug.Log("Score : " + scores[(int)id]);
        GameManager.instance.UpdateMinigameScore(id, scores[(int)id]);
    }
    public virtual void AddPoints(Player.Identity id, int points)
    {
        scores[(int)id] += points;
        GameManager.instance.UpdateMinigameScore(id, scores[(int)id]);
    }
    public virtual void StartTimeoutCountdown() => StartTimeoutCountdown(timeLimit);
    public virtual void StartTimeoutCountdown(float limit) => StartCoroutine(MiniGameCountDown(limit, true));

    protected virtual void OnPlayerJoined(PlayerInput input)
    {
        //GameManager.instance.ConfigureGameHud(displayType);
    }
    protected virtual void EndGame()
    {
        if (gameEnded) return;
        gameEnded = true;
        Debug.Log("No winner! You may want to override this method for your win condition");
        GameManager.instance.DisplayScore();
    }
    protected virtual void EndGame(Player.Identity winner)
    {
        if (gameEnded) return;
        gameEnded = true;
        Debug.Log(winner + " wins!");
        GameManager.instance.AddPoints(winner);
        GameManager.instance.DisplayScore();
    }
    protected IEnumerator InitialCountDown(float timeLeft)
    {
        UIPopup popup = UIPopup.GetPopup("Countdown");
        if (popup == null) throw new System.Exception("Countdown Popup not found");

        Time.timeScale = 0;

        // Compensating for initial popup animation time.
        timeLeft += 0.3f;

        int lastNumber = (int)timeLeft;
        string labelString = "" + timeLeft;
        popup.Data.SetLabelsTexts(labelString);
        popup.Show();

        while (timeLeft > 0)
        {
            timeLeft -= Time.unscaledDeltaTime;
            if (lastNumber != Mathf.CeilToInt(timeLeft))
            {

                labelString = (lastNumber - 1 == 0) ? "Start!" : "" + (lastNumber - 1);
                popup.Data.SetLabelsTexts(labelString);
                popup.Show();
                lastNumber = Mathf.CeilToInt(timeLeft);
            }
            yield return null;
        }
        Time.timeScale = 1;
    }
    protected IEnumerator MiniGameCountDown(float timeLeft, bool endGame)
    {
        GameManager.instance.UpdateTimeLeft(timeLeft);
        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(1);
            timeLeft--;
            GameManager.instance.UpdateTimeLeft(timeLeft);
        }
        if (endGame) this.EndGame();
    }
}
