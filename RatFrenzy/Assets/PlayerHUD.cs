﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerHUD : MonoBehaviour
{
    public Player.Identity identity;
    public TMPro.TMP_Text scoreLabel;
    public Image colorImage;
    public RectTransform deadIndicator, progressBar, progressBarContainer;
    public int maxScore;
    private RatManager mgr;
    private int score;
    private DisplayType currentDisplayType;
    public enum DisplayType
    {
        Survivor,
        Score,
        ProgressBar,
        ProgressLap,
        ProgressPercent
    }
    void OnEnable()
    {
        if (GameManager.instance == null) return;
        mgr = GameManager.instance.GetRatManager(identity);
        if (mgr == null) this.gameObject.SetActive(false);
    }
    public void SetScore(int score)
    {
        switch (currentDisplayType)
        {
            case DisplayType.Score:
                scoreLabel.text = "" + score;
                break;
            case DisplayType.ProgressLap:
                scoreLabel.text = score + "/" + maxScore;
                break;
            case DisplayType.ProgressPercent:
                scoreLabel.text = score + "%";
                break;
            case DisplayType.ProgressBar:
                float width = progressBar.parent.GetComponent<RectTransform>().rect.width;
                progressBar.offsetMax = new Vector2(width - (score * width / maxScore), 0);
                break;
        }
    }
    public void SetAliveStatus(bool isAlive)
    {
        deadIndicator.gameObject.SetActive(!isAlive);
    }
    public void SetDisplayType(DisplayType type)
    {
        scoreLabel.gameObject.SetActive(false);
        progressBarContainer.gameObject.SetActive(false);
        progressBar.offsetMax = new Vector2(progressBar.parent.GetComponent<RectTransform>().rect.width, 0);
        deadIndicator.gameObject.SetActive(false);
        currentDisplayType = type;
        switch (type)
        {
            case DisplayType.Survivor:
                break;
            case DisplayType.Score:
                scoreLabel.gameObject.SetActive(true);
                break;
            case DisplayType.ProgressBar:
                progressBarContainer.gameObject.SetActive(true);
                break;
            case DisplayType.ProgressLap:
                scoreLabel.gameObject.SetActive(true);
                break;
            case DisplayType.ProgressPercent:
                scoreLabel.gameObject.SetActive(true);
                break;
        }
    }
}
