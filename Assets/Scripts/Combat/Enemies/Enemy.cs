using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweens;

public class Enemy : MonoBehaviour, ICombatEntity
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Healthbar healthbar;
    [SerializeField] private EnemyIntent intent;
    [SerializeField] private Vector3 healthbarOffset;
    [SerializeField] private float pointerOffset;
    private readonly Tween shakeTween = new();

    [Header("Stats")]
    [SerializeField] private string enemyName;
    [SerializeField] private int maxHealth;
    [SerializeField] private int speed;
    [SerializeField] private TurnAction[] actions;

    public string Name => enemyName;
    public int Speed => speed;

    public Battle Battle { get; set; }
    public int Health { get; set; }
    public int Defense { get; set; }
    public Turn Turn { get; set; }
    public Animator Animator => animator;
    public float PointerOffset => pointerOffset;
    public bool IsDead { get; set; }
    public Vector3 BattlePosition { get; set; }
    public int MaxHealth => maxHealth;

    private void Awake()
    {
        Health = maxHealth;

        Matrix4x4 matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        healthbar = Instantiate(healthbar, matrix.MultiplyPoint3x4(healthbarOffset), Quaternion.identity);
        healthbar.GenerateHealth(Health);
    }

    public void Damage(int damage)
    {
        Health -= damage;
        healthbar.Damage(damage);
        transform.Shake(0.2f, 0.25f, shakeTween);

        if (Health <= 0)
        {
            Battle.Enemies.Remove(this);
            IsDead = true;

            shakeTween.SetOnComplete(Die);
        }
    }

    private void Die()
    {
        Debug.Log(Name + " dies.");

        Destroy(healthbar);
        Destroy(gameObject);
    }

    private TurnAction GetAction()
    {
        int index = Random.Range(0, actions.Length);
        return actions[index];
    }

    public void ChooseTurn()
    {
        TurnAction action = GetAction();
        intent.Enable();

        if (action is Attack attack)
            intent.SetAttack(attack.Damage);

        Turn = new(this, Level.Instance.Player, action);

        Battle.SubmitTurn(Turn);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + (pointerOffset * Vector3.up), 0.5f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + healthbarOffset, new(3, 1, 0.05f));
    }

    public void Heal(int heal)
    {
        Health += heal;

        if (Health > maxHealth)
            Health = maxHealth;
    }

    public void OnExecutingTurn()
    {
        intent.Disable();
    }
}