using System;
using System.Collections;
using System.Collections.Generic;
using Tweens;
using UnityEngine;

public class Healthbar : MonoBehaviour
{
    [Header("Tick Generation")]
    [SerializeField] private HealthTick healthTickPrefab;
    [SerializeField] private DefenseTick defenseTickPrefab;
    [SerializeField] private float distanceBetween;
    [SerializeField] private int amountPerRow;
    [SerializeField] private AudioSource audioSource;

    [Header("Tweens")]
    [SerializeField] private float tickSpawnDelay;

    private Tween growTween = new();
    private int maxHealth;
    private int currentHealth;
    private bool corrupt;
    private DefenseTick defenseTick;
    private readonly List<HealthTick> livingTicks = new();
    private readonly List<HealthTick> deadTicks = new();

    public void Enable()
    {
        gameObject.SetActive(true);
        transform.DoTweenScaleNonAlloc(Vector3.one, .4f, growTween).SetEasingFunction(EasingFunctions.EasingFunction.OUT_BACK);
    }

    public void Disable()
    {
        transform.DoTweenScaleNonAlloc(Vector3.zero, .2f, growTween).SetOnComplete(() => gameObject.SetActive(false));
    }

    public void Damage(int change)
    {
        if (change > currentHealth)
            change = currentHealth;

        currentHealth -= change;

        for (int i = 0; i < change; i++)
        {
            HealthTick tick = livingTicks[^1];
            bool corrupt = tick.CorruptHeart;

            tick.Damage();

            if (!corrupt)
            {
                livingTicks.RemoveAt(livingTicks.Count - 1);
                deadTicks.Add(tick);
            }

            if (livingTicks.Count == 0)
                break;
        }
    }

    public void Heal(int change)
    {
        if (change > maxHealth)
            change = maxHealth - currentHealth;

        currentHealth += change;
        int initialChange = change;

        if (corrupt)
        {
            change /= 2;

            if (!livingTicks[^1].CorruptHeart)
            {
                livingTicks[^1].CorruptHeart = true;
                change--;
                initialChange--;
            }
        }

        for (int i = 0; i < change; i++)
        {
            HealthTick tick = deadTicks[^1];
            tick.Heal();
            deadTicks.RemoveAt(deadTicks.Count - 1);
            livingTicks.Add(tick);

            if (corrupt)
                tick.CorruptHeart = true;

            if (deadTicks.Count == 0)
                return;
        }

        // integer division could make us heal one less
        if (initialChange > change)
        {
            HealthTick tick = deadTicks[^1];
            tick.Heal();
            deadTicks.RemoveAt(deadTicks.Count - 1);
            livingTicks.Add(tick);
        }
    }

    public void GenerateHealth(int health)
    {
        maxHealth = health;
        currentHealth = health;
        corrupt = maxHealth > 10;

        Vector3 direction = (Level.Instance.Player.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

        int positioner = Mathf.Min(health, amountPerRow);

        Vector3 firstPosition = transform.position + (positioner / 2) * distanceBetween * transform.right;

        StartCoroutine(IEAddTicks(firstPosition, health));
    }

    private IEnumerator IEAddTicks(Vector3 start, int health)
    {
        Vector3 horizontalStep = distanceBetween * -transform.right;
        Vector3 verticalStep = distanceBetween * transform.up;
        WaitForSeconds wait = new(tickSpawnDelay);

        // if we have greater than 10 health, then only produce half the max health and turn all the hearts into corrupt hearts
        if (corrupt)
            health /= 2;

        for (int i = 0; i < health; i++)
        {
            int row = i / amountPerRow;
            int index = i - (row * amountPerRow);

            audioSource.pitch = 0.8f;
            audioSource.Play();
            HealthTick tick = AddTick(start + (index * horizontalStep) + (row * verticalStep));

            yield return wait;

            if (corrupt)
            {
                audioSource.pitch = 0.4f;
                audioSource.Play();
                tick.CorruptHeart = corrupt;
                yield return wait;
            }
        }

        Vector3 defensePosition;

        if (health > amountPerRow)
            defensePosition = start + ((amountPerRow + 1) * horizontalStep);
        else
            defensePosition = start + (health * horizontalStep);

        defenseTick = Instantiate(defenseTickPrefab, defensePosition, transform.rotation, transform);
    }

    private HealthTick AddTick(Vector3 position)
    {
        HealthTick tick = Instantiate(
                healthTickPrefab,
                position,
                transform.rotation,
                transform);

        livingTicks.Add(tick);
        tick.Spawn();

        return tick;
    }

    public void UpdateDefense(int defense) => defenseTick.UpdateDefense(defense);
}