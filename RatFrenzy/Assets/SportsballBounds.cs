using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SportsballBounds : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        SportsBall ball = other.GetComponent<SportsBall>();
        if (ball != null)
        {
            ball.Goal();
        }
    }
}
