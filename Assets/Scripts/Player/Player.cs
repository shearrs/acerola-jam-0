using CustomUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, ICombatEntity
{
    [Header("Movement")]
    [SerializeField, Range(0, 100)] private float speed = .35f;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float bobbingHeight;
    [SerializeField] private float bobbingFrequency;
    [SerializeField] private AudioSource stepAudioSource;
    private bool step = false;

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

    public float MovementSpeed => speed;
    public float RotationSpeed => rotationSpeed;
    
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
            else if (value > defense)
            {
                AudioManager.Instance.DefenseSound();
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
        animator.SetBool(isWalkingID, true);

        Path path = Level.Instance.CurrentEncounter.Path;
        float bobbingCounter = 0;

        for (int waypointIndex = 0; waypointIndex < path.WaypointCount; waypointIndex++)
        {
            Vector3 target = path.GetPosition(waypointIndex);
            Vector3 rotationTarget = (waypointIndex == path.WaypointCount - 1) ? target : path.GetPosition(waypointIndex + 1);

            Vector3 rotationDirection = (rotationTarget - transform.position).normalized;

            Quaternion targetRotation = Quaternion.LookRotation(rotationDirection);
            targetRotation.z = 0;

            while ((target - transform.position).sqrMagnitude > 0.1)
            {
                while (PauseManager.Instance.Paused)
                    yield return null;

                Vector3 position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                float bobFactor = Mathf.Sin(bobbingCounter * bobbingFrequency);
                position.y += bobbingHeight * bobFactor;

                if (bobFactor < -0.75 && !step)
                {
                    AudioManager.RandomizePitch(stepAudioSource);
                    stepAudioSource.Play();
                    step = true;
                }
                else if (bobFactor > 0.75 && step)
                    step = false;

                Quaternion rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                rotation.z = 0;
                transform.SetPositionAndRotation(position, rotation);

                bobbingCounter += Time.deltaTime;
                yield return null;
            }
        }

        animator.SetBool(isWalkingID, false);

        Level.Instance.StartEncounter();

        // rotate to the encounter's rotation
        float elapsedTime = 0;
        const float timeToRotate = .2f;
        Transform encounter = Level.Instance.CurrentEncounter.transform;
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = encounter.rotation;
        while (elapsedTime < timeToRotate)
        {
            float percent = elapsedTime / timeToRotate;
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, percent);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.SetPositionAndRotation(encounter.position, encounter.rotation);
    }

    public void Damage(int damage)
    {
        int originalDamage = damage;

        damage = Mathf.Max(0, damage - Defense);
        Defense = Mathf.Max(0, Defense - originalDamage);

        Health -= damage;

        if (damage > 0)
            actionUI.OnPlayerHealthChanged(-damage);

        if (Health <= 0)
            Die();
        else
        {
            if (damage == 0)
                AudioManager.Instance.ShieldSound();
            else
                AudioManager.Instance.HitSound(null);

            CameraManager.Instance.Shake();
        }
    }

    private void Die()
    {
        AudioManager.Instance.DeathSound();
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
}