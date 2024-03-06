using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPointer : MonoBehaviour
{
    private Battle battle;
    private Player player;

    public Battle Battle => battle;

    private void Awake()
    {
        player = Level.Instance.Player;
    }

    public void StartCombat(Battle battle)
    {
        this.battle = battle;
    }

    public void Enable()
    {
        transform.gameObject.SetActive(true);
        SetPosition();
    }

    public void Disable()
    {
        transform.gameObject.SetActive(false);
    }

    public void CombatEnded()
    {
        battle = null;
        Disable();
    }

    public void SetPosition()
    {
        Enemy enemy = battle.GetEnemy(player.EnemyIndex);
        Vector3 position = enemy.BattlePosition;
        position.y += enemy.PointerOffset;
        Quaternion rotation = Quaternion.LookRotation((Camera.main.transform.position - position).normalized, Vector3.up);

        transform.SetPositionAndRotation(position, rotation);
    }
}
