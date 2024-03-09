using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthbar : MonoBehaviour
{
    [Header("Tick Generation")]
    [SerializeField] private HealthTick healthTickPrefab;
    [SerializeField] private DefenseTick defenseTickPrefab;
    [SerializeField] private float distanceBetween;
    [SerializeField] private int amountPerRow;

    [Header("Tweens")]
    [SerializeField] private float tickSpawnDelay;

    private DefenseTick defenseTick;
    private readonly List<HealthTick> livingTicks = new();
    private readonly List<HealthTick> deadTicks = new();

    public void Damage(int change)
    {
        if (change > livingTicks.Count)
            change = livingTicks.Count;

        for (int i = 0; i < change; i++)
        {
            HealthTick tick = livingTicks[^1];
            tick.Damage();
            livingTicks.RemoveAt(livingTicks.Count - 1);
            deadTicks.Add(tick);

            if (livingTicks.Count == 0)
                break;
        }
    }

    public void Heal(int change)
    {
        if (change > deadTicks.Count)
            change = deadTicks.Count;

        for (int i = 0; i < change; i++)
        {
            HealthTick tick = deadTicks[^1];
            tick.Heal();
            deadTicks.RemoveAt(deadTicks.Count - 1);
            livingTicks.Add(tick);

            if (deadTicks.Count == 0)
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

        Vector3 defensePosition;

        if (num > amountPerRow)
            defensePosition = start + ((amountPerRow + 1) * horizontalStep);
        else
            defensePosition = start + (num * horizontalStep);

        defenseTick = Instantiate(defenseTickPrefab, defensePosition, transform.rotation, transform);
    }

    private void AddTick(Vector3 position)
    {
        HealthTick tick = Instantiate(
                healthTickPrefab,
                position,
                transform.rotation,
                transform);

        livingTicks.Add(tick);
        tick.Spawn();
    }

    public void UpdateDefense(int defense) => defenseTick.UpdateDefense(defense);
}