using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Waypoint
{
    [SerializeField] private Vector3 position;

    public Vector3 Position => position;
}