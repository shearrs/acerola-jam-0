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
        button.enabled = true;
        image.color = initialColor;
    }

    public void Disable()
    {
        button.enabled = false;
        image.color = disabledColor;
    }

    public void UpdateIndex()
    {
        if (next)
        {
            combatUI.UpdateEnemyPointer(1);
            AudioManager.Instance.UISource.pitch = 1;
        }
        else
        {
            combatUI.UpdateEnemyPointer(-1);
            AudioManager.Instance.UISource.pitch = 0.75f;
        }

        AudioManager.Instance.ButtonSound(2, false);
    }
}