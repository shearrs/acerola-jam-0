using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gluttony : Sin
{
    public override void ApplyEffect()
    {
        SinUI.Instance.ActivateUI(SinType.GLUTTONY);
    }

    public override SinType GetSinType()
    {
        return SinType.GLUTTONY;
    }

    public override void Purify()
    {
        throw new System.NotImplementedException();
    }
}
