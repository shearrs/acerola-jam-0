using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum PlayerTurnType { STAFF, DEFEND, PETITION }

public class PlayerActionButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerTurnType turnType;
    [SerializeField] private Color disabledImageColor;
    [SerializeField] private Color disabledTextColor;
    [SerializeField] private TextMeshProUGUI counterText;
    private Button button;
    private Color originalImageColor;
    private Color originalTextColor;
    private Image image;
    private TextMeshProUGUI text;
    private CombatManager combatManager;

    [Header("Upgrades")]
    [SerializeField] private Material bronzeMaterial;
    [SerializeField] private Material silverMaterial;
    [SerializeField] private Material goldMaterial;
    private int level = 0;

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
        counterText.text = "x" + (CombatManager.Instance.LotsBox.GetAmountOfType(GetLotType()) * Level.Instance.Player.GetStrengthForType(turnType));
    }

    public void Disable()
    {
        image.color = disabledImageColor;
        text.color = disabledTextColor;
        button.enabled = false;
        counterText.text = "";
    }
    
    public void SubmitAction()
    {
        AudioManager.Instance.ButtonSound();
        combatManager.ChooseTurn(turnType);
    }

    public void Upgrade()
    {
        level++;

        if (text == null)
            text = GetComponentInChildren<TextMeshProUGUI>();

        if (level == 1)
            text.fontSharedMaterial = silverMaterial;
        else if (level == 2)
            text.fontSharedMaterial = goldMaterial;
    }

    private LotType GetLotType()
    {
        switch(turnType)
        {
            case PlayerTurnType.STAFF:
                return LotType.DAMAGE;
            case PlayerTurnType.DEFEND:
                return LotType.PROTECTION;
            case PlayerTurnType.PETITION:
                return LotType.HOLY;
            default:
                return LotType.DAMAGE;
        }
    }
}