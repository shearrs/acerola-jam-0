using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : Singleton<CombatManager>
{
    [SerializeField] private LotsBox lotsBox;
    private Player player;

    private Battle Battle => player.Battle;

    private void Start()
    {
        player = Level.Instance.Player;
    }

    public void ChooseTurn(PlayerTurnType type)
    {
        switch(type)
        {
            case PlayerTurnType.STAFF:
                Staff(Battle.GetEnemy(player.EnemyIndex));
                break;
            case PlayerTurnType.DEFEND:
                Defend();
                break;
            case PlayerTurnType.PETITION:
                Petition();
                break;
        }    
    }

    private void Staff(Enemy target)
    {
        int damage = lotsBox.ReleaseLotsOfType(LotType.DAMAGE).Count;

        target.Damage(damage);
    }

    private void Defend()
    {
        int defense = lotsBox.ReleaseLotsOfType(LotType.PROTECTION).Count;

        player.Defense += defense;
    }

    private void Petition()
    {
        // deal with temptation
    }
}