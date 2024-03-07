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
    }
}
