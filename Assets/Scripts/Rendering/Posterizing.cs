using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using System;

[Serializable, VolumeComponentMenu("Post-processing/Custom/Posterizing")]
public sealed class Posterizing : CustomPostProcessVolumeComponent, IPostProcessComponent
{
    public ComputeShaderParameter ditherShader = new(null);
    public ShaderParameter pointShader = new(null);
    public Vector2IntParameter resolution = new(new(910, 512));
    public ClampedIntParameter factor = new(3, 1, 11);
    public ClampedFloatParameter darknessThreshold = new(0.25f, 0, 1);
    public ClampedFloatParameter brightnessModifier = new(1.5f, 1, 2);

    RTHandle currentSource;
    RTHandle currentDestination;
    Material pointMat;

    int sourceID = Shader.PropertyToID("_Source");
    int destinationID = Shader.PropertyToID("_Destination");
    int widthID = Shader.PropertyToID("width");
    int factorID = Shader.PropertyToID("factor");
    int darknessThresholdID = Shader.PropertyToID("darknessThreshold");
    int brightnessModifierID = Shader.PropertyToID("brightnessModifier");

    public bool IsActive() => pointMat != null && pointShader.value != null && ditherShader.value != null;

    // Do not forget to add this post process in the Custom Post Process Orders list (Project Settings > Graphics > HDRP Global Settings).
    public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

    public override void Setup()
    {
        if (pointShader.value != null && ditherShader.value != null && pointMat == null)
            pointMat = new Material(pointShader.value);

        currentSource = RTHandles.Alloc(resolution.value.x, resolution.value.y, 1, DepthBits.None, UnityEngine.Experimental.Rendering.GraphicsFormat.R8G8B8A8_SRGB, FilterMode.Point, TextureWrapMode.Repeat, TextureDimension.Tex2DArray, true);
        currentDestination = RTHandles.Alloc(resolution.value.x, resolution.value.y, 1, DepthBits.None, UnityEngine.Experimental.Rendering.GraphicsFormat.R8G8B8A8_SRGB, FilterMode.Point, TextureWrapMode.Repeat, TextureDimension.Tex2DArray, true);
    }

    public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
    {
        ComputeShader shader = ditherShader.value;

        cmd.Blit(source, currentSource, 0, 0);

        cmd.SetComputeTextureParam(shader, 0, sourceID, currentSource.nameID);
        cmd.SetComputeTextureParam(shader, 0, destinationID, currentDestination.nameID);
        cmd.SetComputeIntParam(shader, widthID, resolution.value.x);
        cmd.SetComputeIntParam(shader, factorID, factor.value);
        cmd.SetComputeFloatParam(shader, darknessThresholdID, darknessThreshold.value);
        cmd.SetComputeFloatParam(shader, brightnessModifierID, brightnessModifier.value);

        cmd.DispatchCompute(shader, 0, resolution.value.y/32, 1, 1);

        cmd.Blit(currentDestination, destination, 0, 0);
    }

    public override void Cleanup()
    {
        CoreUtils.Destroy(pointMat);
        RTHandles.Release(currentSource);
        RTHandles.Release(currentDestination);
    }
}
