using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICombatEntity
{
    public int MaxHealth { get; }
    public int Health { get; }
    public int Defense { get; }
    public string Name { get; }
    public int Speed { get; }
    public bool IsDead { get; }
    public Animator Animator { get; }

    public void Heal(int heal);
    public void Damage(int damage);
    public void OnExecutingTurn();
}
