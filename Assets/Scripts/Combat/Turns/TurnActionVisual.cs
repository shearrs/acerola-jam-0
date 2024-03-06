using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TurnActionVisual : ScriptableObject
{
    public abstract void PerformVisual(Turn turn, Action onComplete);
}