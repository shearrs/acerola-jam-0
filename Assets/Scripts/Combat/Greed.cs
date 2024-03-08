using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Greed : Sin
{
    public override void ApplyEffect()
    {
        Level.Instance.Player.LotCapacity--;
        Debug.Log("applying greed");
    }

    public override void Purify()
    {
        Level.Instance.Player.LotCapacity++;
    }
}