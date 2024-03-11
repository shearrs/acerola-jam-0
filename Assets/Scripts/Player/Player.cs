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

    [SerializeField] private Spline spline;
    [SerializeField, Range(0, 1)] private float t;

    [Header("Combat")]
    [SerializeField] private Animator animator;
    [SerializeField] private int maxHealth;
    [SerializeField] private int combatSpeed;
    [SerializeField] private int lotCapacity;
    [SerializeField] private GameObject staff;
    private UIManager uiManager;
    private ActionUI actionUI;
    private readonly List<Sin> sins = new();
    private readonly int isWalkingID = PlayerAnimationData.IsWalking;

    public string Name => "Shepherd";
    public int Health { get; private set; }
    private int defense;
    public int Defense
    {
        get => defense;
        set
        {
            if (value < defense)
            {
                DefenseToRemove -= (defense - value);
                DefenseToRemove = Mathf.Max(0, DefenseToRemove);
            }

            defense = value;
            CombatManager.Instance.DefenseDisplay.UpdateDefense(defense);
        }
    }
    public Battle Battle { get; set; }
    public Turn Turn { get; set; }
    public Animator Animator => animator;
    public int Speed { get => combatSpeed; set => combatSpeed = value; }
    public int EnemyIndex { get; set; }
    public bool IsDead { get; private set; } = false;
    public int MaxHealth => maxHealth;
    public int LotCapacity 
    { 
        get => lotCapacity;
        set => lotCapacity = Mathf.Max(2, value);
    }
    public bool PurifyingSin { get; set; }
    public Sin SelectedSin { get; set; }
    public int SelectedHeal { get; set; }
    public GameObject Staff => staff;
    public List<Sin> Sins => sins;
    public int SinCount => sins.Count;
    public int StaffStrength { get; private set; } = 1;
    public int DefendStrength { get; private set; } = 1;
    public int HealStrength { get; private set; } = 1;
    public int DefenseToRemove { get; set; }


    private void Awake()
    {
        Health = maxHealth;
    }

    private void Start()
    {
        uiManager = UIManager.Instance;
        actionUI = uiManager.ActionUI;
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
            Vector3 sampleRotation = sample.rotation.eulerAngles;
            Quaternion rotation = Quaternion.Euler(0, sampleRotation.y, sampleRotation.z);

            transform.SetPositionAndRotation(position, rotation);

            float distanceScale = path.GetCurrentSplineDistanceRatio(progress);
            Debug.Log(distanceScale);
            progress += Time.deltaTime * (speed * distanceScale);

            yield return null;
        }

        animator.SetBool(isWalkingID, false);
        Level.Instance.StartEncounter();

        Transform encounter = Level.Instance.CurrentEncounter.transform;
        transform.position = encounter.position;
        transform.rotation = encounter.rotation;
    }

    public void Damage(int damage)
    {
        int originalDamage = damage;

        damage = Mathf.Max(0, damage - Defense);
        Defense = Mathf.Max(0, Defense - originalDamage);

        Health -= damage;

        if (Health <= 0)
            Die();
        else
        {
            CameraManager.Instance.Shake();
            actionUI.OnPlayerHealthChanged(-damage);
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

        actionUI.OnPlayerHealthChanged(heal);
    }

    public int GetStrengthForType(PlayerTurnType type)
    {
        return type switch
        {
            PlayerTurnType.STAFF => StaffStrength,
            PlayerTurnType.DEFEND => DefendStrength,
            PlayerTurnType.PETITION => HealStrength,
            _ => -1,
        };
    }

    public void UpgradeAction(PlayerTurnType type)
    {
        switch (type)
        {
            case PlayerTurnType.STAFF:
                actionUI.UpgradeActionButton(0);
                StaffStrength++;
                break;
            case PlayerTurnType.DEFEND:
                actionUI.UpgradeActionButton(1);
                DefendStrength++;
                break;
            case PlayerTurnType.PETITION:
                actionUI.UpgradeActionButton(2);
                HealStrength++;
                break;
        }
    }

    public bool HasSin(SinType type)
    {
        foreach (Sin sin in sins)
        {
            if (sin.GetSinType() == type)
                return true;
        }

        return false;
    }

    public Sin GetSin(SinType type)
    {
        foreach (Sin sin in sins)
        {
            if (sin.GetSinType() == type)
                return sin;
        }

        return null;
    }

    public void AddSin(Sin sin)
    {
        if (sin == null)
            return;

        if (HasSin(sin.GetSinType()))
            return;

        SinUI.Instance.AddSin(sin.GetSinType());
        sins.Add(sin);
        sin.ApplyEffect();
    }

    public void RemoveSin(Sin sin)
    {
        Debug.Log("remove sin");
        sins.Remove(sin);
        SinUI.Instance.RemoveSin(sin.GetSinType());
        sin.Purify();
    }

    public void OnExecutingTurn()
    {
    }

    private void OnDrawGizmos()
    {
        if (spline == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(spline.GetBezierPoint(t).position, 0.5f);
    }
}