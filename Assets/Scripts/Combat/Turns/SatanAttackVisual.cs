using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatanAttackVisual : TurnActionVisual
{
    public override void PerformVisual(Turn turn, Action onComplete)
    {
        Satan satan = (Satan)turn.User;

        satan.AttackAction();
    }
}