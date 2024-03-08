using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lust : Sin
{
    public override void ApplyEffect()
    {
        SinUI.Instance.ActivateUI(SinType.LUST);
    }

    public override SinType GetSinType()
    {
        return SinType.LUST;
    }

    public override void Purify()
    {
    }
}
