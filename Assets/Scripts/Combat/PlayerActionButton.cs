using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum PlayerTurnType { STAFF, DEFEND, PETITION }

public class PlayerActionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
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
        counterText.text = "x" + CombatManager.Instance.LotsBox.GetAmountOfType(GetLotType());
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
        combatManager.ChooseTurn(turnType);
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (turnType == PlayerTurnType.DEFEND)
        {

        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }
}