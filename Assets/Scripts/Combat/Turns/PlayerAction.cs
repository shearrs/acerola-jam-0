using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Action", menuName = "Turn/Player")]
public class PlayerAction : TurnAction
{
    public Action SelectedAction { get; set; }

    protected override void PerformInternal(Turn turn)
    {
        SelectedAction?.Invoke();
    }

    public void SetName(string name)
    {
        actionName = name;
    }
}