using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Defend", menuName = "Turn/Defend")]
public class DefendAction : TurnAction
{
    [SerializeField] private int defense;

    public int Defense => defense;

    protected override void PerformInternal(Turn turn)
    {
        turn.Target.Defense += defense;
    }
}
