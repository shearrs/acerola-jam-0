using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SinType { PRIDE, GREED, LUST, ENVY, GLUTTONY, WRATH, SLOTH }

public abstract class Sin
{
    public abstract SinType GetSinType();
    public abstract void ApplyEffect();
    public abstract void Purify();
}