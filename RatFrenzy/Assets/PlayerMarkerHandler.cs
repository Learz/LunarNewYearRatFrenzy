using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMarkerHandler : MonoBehaviour
{
    public static PlayerMarkerHandler instance;
    public RectTransform canvasRect;
    private List<RectTransform> playerMarkers;
    private bool markersEnabled = true;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
        playerMarkers = new List<RectTransform>();
        foreach (RectTransform rect in this.transform) playerMarkers.Add(rect);

    }
    public void UpdatePlayerPosition(Player.Identity id, Vector2 position)
    {
        if (!markersEnabled) return;
        Vector2 screenPosition = new Vector2((position.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f),
            (position.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f));
        Debug.Log(screenPosition);
        playerMarkers[(int)id].anchoredPosition = screenPosition;
    }
    public void EnableMarkers()
    {
        if (markersEnabled) return;
        markersEnabled = true;
        foreach (RectTransform marker in playerMarkers)
        {
            marker.gameObject.SetActive(true);
        }
    }
    public void DisableMarkers()
    {
        if (!markersEnabled) return;
        markersEnabled = false;
        foreach (RectTransform marker in playerMarkers)
        {
            marker.gameObject.SetActive(false);
        }
    }
}
