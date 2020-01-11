using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointTrigger : ResettingMonoBehaviour
{
    public GenericWinCondition winCondition;

    private List<Collider> ignoredCollisions = new List<Collider>();

    private void Start()
    {
        if (winCondition == null) winCondition = FindObjectsOfType<GenericWinCondition>()[0];
    }

    private void OnTriggerEnter(Collider other)
    {
        GenericController player = other.GetComponent<GenericController>();
        if (player != null)
        {
            player.AddPoint();
            Collider curColl = other.GetComponentInParent<Collider>();
            Physics.IgnoreCollision(curColl, GetComponent<Collider>());
            ignoredCollisions.Add(other.GetComponentInParent<Collider>());
        }
    }

    public override void ResetOnSpawn()
    {
        foreach (Collider coll in ignoredCollisions)
        {
            Physics.IgnoreCollision(coll, GetComponent<Collider>(), false);
        }
        ignoredCollisions.Clear();
    }
}
