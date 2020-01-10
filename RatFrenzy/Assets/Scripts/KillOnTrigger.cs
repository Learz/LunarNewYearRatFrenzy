using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class KillOnTrigger : MonoBehaviour
{
    public GenericWinCondition winCondition;
    public bool eliminatePlayer;
    private void OnTriggerEnter(Collider other)
    {
        GenericController player = other.GetComponent<GenericController>();
        if (player != null)
        {
            player.Kill();
            if (eliminatePlayer) winCondition.EliminatePlayer(player.identity);
        }
    }
}
