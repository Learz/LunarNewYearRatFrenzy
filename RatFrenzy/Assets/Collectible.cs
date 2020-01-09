using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public bool multiplySpeedOnCollected;
    public float speedMultiplier = 1.5f;

    private void OnTriggerEnter(Collider other)
    {
        GenericController ctrl = other.GetComponent<GenericController>();
        if (ctrl == null) return;
        ctrl.AddPoint();
        if (multiplySpeedOnCollected)
        {
            RatController rat = (RatController)ctrl;
            rat.MultiplySpeed(speedMultiplier);
        }
        Destroy(this.gameObject);
    }
}
