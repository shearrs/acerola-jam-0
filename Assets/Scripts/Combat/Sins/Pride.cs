using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pride : Sin
{
    public override void ApplyEffect()
    {
        Level.Instance.Player.LotCapacity--;

        Debug.Log("applying pride");
    }

    public override SinType GetSinType()
    {
        return SinType.PRIDE;
    }

    public override void Purify()
    {
        Level.Instance.Player.LotCapacity++;
    }
}