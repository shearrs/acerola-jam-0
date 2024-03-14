using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CustomUI;

public abstract class ToggleableButton : MonoBehaviour
{
    protected UIManager uiManager;
    protected Image image;
    protected TextMeshProUGUI text;
    protected Button button;
    protected Color originalImageColor;
    protected Color originalTextColor;

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
        gameObject.SetActive(true);
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

    protected abstract void OnClickedInternal();
}