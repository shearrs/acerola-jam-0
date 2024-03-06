using CustomUI;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Lots Visual", menuName = "Turn/Visual/Lots")]
public class LotsVisual : TurnActionVisual
{
    public override void PerformVisual(Turn turn, Action onComplete)
    {
        // tween up the lots ui
        // spawn the different lots

        UIManager.Instance.LotsUI.Enable();
    }
}
