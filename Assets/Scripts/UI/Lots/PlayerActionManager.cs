using CustomUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static UnityEngine.GraphicsBuffer;

[Serializable]
public class PlayerActionManager
{
    [SerializeField] private PlayerVisual playerVisual;
    [SerializeField] private PlayerAction playerAction;

    private LotsBox lotsBox;
    private Player player;
    private ActionUI actionUI;
    private bool enabled = false;

    public Battle Battle => player.Battle;

    public void Initialize()
    {
        player = Level.Instance.Player;
        actionUI = UIManager.Instance.ActionUI;
        lotsBox = CombatManager.Instance.LotsBox;
    }

    public void EnterPhase()
    {
        enabled = true;

        Turn turn = new(player, null, playerAction);
        player.Turn = turn;

        actionUI.Enable();
        Battle.StartTurns();
    }

    public void ExitPhase()
    {
        if(!enabled) 
            return;

        enabled = false;
    }

    public void ChooseTurn(PlayerTurnType type)
    {
        string name = "";
        Action selectedAction = null;
        Action<Action> selectedVisual = null;
        Turn turn = new(player, null, playerAction);

        switch(type)
        {
            case PlayerTurnType.STAFF:
                name = "Staff";
                Enemy enemy = Battle.GetEnemy(player.EnemyIndex);
                selectedAction = () => Staff(enemy);
                selectedVisual = (onComplete) => ActionVisual("Attack", onComplete);
                turn.Target = enemy;
                break;
            case PlayerTurnType.DEFEND:
                name = "Defend";
                selectedAction = Defend;
                selectedVisual = (onComplete) => ActionVisual("Defend", onComplete);
                turn.Target = player;
                break;
            case PlayerTurnType.PETITION:
                name = "Petition";
                selectedAction = Petition;
                selectedVisual = PetitionVisual;
                turn.Target = player;
                break;
        }

        playerVisual.SelectedVisual = selectedVisual;
        playerAction.SelectedAction = selectedAction;
        playerAction.SetName(name);
        player.Turn = turn;

        Battle.SubmitTurn(turn);
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
        player.Staff.SetActive(true);
        int holiness = lotsBox.ReleaseLotsOfType(LotType.HOLY).Count;

        if (holiness >= 3)
        {
            player.TestSins();
        }
        else
            player.Heal(holiness);
    }

    private void ActionVisual(string animation, Action onComplete)
    {
        player.Animator.PlayAndNotify(player, animation, onComplete);
    }

    private void PetitionVisual(Action onComplete)
    {
        player.Staff.SetActive(false);
        ActionVisual("Petition", onComplete);
    }
}