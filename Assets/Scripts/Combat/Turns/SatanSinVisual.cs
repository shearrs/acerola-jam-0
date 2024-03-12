using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatanSinVisual : TurnActionVisual
{
    public override void PerformVisual(Turn turn, Action onComplete)
    {
        Satan satan = (Satan)turn.User;

        satan.SinAction();
    }
}
