using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScoreUI : MonoBehaviour
{
    public GameManager.PlayerIdentity identity;
    public TMPro.TMP_Text scoreLabel;
    private PlayerInput playerInput;
    private int score;

    void OnEnable()
    {
        playerInput = GameManager.instance.GetPlayerInput(identity);
        if (playerInput == null) this.gameObject.SetActive(false);
        else
        {
            score = GameManager.instance.GetScore(identity);
            scoreLabel.text = "" + score;
        }
    }
}
