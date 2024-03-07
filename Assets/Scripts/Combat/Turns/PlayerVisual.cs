using CustomUI;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Visual", menuName = "Turn/Visual/Player")]
public class PlayerVisual : TurnActionVisual
{
    public Action<Action> SelectedVisual { get; set; }

    public override void PerformVisual(Turn turn, Action onComplete)
    {
        SelectedVisual?.Invoke(onComplete);
    }
}
