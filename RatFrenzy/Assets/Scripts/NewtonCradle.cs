using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewtonCradle : MonoBehaviour
{
    public Transform pendulum0, pendulum4;
    public float speed = 1f;
    public float peakAngle = 45f;
    private float k = 3;
    private Quaternion targetRotation0, targetRotation4;

    // Update is called once per frame
    void Update()
    {
        targetRotation0 = Quaternion.identity * Quaternion.Euler(0, -peakAngle, 0);
        targetRotation4 = Quaternion.identity * Quaternion.Euler(0, peakAngle, 0);

        pendulum0.localRotation = Quaternion.Slerp(Quaternion.identity, targetRotation0, Mathf.Sin(k * (Time.time * speed)));
        pendulum4.localRotation = Quaternion.Slerp(Quaternion.identity, targetRotation4, Mathf.Sin(k * ((Time.time * speed) - 1)));
    }
}
