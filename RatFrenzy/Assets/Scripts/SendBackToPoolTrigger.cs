using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendBackToPoolTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10) return;
        other.transform.position = new Vector3(-500, -500, -500);
        ResettingMonoBehaviour resetteable = other.GetComponent<ResettingMonoBehaviour>();
        if(resetteable != null)
        {
            resetteable.readyToRespawn = true;
            resetteable.ResetOnSpawn();
        }
    }
}
