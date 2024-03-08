using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinUI : Singleton<SinUI>
{
    [SerializeField] private SinImage pride;
    [SerializeField] private SinImage greed;
    [SerializeField] private SinImage lust;
    [SerializeField] private SinImage envy;
    [SerializeField] private SinImage gluttony;
    [SerializeField] private SinImage wrath;
    [SerializeField] private SinImage sloth;

    public void ActivateUI(SinType type)
    {
        GetSinImageForType(type).OnActivation();
    }

    public void AddSin(SinType type)
    {
        GetSinImageForType(type).Enable();
    }

    public void RemoveSin(SinType type)
    {
        GetSinImageForType(type).Disable();
    }

    private SinImage GetSinImageForType(SinType type)
    {
        switch (type)
        {
            case SinType.PRIDE:
                return pride;
            case SinType.GREED:
                return greed;
            case SinType.LUST:
                return lust;
            case SinType.ENVY:
                return envy;
            case SinType.GLUTTONY:
                return gluttony;
            case SinType.WRATH:
                return wrath;
            case SinType.SLOTH:
                return sloth;
            default:
                return null;
        }
    }
}
