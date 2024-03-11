using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweens;

public abstract class Enemy : MonoBehaviour, ICombatEntity
{
    [Header("References")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected Healthbar healthbar;
    [SerializeField] protected EnemyIntent intent;
    [SerializeField] protected Vector3 healthbarOffset;
    [SerializeField] protected float pointerOffset;
    protected readonly Tween shakeTween = new();
    protected Player player;

    [Header("Stats")]
    [SerializeField] protected string enemyName;
    [SerializeField] protected int maxHealth;
    [SerializeField] protected int speed;
    [SerializeField] protected TurnAction[] actions;
    protected int turnCounter = 0;

    public string Name => enemyName;
    public int Speed => speed;

    public Battle Battle { get; set; }
    public int Health { get; set; }
    private int defense = 0;
    public int Defense 
    { 
        get => defense; 
        set
        {
            defense = Mathf.Max(0, value);
            healthbar.UpdateDefense(defense);
        }
    }
    public Turn Turn { get; set; }
    public Animator Animator => animator;
    public float PointerOffset => pointerOffset;
    public bool IsDead { get; set; }
    public Vector3 BattlePosition { get; set; }
    public int MaxHealth => maxHealth;
    public Healthbar Healthbar => healthbar;

    protected virtual void Awake()
    {
        Health = maxHealth;

        Matrix4x4 matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        healthbar = Instantiate(healthbar, matrix.MultiplyPoint3x4(healthbarOffset), Quaternion.identity);
        healthbar.GenerateHealth(Health);

        player = Level.Instance.Player;
    }

    public virtual void Damage(int damage)
    {
        int initialDamage = damage;
        damage = Mathf.Max(0, damage - Defense);
        Defense -= initialDamage;

        Health -= damage;
        healthbar.Damage(damage);
        transform.Shake(0.2f, 0.25f, shakeTween);

        if (Health <= 0)
        {
            AudioManager.Instance.DeathSound();
            Battle.Enemies.Remove(this);
            IsDead = true;

            shakeTween.SetOnComplete(Die);
        }
        else
            AudioManager.Instance.HitSound();
    }

    protected virtual void Die()
    {
        Debug.Log(Name + " dies.");

        Destroy(healthbar.gameObject);
        Destroy(gameObject);
    }

    protected abstract Turn ChooseTurnInternal();

    public void ChooseTurn()
    {
        Turn turn = ChooseTurnInternal();
        intent.Enable();

        switch(turn.Action)
        {
            case AttackAction attack:
                intent.SetAttack(attack.Damage);
                break;
            case DefendAction defense:
                intent.SetDefense(defense.Defense);
                break;
            case HealAction heal:
                intent.SetHeal(heal.Heal);
                break;
            case SinAction:
                intent.SetSin();
                break;
            case WaitAction:
                intent.SetWait();
                break;
            default:
                break;
        }
        
        Turn = turn;
        Battle.SubmitTurn(Turn);
        turnCounter++;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + (pointerOffset * Vector3.up), 0.5f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + healthbarOffset, new(3, 1, 0.05f));
    }

    public virtual void Heal(int heal)
    {
        if (Health == maxHealth)
            return;

        int previousHealth = Health;
        Health += heal;

        if (Health > maxHealth)
            Health = maxHealth;

        healthbar.Heal(Health - previousHealth);
    }

    public virtual void OnExecutingTurn()
    {
        Defense = 0;
        intent.Disable();
    }
}