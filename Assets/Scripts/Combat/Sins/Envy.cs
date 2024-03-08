using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Envy : Sin
{
    public override void ApplyEffect()
    {
        SinUI.Instance.ActivateUI(SinType.ENVY);
    }

    public override SinType GetSinType()
    {
        return SinType.ENVY;
    }

    public override void Purify()
    {
        throw new System.NotImplementedException();
    }
}
