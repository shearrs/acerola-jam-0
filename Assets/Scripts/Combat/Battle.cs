using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Battle
{
    [SerializeField] private List<Turn> turns = new();
    private readonly List<Enemy> enemies;
    private readonly Vector3[] positions;
    private readonly Player player;
    private readonly CombatEncounter encounter;

    public List<Enemy> Enemies => enemies;
    public List<Turn> Turns => turns;

    public Battle(CombatEncounter encounter, List<Enemy> enemies, Vector3[] positions)
    {
        this.encounter = encounter;
        this.enemies = enemies;
        this.positions = positions;
        player = Level.Instance.Player;

        player.Battle = this;

        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            Vector3 direction = (player.transform.position - positions[i]).normalized;

            Enemy enemy = enemies[i];

            enemy.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            enemy.Battle = this;
            enemy.BattlePosition = positions[i];
            enemy.Enable();
        }
    }

    public void StartTurns()
    {
        ClearTurns();

        foreach (Enemy enemy in enemies)
            enemy.ChooseTurn();
    }

    public void SubmitTurn(Turn turn)
    {
        turns.Add(turn);

        if (AllTurnsSubmitted())
        {
            encounter.PlayCombat(turns);
        }
    }

    public bool AllEnemiesDead()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] != null)
                return false;
        }

        return true;
    }

    public Enemy GetEnemy(int index)
    {
        if (index < enemies.Count && enemies[index] != null)
            return enemies[index];

        int newIndex = index - 1;
        Enemy enemy = null;

        while (newIndex >= 0 && enemy == null)
        {
            enemy = enemies[newIndex];
            newIndex--;
        }

        if (enemy != null)
            return enemy;

        newIndex = index + 1;
        while(index < enemies.Count && enemy == null)
        {
            enemy = enemies[newIndex];
            newIndex++;
        }

        return enemy;
    }

    public Enemy GetRandomEnemy(int exclusionIndex)
    {
        List<Enemy> possibleEnemies = new List<Enemy>(enemies.Count - 1);

        for (int i = 0; i < enemies.Count; i++)
        {
            if (i != exclusionIndex)
                possibleEnemies.Add(enemies[i]);
        }

        int random = Random.Range(0, possibleEnemies.Count);

        return possibleEnemies[random];
    }

    private bool AllTurnsSubmitted()
    {
        if (player.Turn == null)
            return false;

        foreach(Enemy enemy in enemies)
        {
            if (enemy.Turn == null)
                return false;
        }

        return true;
    }

    private void ClearTurns()
    {
        player.Turn = null;

        foreach (Enemy enemy in enemies)
            enemy.Turn = null;

        turns.Clear();
    }
}