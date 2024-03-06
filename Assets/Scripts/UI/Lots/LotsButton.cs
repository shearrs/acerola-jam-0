using CustomUI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LotsButton : MonoBehaviour
{
    private const string CAST_TEXT = "CAST LOTS";
    private const string CONFIRM_TEXT = "CONFIRM";

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Color disabledImageColor;
    [SerializeField] private Color disabledTextColor;
    private Color originalImageColor;
    private Color originalTextColor;
    private Image image;
    private Button button;
    private LotsUI lotsUI;
    private Player player;

    private bool confirmable = false;

    private void Start()
    {
        player = Level.Instance.Player;
        lotsUI = UIManager.Instance.LotsUI;

        image = GetComponent<Image>();
        button = GetComponent<Button>();

        originalImageColor = image.color;
        originalTextColor = text.color;
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

    public void UpdateState(int lots)
    {
        if (!confirmable && lots == 0)
        {
            confirmable = true; 
            text.text = CONFIRM_TEXT;
        }
        else if (confirmable && lots > 0)
        {
            confirmable = false;
            text.text = CAST_TEXT;
        }
    }

    public void LotsAction()
    {
        if (confirmable)
            lotsUI.ConfirmLots();
        else
            lotsUI.ThrowLots();
    }
}