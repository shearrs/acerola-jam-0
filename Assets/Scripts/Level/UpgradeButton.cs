using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] private PlayerTurnType upgradeTarget;
    [SerializeField] private TextMeshProUGUI text;

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
                level = Level.Instance.Player.StaffStrength;
            else if (upgradeTarget == PlayerTurnType.DEFEND)
                level = Level.Instance.Player.DefendStrength;
            else
                level = Level.Instance.Player.HealStrength;

            if (level == 0)
                text.fontSharedMaterial = bronzeMaterial;
            else if (level == 1)
                text.fontSharedMaterial = silverMaterial;
            else
                text.fontSharedMaterial = goldMaterial;
        }
    }

    public void SelectUpgrade()
    {
        Level.Instance.Player.UpgradeAction(upgradeTarget);
    }
}
