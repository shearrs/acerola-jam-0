using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Turn
{
    [SerializeField] private TurnAction action;
    private ICombatEntity user;
    private ICombatEntity target;

    public ICombatEntity User => user;
    public ICombatEntity Target => target;
    public TurnAction Action => action;
    public bool Finished { get; set; }

    public Turn(ICombatEntity user, ICombatEntity target, TurnAction action)
    {
        this.user = user;
        this.target = target;
        this.action = action;
    }

    public void Perform()
    {
        Debug.Log(User.Name + " used " + action.Name + " on " + target.Name + ".");

        action.Perform(this);
    }
}

public abstract class TurnAction : ScriptableObject
{

    [SerializeField] private TurnActionVisual visual;
    [SerializeField] private string actionName;

    public string Name => actionName;

    public void Perform(Turn turn)
    {
        void onComplete()
        {
            PerformInternal(turn);
            turn.Finished = true;
        }

        if (visual != null)
            visual.PerformVisual(turn, onComplete);
        else
        {
            PerformInternal(turn);
            turn.Finished = true;
        }
    }

    protected abstract void PerformInternal(Turn turn);
}