using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointTrigger : ResettingMonoBehaviour
{
    public GenericWinCondition winCondition;
    public pointType trigger;
    public bool playerSpecific = false;
    public Player.Identity identity;

    private List<Collider> ignoredCollisions = new List<Collider>();

    private void Start()
    {
        if (winCondition == null) winCondition = FindObjectsOfType<GenericWinCondition>()[0];
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (trigger)
        {
            case pointType.player:
                GenericController player = other.GetComponent<GenericController>();
                if (player != null)
                {
                    player.AddPoint();
                    Collider curColl = other.GetComponentInParent<Collider>();
                    Physics.IgnoreCollision(curColl, GetComponent<Collider>());
                    ignoredCollisions.Add(other.GetComponentInParent<Collider>());
                }
                break;
            case pointType.item:
                HoldableObject obj = other.GetComponent<HoldableObject>();
                if (obj != null && (playerSpecific && obj.lastHolder && obj.lastHolder.identity == identity))
                {
                    obj.lastHolder.AddPoint();
                    obj.readyToRespawn = true;
                    obj.Drop();
                    obj.transform.position = new Vector3(-500, -500, -500);
                }
                break;
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

    public enum pointType
    {
        player,
        item
    }
}
