using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    [SerializeField] private bool drawGizmos;
    [SerializeField] private Waypoint[] waypoints;

    public int WaypointCount => waypoints.Length;

    public Vector3 GetPosition(int index)
    {
        return waypoints[index].Position;
    }

    private void OnDrawGizmos()
    {
        if (!drawGizmos || WaypointCount == 0)
            return;

        for (int i = 0; i < waypoints.Length; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(waypoints[i].Position, 0.25f);

            if (i < waypoints.Length - 1)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawLine(waypoints[i].Position, waypoints[i + 1].Position);
            }
        }
    }
}