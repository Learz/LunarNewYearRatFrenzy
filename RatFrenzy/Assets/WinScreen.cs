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
    }
    private void FadeToBlack()
    {
        fadeAnimator.Play("fadetoblack");
    }
}
