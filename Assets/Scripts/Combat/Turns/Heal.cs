using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Heal", menuName = "Turn/Heal")]
public class Heal : TurnAction
{
    [SerializeField] private int healAmount;

    protected override void PerformInternal(Turn turn)
    {
        turn.Target.Heal(healAmount);
        Debug.Log(turn.Target.Name + " healed " + healAmount + " health.");
    }
}