using UnityEngine;

[CreateAssetMenu(fileName="New Enemy Attack", menuName="Turn/Attack")]
public class AttackAction : TurnAction
{
    [SerializeField] private int damage;

    public int Damage => damage;

    protected override void PerformInternal(Turn turn)
    {
        turn.Target.Damage(damage);
        Debug.Log("It did " + damage + " damage.");
    }
}