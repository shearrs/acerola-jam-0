using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wrath : Sin
{
    public override void ApplyEffect()
    {
    }

    public void DamagePlayer()
    {
        SinUI.Instance.ActivateUI(SinType.WRATH);
        Level.Instance.Player.Damage(1);
    }

    public override SinType GetSinType()
    {
        return SinType.WRATH;
    }

    public override void Purify()
    {
    }
}
