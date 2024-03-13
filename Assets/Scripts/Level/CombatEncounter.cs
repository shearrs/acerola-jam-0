using CustomUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CombatEncounter : Encounter
{
    [SerializeField] private Volume combatFog;

    [Header("Combat")]
    [SerializeField] private CombatDrop drop;
    [SerializeField] private Battle battle;
    private CombatManager combatManager;

    [Header("Enemies")]
    [SerializeField] private List<Enemy> enemies;
    [SerializeField] private Vector3[] enemyPositions;

    public Battle Battle => battle;

    private void Awake()
    {
        // spawn enemies
        for (int i = 0; i < enemies.Count; i++)
        {
            Enemy enemy = Instantiate(enemies[i], transform.TransformPoint(enemyPositions[i]), Quaternion.identity, transform);
            enemy.gameObject.SetActive(false);

            enemies[i] = enemy;
        }
    }

    public override void Enter()
    {
        if (combatManager == null)
            combatManager = CombatManager.Instance;

        AudioManager.Instance.EncounterSound();
        AudioManager.Instance.PlaySong(null, 1.25f);
        combatManager.Enable();
        battle = new(this, enemies, GetRelativePositions());

        StartCoroutine(IEFog(true));
        battle.StartTurns();
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
        WaitForSeconds wait = new(combatManager.TimeBetweenTurns);
        int count = turns.Count;

        combatManager.EnterPhase(CombatPhase.TURN);
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
                combatManager.EnterPhase(CombatPhase.LOTS);
                battle.StartTurns();
            }
        }
        else
            EndEncounter();
    }

    protected override void EndEncounter()
    {
        combatManager.Disable();
        StartCoroutine(IEFog(false));

        if (drop == CombatDrop.NONE)
        {
            Level.Instance.EndEncounter();
            UIManager.Instance.ToggleBar(false, null, true);
        }
        else 
            CombatDropUI.Instance.Enable(drop);
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

    private IEnumerator IEFog(bool enable)
    {
        float start;
        float end;
        float time;
        const float timeToFadeIn = 0.3f;
        const float timeToFadeOut = 4f;

        if (enable)
        {
            combatFog.gameObject.SetActive(true);
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

        float elapsedTime = 0;

        while(elapsedTime < time)
        {
            float percentage = elapsedTime / time;
            combatFog.weight = Mathf.Lerp(start, end, percentage);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        if (!enable)
            combatFog.gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1f);

        foreach (Vector3 position in enemyPositions)
        {
            Gizmos.DrawWireSphere(transform.TransformPoint(position), 0.2f);
        }
    }
}