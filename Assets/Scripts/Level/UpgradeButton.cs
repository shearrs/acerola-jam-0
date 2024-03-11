using CustomUI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Tweens;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] private PlayerTurnType upgradeTarget;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image image;
    [SerializeField] private Button button;
    [SerializeField] private Color disabledImageColor;
    [SerializeField] private Color disabledTextColor;
    private Color originalImageColor;
    private Color originalTextColor;

    [Header("Materials")]
    [SerializeField] private Material bronzeMaterial;
    [SerializeField] private Material silverMaterial;
    [SerializeField] private Material goldMaterial;

    public PlayerTurnType UpgradeTarget 
    { 
        get => upgradeTarget;
        set
        {
            upgradeTarget = value;

            int level;
            if (upgradeTarget == PlayerTurnType.STAFF)
            {
                text.text = "STAFF";
                level = Level.Instance.Player.StaffStrength;
            }
            else if (upgradeTarget == PlayerTurnType.DEFEND)
            {
                text.text = "DEFEND";
                level = Level.Instance.Player.DefendStrength;
            }
            else
            {
                text.text = "PETITION";
                level = Level.Instance.Player.HealStrength;
            }

            if (level == 0)
                text.fontSharedMaterial = bronzeMaterial;
            else if (level == 1)
                text.fontSharedMaterial = silverMaterial;
            else
                text.fontSharedMaterial = goldMaterial;
        }
    }

    private void Awake()
    {
        originalImageColor = image.color;
        originalTextColor = text.color;
    }

    public void SelectUpgrade()
    {
        AudioManager.Instance.ButtonSound(2, false, 0.45f);
        Level.Instance.Player.UpgradeAction(upgradeTarget);
        UpgradeUI.Instance.Disable();
    }

    public void Enable()
    {
        button.enabled = true;
        image.color = originalImageColor;
        text.color = originalTextColor;
    }

    public void Disable(bool colorChange = false)
    {
        button.enabled = false;

        if (colorChange)
        {
            image.color = disabledImageColor;
            text.color = disabledTextColor;
        }
    }
}
