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

    public ICombatEntity User { get => user; set => user = value; }
    public ICombatEntity Target { get => target; set => target = value; }
    public TurnAction Action { get => action; set => action = value; }
    public bool Finished { get; set; }

    public Turn(ICombatEntity user, ICombatEntity target, TurnAction action)
    {
        this.user = user;
        this.target = target;
        this.action = action;
    }

    public void Perform()
    {
        if (User != null)
        {
            string text = User.Name + " used " + action.Name;

            if (Target != null)
                text += " on " + Target.Name;

            Debug.Log(text);
        }

        User.OnExecutingTurn();
        action.Perform(this);
    }
}

public abstract class TurnAction : ScriptableObject
{

    [SerializeField] protected TurnActionVisual visual;
    [SerializeField] protected string actionName;

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