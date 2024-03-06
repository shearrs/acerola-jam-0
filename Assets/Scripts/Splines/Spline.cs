using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Spline : MonoBehaviour
{
    [SerializeField, Range(0.2f, 1)] private float handlesSize = 0.75f;
    [Range(0, 1f)] public float tValue = 0f;
    public List<ControlPoint> controlPoints = new();
    public bool editable = true;

    private int BezierCount => controlPoints.Count - 1;

    public void AddBezier(ControlPoint start, ControlPoint end)
    {
        if (!controlPoints.Contains(start))
        {
            controlPoints.Add(start);
        }

        controlPoints.Add(end);
    }

    public void AddToEnd(ControlPoint end)
    {
        controlPoints.Add(end);
    }

    public void RemoveBezier()
    {
        controlPoints.RemoveAt(controlPoints.Count - 1);
    }

    public OrientedPoint GetBezierPoint(float t)
    {
        float newT = t * BezierCount;
        int index = (int)newT;
        newT -= index;

        if (index == BezierCount)
        {
            index = BezierCount - 1;
            newT = 1;
        }

        ControlPoint start = controlPoints[index];
        ControlPoint end = controlPoints[index + 1];

        return CalculateBezierPoint(start, end, newT);
    }

    public Vector3 GetStartTangentPoint(ControlPoint start) => start.LocalToWorldPosition(Vector3.forward * start.Scale.z);
    public Vector3 GetEndTangentPoint(ControlPoint end) => end.LocalToWorldPosition(Vector3.back * end.Scale.z);

    private OrientedPoint CalculateBezierPoint(ControlPoint start, ControlPoint end, float t)
    {
        Vector3 p0 = start.Position;
        Vector3 p1 = start.LocalToWorldPosition(Vector3.forward * start.Scale.z);
        Vector3 p2 = end.LocalToWorldPosition(Vector3.back * end.Scale.z);
        Vector3 p3 = end.Position;

        Vector3 a = Vector3.Lerp(p0, p1, t);
        Vector3 b = Vector3.Lerp(p1, p2, t);
        Vector3 c = Vector3.Lerp(p2, p3, t);

        Vector3 d = Vector3.Lerp(a, b, t);
        Vector3 e = Vector3.Lerp(b, c, t);

        Vector3 position = Vector3.Lerp(d, e, t);
        Quaternion rotation = Quaternion.Lerp(start.Rotation, end.Rotation, t);

        return new OrientedPoint(position, rotation, Vector3.Lerp(start.Scale, end.Scale, t));
    }
}