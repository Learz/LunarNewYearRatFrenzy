using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushTrigger : MonoBehaviour
{
    public WinScreen winScreen;
    public Vector3 force;

    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null) rb.AddForce(transform.TransformPoint(force) * Time.deltaTime);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<BallController>() != null) winScreen.SendNextWinner();
    }
}
