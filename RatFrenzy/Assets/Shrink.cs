using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Shrink : MonoBehaviour
{
    public Vector3 targetScale;
    public float duration;

    private void Start()
    {
        this.transform.DOScale(targetScale, duration);
    }
}
