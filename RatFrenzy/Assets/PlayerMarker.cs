using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMarker : MonoBehaviour
{
    public Player.Identity identity;
    public TMPro.TMP_Text label;
    public Image arrow;
    [Range(0f, 1f)]
    public float opacity;
    private RatManager mgr;
    private Color col;

    void OnEnable()
    {
        if (GameManager.instance == null) return;
        mgr = GameManager.instance.GetRatManager(identity);
        if (mgr == null) this.gameObject.SetActive(false);
        else
        {
            this.gameObject.SetActive(true);
            col = mgr.GetPlayerColor();
            col.a = opacity;
            arrow.color = col;
            label.color = col;
        }
    }
}
