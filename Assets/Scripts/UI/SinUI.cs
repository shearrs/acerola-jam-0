using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinUI : Singleton<SinUI>
{
    [SerializeField] private SinImage greed;

    public void AddSin(Sin sin)
    {
        if (sin is Greed)
            greed.Enable();
    }

    public void RemoveSin(Sin sin)
    {
        int count = Level.Instance.Player.GetAmountOfSin(sin);

        if (count > 0)
            return;

        if (sin is Greed)
            greed.Disable();
    }
}
