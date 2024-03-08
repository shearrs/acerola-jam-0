using CustomUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, ICombatEntity
{
    [Header("Movement")]
    [SerializeField, Range(0, 1)] private float speed = 1;
    [SerializeField] private float bobbingHeight;
    [SerializeField] private float bobbingFrequency;
    private Vector3 previousPosition;
    private Vector3 moveDirection;

    [Header("Combat")]
    [SerializeField] private Animator animator;
    [SerializeField] private int maxHealth;
    [SerializeField] private int combatSpeed;
    [SerializeField] private int lotCapacity;
    [SerializeField] private GameObject staff;
    private UIManager uiManager;
    private ActionUI combatUI;
    private readonly List<Sin> sins = new();

    private readonly int isWalkingID = PlayerAnimationData.IsWalking;

    public string Name => "Shepherd";
    public int Health { get; private set; }
    public int Defense { get; set; }
    public Battle Battle { get; set; }
    public Turn Turn { get; set; }
    public Animator Animator => animator;
    public int Speed => combatSpeed;
    public int EnemyIndex { get; set; }
    public bool IsDead { get; private set; } = false;
    public int MaxHealth => maxHealth;
    public int LotCapacity 
    { 
        get => lotCapacity;
        set => lotCapacity = Mathf.Max(2, value);
    }
    public GameObject Staff => staff;

    private void Awake()
    {
        Health = maxHealth;
    }

    private void Start()
    {
        uiManager = UIManager.Instance;
        combatUI = uiManager.ActionUI;
    }

    public void Move()
    {
        StartCoroutine(IEMove());
    }

    private IEnumerator IEMove()
    {
        Spline path = Level.Instance.CurrentEncounter.Path;
        animator.SetBool(isWalkingID, true);
        float progress = 0;

        while (progress < 1)
        {
            OrientedPoint sample = path.GetBezierPoint(progress);
            Vector3 position = sample.position;
            position.y += bobbingHeight * Mathf.Sin(progress * bobbingFrequency);

            previousPosition = transform.position;
            transform.position = position;

            moveDirection = (transform.position - previousPosition);
            moveDirection.y = 0;
            moveDirection.Normalize();

            Quaternion rotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = rotation;

            progress += Time.deltaTime * speed;

            yield return null;
        }

        animator.SetBool(isWalkingID, false);
        Level.Instance.StartEncounter();
    }

    public void Damage(int damage)
    {
        damage = Mathf.Max(0, damage - Defense);
        Defense = Mathf.Max(0, Defense - damage);

        Health -= damage;

        if (Health <= 0)
            Die();
        else
        {
            CameraManager.Instance.Shake();
            combatUI.OnPlayerHealthChanged(-damage);
        }
    }

    private void Die()
    {
        CameraManager.Instance.Shake(0.1f, 2f);
        IsDead = true;
    }

    public void Heal(int heal)
    {
        Health += heal;

        if (Health > maxHealth)
        {
            heal -= (Health - maxHealth);
            Health = maxHealth;
        }

        combatUI.OnPlayerHealthChanged(heal);
    }

    public int GetAmountOfSin(Sin type)
    {
        int count = 0;

        foreach (Sin sin in sins)
        {
            if (sin.GetType() == type.GetType())
                count++;
        }

        return count;
    }

    public void AddSin(Sin sin)
    {
        SinUI.Instance.AddSin(sin);
        sins.Add(sin);
        sin.ApplyEffect();
    }

    public void TestSins()
    {
        RemoveSin(sins[0]);
    }

    public void RemoveSin(Sin sin)
    {
        Debug.Log("remove sin");
        sins.Remove(sin);
        SinUI.Instance.RemoveSin(sin);
        sin.Purify();
    }

    public void OnExecutingTurn()
    {
    }
}