using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTracker : MonoBehaviour
{
    public CheckpointList checkpointList;
    public GenericWinCondition winCondition;
    public int lastCheckpointIndex;
    private GenericController controller;

    private void Start()
    {
        controller = GetComponent<GenericController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        int otherIndex = checkpointList.checkpoints.IndexOf(other.transform);
        if (otherIndex == -1) return;

        if (lastCheckpointIndex != checkpointList.checkpoints.Count - 1 && otherIndex - lastCheckpointIndex == 1) lastCheckpointIndex = otherIndex;
        else if (lastCheckpointIndex == checkpointList.checkpoints.Count - 1 && otherIndex == 0)
        {
            winCondition.AddPoint(GetComponent<GenericController>().identity);
            lastCheckpointIndex = 0;
        }
        else
        {
            if (Math.Abs(otherIndex - lastCheckpointIndex) > 1) controller.Kill();
            return;
        }
        controller.SetRespawnPosition(other.transform.position, other.transform.rotation);
    }

}
