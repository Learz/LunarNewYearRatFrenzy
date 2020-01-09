﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Doozy.Engine.UI;

public class GameSelectionHandler : MonoBehaviour
{
    public GameObject gameSelectorPrefab;
    public RectTransform content;
    public Image thumbnail;
    public TMPro.TMP_Text descriptionField;
    public Doozy.Engine.UI.UIView gameSelectView;
    private List<MiniGame> games;
    private List<GameObject> selectors;
    private GameObject lastSelected;
    // Start is called before the first frame update
    void Start()
    {
        selectors = new List<GameObject>();
        foreach (MiniGame game in GameManager.instance.miniGames.gamesList)
        {
            GameObject selector = Instantiate(gameSelectorPrefab, content);
            selectors.Add(selector);
            selector.GetComponentInChildren<TMPro.TMP_Text>().text = game.gameName;
            Button button = selector.GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                lastSelected = selector;
                GameManager.instance.LoadSceneName(game.sceneName);
                gameSelectView.Hide();
            });
            selector.GetComponent<UIButton>().OnSelected.OnTrigger.Event.AddListener(() =>
            {
                DisplayMiniGameInfo(game);
            });
            selector.GetComponent<UIButton>().OnPointerEnter.OnTrigger.Event.AddListener(() =>
            {
                DisplayMiniGameInfo(game);
            });
        }
        selectors[0].GetComponent<Button>().Select();
    }
    public void SelectLastButton()
    {
        if (selectors == null) return;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(lastSelected);
    }
    public void DisplayMiniGameInfo(MiniGame game)
    {
        if (game.thumbnail != null) thumbnail.sprite = game.thumbnail;
        else thumbnail.sprite = null;
        if (game.description != null) descriptionField.text = game.description;
        else descriptionField.text = "Please add a description in your minigames settings file.";
    }
}