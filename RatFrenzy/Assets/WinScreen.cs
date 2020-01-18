using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScreen : MonoBehaviour
{
    public AudioClip song;
    public Animator fadeAnimator;
    private void Start()
    {
        GameManager.instance.PlayMusic(song);
        Physics.gravity = new Vector3(0, -1.62f, 0);
        Invoke("ShowMarker", 12f);
    }
    private void FadeToBlack()
    {
        fadeAnimator.Play("fadetoblack");
    }
    private void ShowMarker()
    {
        PlayerMarkerHandler.instance.ShowMarker(GameManager.instance.GetLeader());
    }
}
