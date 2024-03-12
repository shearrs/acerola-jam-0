using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CustomUI;

public abstract class ToggleableButton : MonoBehaviour
{
    private UIManager uiManager;
    private Image image;
    private TextMeshProUGUI text;
    private Button button;
    private Color originalImageColor;
    private Color originalTextColor;

    protected virtual void OnEnable()
    {
        if (button == null)
        {
            uiManager = UIManager.Instance;
            button = GetComponent<Button>();
            image = GetComponent<Image>();
            text = GetComponentInChildren<TextMeshProUGUI>();
            originalImageColor = image.color;
            originalTextColor = text.color;
        }
    }

    public virtual void Enable()
    {
        image.color = originalImageColor;
        text.color = originalTextColor;
        button.enabled = true;
    }

    public virtual void Disable()
    {
        image.color = uiManager.DefaultUIData.DisabledImageColor;
        text.color = uiManager.DefaultUIData.DisabledTextColor;
        button.enabled = false;
    }

    public virtual void OnClicked()
    {
        AudioManager.Instance.ButtonSound(2);
        OnClickedInternal();
    }

    public abstract void OnClickedInternal();
}