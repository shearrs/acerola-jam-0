using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Encounter : MonoBehaviour
{
    [SerializeField] private Path path;

    public Path Path => path;

    public abstract void Enter();

    protected abstract void EndEncounter();
}