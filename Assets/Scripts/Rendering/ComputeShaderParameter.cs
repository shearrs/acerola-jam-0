using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public class ComputeShaderParameter : VolumeParameter<ComputeShader>
{
    public ComputeShaderParameter(ComputeShader shader, bool overrideState = false) : base(shader, overrideState)
    {

    }
}