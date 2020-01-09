using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointTrigger : MonoBehaviour
{
    public GenericWinCondition winCondition;

    private void Start()
    {
        if(winCondition == null) winCondition = FindObjectsOfType<GenericWinCondition>()[0];
    }

    private void OnTriggerEnter(Collider other)
    {
        GenericController player = other.GetComponent<GenericController>();
        if (player != null)
        {
            winCondition.AddPoint(player.identity);
            Physics.IgnoreCollision(other.GetComponentInParent<Collider>(), GetComponent<Collider>());
            Debug.Log("Point!");
        }
    }
}
