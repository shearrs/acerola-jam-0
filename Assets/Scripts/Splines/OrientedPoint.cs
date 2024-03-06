using UnityEngine;

public struct OrientedPoint
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;

    public OrientedPoint(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        this.position = position;
        this.rotation = rotation;
        this.scale = scale;
    }

    public readonly Vector3 LocalToWorldPosition(Vector3 localPosition)
    {
        localPosition.x *= scale.x;
        localPosition.y *= scale.y;
        localPosition.z *= scale.z;

        return position + rotation * localPosition;
    }

    public readonly Vector3 LocalToWorldVector(Vector3 localPosition)
    {
        return rotation * localPosition;
    }
}