using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public class ShaderParameter : VolumeParameter<Shader>
{
    public ShaderParameter(Shader shader, bool overrideState = false) : base(shader, overrideState)
    {

    }
}
