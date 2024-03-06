using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ControlPoint
{
    public Transform Parent => parent;
    public Vector3 Position { get => parent.TransformPoint(position); set => position = parent.InverseTransformPoint(value); }
    public Quaternion Rotation { get => Quaternion.Normalize(parent.rotation * rotation); set => rotation = Quaternion.Inverse(parent.rotation) * value; }
    public Vector3 Scale { get => scale; set => scale = value; }

    public Transform parent;
    [SerializeField] private Vector3 position;
    [SerializeField] private Quaternion rotation;
    [SerializeField] private Vector3 scale;

    public Vector3 Forward => Rotation * Vector3.forward;

    public ControlPoint(Transform parent)
    {
        this.parent = parent;
        Position = Vector3.zero;
        Rotation = Quaternion.identity;
        Scale = Vector3.one;
    }

    public ControlPoint(Transform parent, Vector3 position, Quaternion rotation)
    {
        this.parent = parent;
        Position = position;
        Rotation = rotation;
        Scale = Vector3.one;
    }

    public ControlPoint(Transform parent, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        this.parent = parent;
        Position = position;
        Rotation = rotation;
        Scale = scale;
    }

    public Vector3 LocalToWorldPosition(Vector3 localPosition)
    {
        return Position + Rotation * localPosition;
    }
}