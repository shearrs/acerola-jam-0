using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthbar : MonoBehaviour
{
    [SerializeField] private PlayerHeart playerHeart;
    private readonly List<PlayerHeart> livingHearts = new();
    private readonly List<PlayerHeart> deadHearts = new();

    [Header("Positioning")]
    [SerializeField] private Vector2 startPosition;
    [SerializeField] private float horizontalPadding;
    [SerializeField] private float verticalPadding;
    [SerializeField] private int heartsPerRow;
    private Player player;

    private void Awake()
    {
        player = Level.Instance.Player;
    }

    public void GenerateHearts()
    {
        if (player == null)
            player = Level.Instance.Player;

        PlayerHeart first = Instantiate(playerHeart, transform);
        first.RectTransform.anchoredPosition = startPosition;
        livingHearts.Add(first);

        AddHearts(player.Health - 1);
    }

    public void UpdateHealth()
    {
        int health = player.Health;
        int change = health - livingHearts.Count;

        Debug.Log("change: " + change);

        if (change > 0)
            HealHearts(change);
        else if (change < 0)
            DamageHearts(-change);
    }

    private void HealHearts(int amount)
    {
        if (amount > deadHearts.Count)
            amount = deadHearts.Count;

        for (int i = 0; i < amount; i++)
        {
            PlayerHeart heart = deadHearts[^1];
            heart.Heal();
            deadHearts.RemoveAt(deadHearts.Count - 1);
            livingHearts.Add(heart);
        }
    }

    private void DamageHearts(int amount)
    {
        if (amount > livingHearts.Count)
            amount = livingHearts.Count;

        for (int i = 0; i < amount; i++)
        {
            PlayerHeart heart = livingHearts[^1];
            heart.Damage();
            livingHearts.RemoveAt(livingHearts.Count - 1);
            deadHearts.Add(heart);
        }
    }

    private void AddHearts(int change)
    {
        int row = livingHearts.Count / heartsPerRow;

        for (int i = 0; i < change; i++)
        {
            PlayerHeart heart = Instantiate(playerHeart, transform);
            Vector2 position = startPosition;
            position.x += (livingHearts.Count - (row * heartsPerRow)) * horizontalPadding;
            position.y -= row * verticalPadding;

            heart.RectTransform.anchoredPosition = position;

            livingHearts.Add(heart);

            if (livingHearts.Count - (row * heartsPerRow) >= heartsPerRow)
                row++;
        }
    }
}
