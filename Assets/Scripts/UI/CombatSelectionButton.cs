using CustomUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatSelectionButton : MonoBehaviour
{
    [SerializeField] private Color disabledColor;
    [SerializeField] private bool next;
    private CombatUI combatUI;
    private Button button;
    private Image image;
    private Color initialColor;

    private int Change => (next) ? 1 : -1;

    private void OnEnable()
    {
        if (combatUI == null)
        {
            combatUI = UIManager.Instance.CombatUI;
            image = GetComponent<Image>();
            button = GetComponent<Button>();

            initialColor = image.color;
        }
    }

    public void UpdateIndex()
    {
        combatUI.UpdateEnemyPointer(Change);
    }

    public void Enable()
    {
        button.enabled = true;
        image.color = initialColor;
    }

    public void Disable()
    {
        button.enabled = false;
        image.color = disabledColor;
    }
}