using CustomUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeEncounter : Encounter
{
    [SerializeField] private bool blessed;
    private UpgradeUI upgradeUI;

    private void Start()
    {
        upgradeUI = UpgradeUI.Instance;
    }

    public override void Enter()
    {
        List<PlayerTurnType> upgradeTypes = GetValidUpgradeTypes();

        if (upgradeTypes.Count == 0)
        {
            EndEncounter();
            return;
        }

        int index1 = Random.Range(0, upgradeTypes.Count);
        upgradeUI.UpgradeButton1.UpgradeTarget = upgradeTypes[index1];
        upgradeTypes.RemoveAt(index1);

        if (upgradeTypes.Count == 0)
            upgradeUI.HasTwoOptions = false;
        else
        {
            int index2 = Random.Range(0, upgradeTypes.Count);
            upgradeUI.UpgradeButton2.UpgradeTarget = upgradeTypes[index2];
            upgradeTypes.RemoveAt(index2);

            upgradeUI.HasTwoOptions = true;
        }

        if (blessed)
            Level.Instance.Player.LotCapacity++;

        upgradeUI.Enable(blessed);
    }

    private List<PlayerTurnType> GetValidUpgradeTypes()
    {
        List<PlayerTurnType> upgradeTypes = new()
        {
            PlayerTurnType.STAFF,
            PlayerTurnType.DEFEND,
            PlayerTurnType.PETITION
        };

        Player player = Level.Instance.Player;
        if (player.StaffStrength >= 3)
            upgradeTypes.Remove(PlayerTurnType.STAFF);
        if (player.DefendStrength >= 3)
            upgradeTypes.Remove(PlayerTurnType.DEFEND);
        if (player.HealStrength >= 3)
            upgradeTypes.Remove(PlayerTurnType.PETITION);

        return upgradeTypes;
    }

    protected override void EndEncounter()
    {
        // make ui disappear
    }
}