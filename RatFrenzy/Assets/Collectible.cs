using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GenericController ctrl = other.GetComponent<GenericController>();
        if (ctrl == null) return;
        ctrl.AddPoint();
        Destroy(this.gameObject);
    }
}
