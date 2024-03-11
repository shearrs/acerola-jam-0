using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PurifyButton : MonoBehaviour
{
    [SerializeField] private bool heal;
    [SerializeField] private Color disabledImageColor;
    [SerializeField] private Color disabledTextColor;
    private PetitionManager petitionManager;
    private Image image;
    private TextMeshProUGUI text;
    private Button button;
    private Color originalImageColor;
    private Color originalTextColor;

    private void OnEnable()
    {
        if (button == null)
        {
            petitionManager = PetitionManager.Instance;
            button = GetComponent<Button>();
            image = GetComponent<Image>();
            text = GetComponentInChildren<TextMeshProUGUI>();
            originalImageColor = image.color;
            originalTextColor = text.color;
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
        AudioManager.Instance.ButtonSound(2);
        petitionManager.SelectMenuOption(heal);
    }
}