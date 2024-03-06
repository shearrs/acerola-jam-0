using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerTurnType { STAFF, DEFEND, PETITION }

public class PlayerActionButton : MonoBehaviour
{
    [SerializeField] private PlayerTurnType turnType;
    [SerializeField] private Color disabledImageColor;
    [SerializeField] private Color disabledTextColor;
    private Button button;
    private Color originalImageColor;
    private Color originalTextColor;
    private Image image;
    private TextMeshProUGUI text;
    private CombatManager combatManager;

    private void OnEnable()
    {
        if (button == null)
        {
            button = GetComponent<Button>();
            image = GetComponent<Image>();
            text = GetComponentInChildren<TextMeshProUGUI>();
            originalImageColor = image.color;
            originalTextColor = text.color;
            combatManager = CombatManager.Instance;
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
        combatManager.ChooseTurn(turnType);
    }
}