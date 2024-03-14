using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    private CombatManager combatManager;

    private void Start()
    {
        combatManager = CombatManager.Instance;
    }

    public void StartCombat()
    {
        combatManager.EnterPhase(CombatPhase.LOTS);
        gameObject.SetActive(false);

        AudioManager audioManager = AudioManager.Instance;
        audioManager.ButtonSound();

        AudioClip song;
        CombatEncounter encounter = (CombatEncounter)Level.Instance.CurrentEncounter;

        if (encounter.SatanFight)
            song = audioManager.SatanMusic;
        else
            song = audioManager.BattleMusic;

        audioManager.PlaySong(song);
    }
}
