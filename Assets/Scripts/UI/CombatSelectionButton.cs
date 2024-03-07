using CustomUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatSelectionButton : MonoBehaviour
{
    [SerializeField] private Color disabledColor;
    [SerializeField] private bool next;
    private ActionUI combatUI;
    private Button button;
    private Image image;
    private Color initialColor;

    private void OnEnable()
    {
        if (combatUI == null)
        {
            combatUI = UIManager.Instance.ActionUI;
            image = GetComponent<Image>();
            button = GetComponent<Button>();

            initialColor = image.color;
        }
    }

    public void Enable()
    {
        Debug.Log("enable");

        button.enabled = true;
        image.color = initialColor;
    }

    public void Disable()
    {
        Debug.Log("disable");

        button.enabled = false;
        image.color = disabledColor;
    }
}