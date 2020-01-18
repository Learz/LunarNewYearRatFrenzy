using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShowWinner : MonoBehaviour
{
    public TMPro.TMP_Text winnerLabel;
    private RatManager mgr;
    private int score;

    void OnEnable()
    {
        if (GameManager.instance == null) return;
        else
        {
            winnerLabel.text = GameManager.instance.roundWinMessage;
        }
    }
}
