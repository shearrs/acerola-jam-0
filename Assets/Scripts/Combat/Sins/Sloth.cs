using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sloth : Sin
{
    public override void ApplyEffect()
    {
        Level.Instance.Player.Speed -= 2;
    }

    public override SinType GetSinType()
    {
        return SinType.SLOTH;
    }

    public override void Purify()
    {
        Level.Instance.Player.Speed += 2;
    }
}