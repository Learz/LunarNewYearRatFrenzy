using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    public Player.Identity identity;
    public TMPro.TMP_Text scoreLabel;
    public Image background;
    private RatManager mgr;
    private int score;

    void OnEnable()
    {
        if (GameManager.instance == null) return;
        mgr = GameManager.instance.GetRatManager(identity);
        if (mgr == null) this.gameObject.SetActive(false);
        else
        {
            score = GameManager.instance.GetScore(identity);
            scoreLabel.text = "" + score;
            background.color = mgr.GetPlayerColor();
        }
    }

}
