using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthbar : MonoBehaviour
{
    [SerializeField] private PlayerHeart playerHeart;
    private readonly List<PlayerHeart> hearts = new();

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
        hearts.Add(first);

        AddHearts(player.Health - 1);
    }

    public void UpdateHealth()
    {
        int health = player.Health;
        int change = health - hearts.Count;

        Debug.Log("change: " + change);

        if (change > 0)
            AddHearts(change);
        else if (change < 0)
            RemoveHearts(change);
    }

    private void AddHearts(int change)
    {
        int row = hearts.Count / heartsPerRow;

        for (int i = 0; i < change; i++)
        {
            PlayerHeart heart = Instantiate(playerHeart, transform);
            Vector2 position = startPosition;
            position.x += (hearts.Count - (row * heartsPerRow)) * horizontalPadding;
            position.y -= row * verticalPadding;

            heart.RectTransform.anchoredPosition = position;

            hearts.Add(heart);

            if (hearts.Count - (row * heartsPerRow) >= heartsPerRow)
                row++;
        }
    }

    private void RemoveHearts(int change)
    {
        change = Mathf.Abs(change);

        for (int i = 0; i < change; i++)
        {
            if (hearts.Count == 0)
                return;

            PlayerHeart heart = hearts[^1];
            hearts.RemoveAt(hearts.Count - 1);
            Destroy(heart.gameObject);
        }
    }
}
