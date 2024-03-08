using System;
using System.Collections;
using System.Collections.Generic;
using Tweens;
using UnityEngine;

public class Healthbar : MonoBehaviour
{
    [Header("Tick Generation")]
    [SerializeField] private HealthTick healthTick;
    [SerializeField] private float distanceBetween;
    [SerializeField] private int amountPerRow;

    [Header("Tweens")]
    [SerializeField] private float tickSpawnDelay;

    private readonly List<HealthTick> ticks = new();

    public void Damage(int change)
    {
        if (change > ticks.Count)
            change = ticks.Count;

        for (int i = 0; i < change; i++)
        {
            ticks[^1].Damage();
            ticks.RemoveAt(ticks.Count - 1);

            if (ticks.Count == 0)
                break;
        }
    }

    public void GenerateHealth(int health)
    {
        Vector3 direction = (Level.Instance.Player.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

        int positioner = Mathf.Min(health, amountPerRow);

        Vector3 firstPosition = transform.position + (positioner / 2) * distanceBetween * transform.right;

        StartCoroutine(IEAddTicks(firstPosition, health));
    }

    private IEnumerator IEAddTicks(Vector3 start, int num)
    {
        Vector3 horizontalStep = distanceBetween * -transform.right;
        Vector3 verticalStep = distanceBetween * transform.up;
        WaitForSeconds wait = new(tickSpawnDelay);

        for (int i = 0; i < num; i++)
        {
            int row = i / amountPerRow;
            int index = i - (row * amountPerRow);

            AddTick(start + (index * horizontalStep) + (row * verticalStep));

            yield return wait;
        }
    }

    private void AddTick(Vector3 position)
    {
        HealthTick tick = Instantiate(
                healthTick,
                position,
                transform.rotation,
                transform);

        ticks.Add(tick);
        tick.Spawn();
    }
}