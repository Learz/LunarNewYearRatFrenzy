using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using DG.Tweening;

public class PlayerHUD : MonoBehaviour
{
    public Player.Identity identity;
    public TMPro.TMP_Text scoreLabel;
    public Image poseImage;
    public RectTransform deadIndicator, progressBar, progressBarContainer, pointMarker;
    [SerializeField]
    private float targetPointMarkerHeight;
    [SerializeField]
    private float moveDuration = 1.0f;
    [SerializeField]
    private Ease moveEase = Ease.InOutSine;

    [HideInInspector]
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
        else
        {
            this.gameObject.SetActive(true);
            poseImage.color = mgr.GetPlayerColor();
            poseImage.sprite = mgr.GetPoseSprite();
        }
    }
    public void EnableHud()
    {
        if (GameManager.instance == null) return;
        mgr = GameManager.instance.GetRatManager(identity);
        if (mgr == null) this.gameObject.SetActive(false);

        else
        {
            this.gameObject.SetActive(true);
            poseImage.color = mgr.GetPlayerColor();
            poseImage.sprite = mgr.GetPoseSprite();
        }
    }
    public void ShowWinAnimation()
    {
        float startingHeight = pointMarker.localPosition.y;
        pointMarker.localRotation = Quaternion.Euler(0, 0, 15);
        DOTween.Sequence()
            .Append(pointMarker.DOLocalMoveY(targetPointMarkerHeight + startingHeight, moveDuration).SetEase(moveEase).SetUpdate(UpdateType.Normal,true).SetLoops(2, LoopType.Yoyo))
            .Join(pointMarker.DORotate(new Vector3(0, 0, -15), moveDuration * 2).SetEase(Ease.Linear).SetUpdate(UpdateType.Normal, true))
            .Join(pointMarker.GetComponent<CanvasGroup>().DOFade(1, moveDuration).SetEase(moveEase).SetUpdate(UpdateType.Normal, true).SetLoops(2, LoopType.Yoyo));
        pointMarker.GetComponent<CanvasGroup>().alpha = 0;
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
                scoreLabel.text = (score / maxScore * 100).ToString("0.0") + "%";
                break;
            case DisplayType.ProgressBar:
                float width = progressBar.parent.GetComponent<RectTransform>().rect.width;
                progressBar.offsetMax = new Vector2(-width + (score * width / (float)maxScore), 0);
                break;
        }
    }
    public void SetAliveStatus(bool isAlive)
    {
        deadIndicator.gameObject.SetActive(!isAlive);
    }
    public void ConfigureGameHud(DisplayType type, int maxSc)
    {
        maxScore = maxSc;
        scoreLabel.gameObject.SetActive(false);
        progressBarContainer.gameObject.SetActive(false);
        progressBar.offsetMax = new Vector2(-progressBar.parent.GetComponent<RectTransform>().rect.width, 0);
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
        SetScore(0);
    }
}
