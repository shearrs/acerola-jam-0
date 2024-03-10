using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SinType { PRIDE, GREED, LUST, ENVY, GLUTTONY, WRATH, SLOTH }

public abstract class Sin
{
    public abstract SinType GetSinType();
    public abstract void ApplyEffect();
    public abstract void Purify();

    public static Sin GetSinForType(SinType type)
    {
        return type switch
        {
            SinType.PRIDE => new Pride(),
            SinType.GREED => new Greed(),
            SinType.LUST => new Lust(),
            SinType.ENVY => new Envy(),
            SinType.GLUTTONY => new Gluttony(),
            SinType.WRATH => new Wrath(),
            SinType.SLOTH => new Sloth(),
            _ => null,
        };
    }
}