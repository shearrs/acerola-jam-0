using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu(fileName="New Enemy Attack", menuName="Turn/Attack")]
public class Attack : TurnAction
{
    [SerializeField] private int damage;
    [SerializeField] private string attackText;

    public int Damage => damage;
    public string AttackText => attackText;

    protected override void PerformInternal(Turn turn)
    {
        turn.Target.Damage(damage);
        Debug.Log("It did " + damage + " damage.");
    }
}