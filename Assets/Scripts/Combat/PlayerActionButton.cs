using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActionButton : MonoBehaviour
{
    [SerializeField] private TurnAction action;
    [SerializeField] private bool isTargetingEnemy;
    [SerializeField] private Color disabledImageColor;
    [SerializeField] private Color disabledTextColor;
    private Button button;
    private Color originalImageColor;
    private Color originalTextColor;
    private Image image;
    private TextMeshProUGUI text;
    private Player player;

    private void OnEnable()
    {
        if (button == null)
        {
            button = GetComponent<Button>();
            image = GetComponent<Image>();
            text = GetComponentInChildren<TextMeshProUGUI>();
            originalImageColor = image.color;
            originalTextColor = text.color;
            player = Level.Instance.Player;
        }
    }

    public void Enable()
    {
        image.color = originalImageColor;
        text.color = originalTextColor;
        button.enabled = true;
    }

    public void Disable()
    {
        image.color = disabledImageColor;
        text.color = disabledTextColor;
        button.enabled = false;
    }
    
    public void SubmitAction()
    {
        ICombatEntity target;

        if (isTargetingEnemy)
            target = player.Battle.GetEnemy(player.EnemyIndex);
        else
            target = player;

        Turn turn = new(player, target, action);

        player.ChooseTurn(turn);
    }
}