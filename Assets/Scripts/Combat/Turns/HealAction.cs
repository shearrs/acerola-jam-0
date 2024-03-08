using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Heal", menuName = "Turn/Heal")]
public class HealAction : TurnAction
{
    [SerializeField] private int heal;

    public int Heal => heal;

    protected override void PerformInternal(Turn turn)
    {
        turn.Target.Heal(heal);
        Debug.Log(turn.Target.Name + " healed " + heal + " health.");
    }
}