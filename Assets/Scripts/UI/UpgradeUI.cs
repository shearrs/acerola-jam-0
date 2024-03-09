using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweens;

public class UpgradeUI : Singleton<UpgradeUI>
{
    [SerializeField] private UpgradeButton upgradeButton1;
    [SerializeField] private UpgradeButton upgradeButton2;
    private Tween tween;

    public UpgradeButton UpgradeButton1 => upgradeButton1;
    public UpgradeButton UpgradeButton2 => upgradeButton2;

    public void Enable()
    {

    }

    public void Disable()
    {

    }
}