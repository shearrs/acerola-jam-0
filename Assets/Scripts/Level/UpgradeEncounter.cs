using CustomUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class UpgradeEncounter : Encounter
{
    [SerializeField] private bool blessed;
    [SerializeField] private Volume encounterVolume;
    private UpgradeUI upgradeUI;

    private void Start()
    {
        upgradeUI = UpgradeUI.Instance;
    }

    public override void Enter()
    {
        encounterVolume.gameObject.SetActive(true);
        StartCoroutine(IEEncounterFog(true));

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
        {
            Player player = Level.Instance.Player;

            foreach(Sin sin in player.Sins)
            {
                player.RemoveSin(sin);
            }
        }

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
        StartCoroutine(IEEncounterFog(false));
    }

    private IEnumerator IEEncounterFog(bool enable)
    {
        float start;
        float end;
        float time;
        float elapsedTime = 0;
        const float timeToFadeIn = 0.3f;
        const float timeToFadeOut = 4f;

        if (enable)
        {
            encounterVolume.gameObject.SetActive(true);
            start = 0;
            end = 1;
            time = timeToFadeIn;
        }
        else
        {
            start = 1;
            end = 0;
            time = timeToFadeOut;
        }

        while (elapsedTime < time)
        {
            float percentage = elapsedTime / time;
            encounterVolume.weight = Mathf.Lerp(start, end, percentage);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        if (!enable)
            encounterVolume.gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
}
