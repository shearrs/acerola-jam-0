using CustomUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerActionManager
{
    [SerializeField] private PlayerVisual playerVisual;
    [SerializeField] private PlayerAction playerAction;

    private LotsBox lotsBox;
    private Player player;
    private ActionUI actionUI;
    private bool enabled = false;

    private readonly int petitionID = Animator.StringToHash("Petition");

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
                Enemy enemy = GetTarget();
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
            case PlayerTurnType.PETITION: // rather than actually setting petition here, just open the petition menu and that will set the player action
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

    private Enemy GetTarget()
    {
        if (player.HasSin(SinType.LUST))
        {
            int choice = UnityEngine.Random.Range(0, 6);

            if (choice < 4 && Battle.Enemies.Count > 1)
            {
                SinUI.Instance.ActivateUI(SinType.LUST);
                return Battle.GetRandomEnemy(player.EnemyIndex);
            }
            else
                return Battle.GetEnemy(player.EnemyIndex);
        }
        else
            return Battle.GetEnemy(player.EnemyIndex);
    }

    private void Staff(Enemy target)
    {
        int damage = lotsBox.ReleaseLotsOfType(LotType.DAMAGE).Count;
        damage *= player.StaffStrength;
        
        Wrath wrath = (Wrath)player.GetSin(SinType.WRATH);
        wrath?.DamagePlayer();

        target.Damage(damage);
    }

    private void Defend()
    {
        int defense = lotsBox.ReleaseLotsOfType(LotType.PROTECTION).Count;
        defense *= player.DefendStrength;

        CombatManager.Instance.DefenseDisplay.IsEnabled = defense > 0;
        player.Defense += defense;
    }

    private void Petition()
    {
        player.Staff.SetActive(true);
    }

    private void ActionVisual(string animation, Action onComplete)
    {
        player.Animator.PlayAndNotify(player, animation, onComplete);
    }

    private void PetitionVisual(Action onComplete)
    {
        player.StartCoroutine(IEPetitionVisual(onComplete));
    }

    private IEnumerator IEPetitionVisual(Action onComplete)
    {
        Animator animator = player.Animator;

        player.Staff.SetActive(false);
        animator.Play(petitionID);

        while (!animator.IsPlaying("Petition"))
            yield return null;

        float length = animator.GetCurrentAnimatorStateInfo(0).length;

        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime / length < 0.5f)
            yield return null;

        float speed = animator.speed;
        animator.speed = 0;

        PetitionManager.Instance.Enable();

        while (PetitionManager.Instance.IsEnabled) // while we are selecting what to do with our menu, wait
            yield return null;

        if (player.SelectedSin != null)
        {
            player.RemoveSin(player.SelectedSin);
            lotsBox.ReleaseLotsOfType(LotType.HOLY, 3);
        }
        else
        {
            player.Heal(player.SelectedHeal * player.HealStrength);
            player.SelectedHeal = 0;
        }

        while (player.PurifyingSin)
            yield return null;

        animator.speed = speed;

        while (animator.IsPlaying("Petition"))
            yield return null;

        onComplete?.Invoke();
    }
}