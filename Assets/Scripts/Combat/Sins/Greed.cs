using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Greed : Sin
{
    public override void ApplyEffect()
    {
        SinUI.Instance.ActivateUI(SinType.GREED);
    }

    public override SinType GetSinType()
    {
        return SinType.GREED;
    }

    public override void Purify()
    {
        throw new System.NotImplementedException();
    }
}