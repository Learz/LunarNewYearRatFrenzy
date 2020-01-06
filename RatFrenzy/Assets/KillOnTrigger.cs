using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class KillOnTrigger : MonoBehaviour
{
    public SurvivorWinCondition winCondition;

    private void OnTriggerEnter(Collider other)
    {
        GenericController player = other.GetComponent<GenericController>();
        if (player != null)
        {
            player.GetComponent<ParticleSystem>().Play();
            winCondition.EliminatePlayer(player.identity);

        }
    }
}
