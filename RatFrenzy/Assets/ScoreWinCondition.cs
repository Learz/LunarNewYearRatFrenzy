using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreWinCondition : GenericWinCondition
{
    public override void AddPoint(Player.Identity id)
    {
        base.AddPoint(id);
        if (scores[(int)id] == maxScore) EndGame(id);
        Debug.Log("Point added for " + id);
    }
}
