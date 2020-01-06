using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameSelectionHandler : MonoBehaviour
{
    public GameObject gameSelectorPrefab;
    public RectTransform content;
    public Doozy.Engine.UI.UIView gameSelectView;
    private List<string> scenes;
    private List<GameObject> selectors;
    private GameObject lastSelected;
    // Start is called before the first frame update
    void Start()
    {
        scenes = new List<string>();
        scenes = GameManager.instance.GetScenes();
        selectors = new List<GameObject>();
        foreach (string scene in scenes)
        {
            GameObject selector = Instantiate(gameSelectorPrefab, content);
            selectors.Add(selector);
            selector.GetComponentInChildren<TMPro.TMP_Text>().text = scene;
            Button button = selector.GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                lastSelected = selector;
                GameManager.instance.LoadSceneName(scene);
                gameSelectView.Hide();
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
}
