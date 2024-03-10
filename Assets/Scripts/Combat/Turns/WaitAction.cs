using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wait", menuName = "Turn/Wait")]
public class WaitAction : TurnAction
{
    protected override void PerformInternal(Turn turn)
    {
        return;
    }
}
