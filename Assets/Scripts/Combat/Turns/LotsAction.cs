using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Lots Action", menuName = "Turn/Lots")]
public class LotsAction : TurnAction
{
    protected override void PerformInternal(Turn turn)
    {
        Debug.Log("apply lots");
    }
}