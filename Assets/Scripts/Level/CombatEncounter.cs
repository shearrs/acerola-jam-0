using CustomUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatEncounter : Encounter
{
    [SerializeField] private List<Enemy> enemies;
    [SerializeField] private Vector3[] enemyPositions;
    [SerializeField] private Battle battle;
    private CombatUI combatUI;

    public Battle Battle => battle;

    public override void Enter()
    {
        combatUI ??= UIManager.Instance.CombatUI;

        battle = new(this, enemies, GetRelativePositions());
    }

    private Vector3[] GetRelativePositions()
    {
        Vector3[] relativePositions = new Vector3[enemyPositions.Length];

        for (int i = 0; i < enemyPositions.Length; i++)
            relativePositions[i] = transform.TransformPoint(enemyPositions[i]);

        return relativePositions;
    }

    public void PlayCombat(List<Turn> turns)
    {
        StartCoroutine(IEPlay(turns));
    }

    private IEnumerator IEPlay(List<Turn> turns)
    {
        WaitForSeconds wait = new(combatUI.TimeBetweenTurns);
        int count = turns.Count;

        combatUI.SetActions(false);
        turns.Sort(OrderTurns);

        foreach (Turn turn in turns)
        {
            if (turn.User.IsDead)
                continue;

            turn.Perform();

            while (!turn.Finished)
                yield return null;

            yield return wait;
        }

        if (!Level.Instance.Player.IsDead)
        {
            if (battle.Enemies.Count == 0)
            {
                EndEncounter();
            }
            else
            {
                battle.StartTurns();
                combatUI.SetActions(true);
            }
        }
        else
            EndEncounter();
    }

    protected override void EndEncounter()
    {
        UIManager.Instance.EndEncounter(this);
        Level.Instance.EndEncounter();
    }

    private static int OrderTurns(Turn turn1, Turn turn2)
    {
        if (turn1.User.Speed > turn2.User.Speed)
            return -1;
        else if (turn1.User.Speed < turn2.User.Speed)
            return 1;
        else 
            return 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        foreach (Vector3 position in enemyPositions)
        {
            Gizmos.DrawWireSphere(transform.TransformPoint(position), 0.2f);
        }
    }
}