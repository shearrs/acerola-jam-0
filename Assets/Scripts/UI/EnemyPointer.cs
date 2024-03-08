using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweens;

public class EnemyPointer : MonoBehaviour
{
    [SerializeField] private Tween movementTween;
    private readonly Tween rotationTween = new();
    private Battle battle;
    private Player player;

    public Battle Battle => battle;

    private void Awake()
    {
        player = Level.Instance.Player;
    }

    public void StartCombat(Battle battle)
    {
        this.battle = battle;
    }

    public void Enable()
    {
        transform.gameObject.SetActive(true);
        SetPosition(true);
    }

    public void Disable()
    {
        transform.gameObject.SetActive(false);
    }

    public void CombatEnded()
    {
        battle = null;
        Disable();
    }

    public void SetPosition(bool initialize = false)
    {
        Enemy enemy = battle.GetEnemy(player.EnemyIndex);
        Vector3 position = enemy.BattlePosition;
        position.y += enemy.PointerOffset;
        Quaternion rotation = Quaternion.LookRotation((Camera.main.transform.position - position).normalized, Vector3.up);

        if (initialize)
            transform.SetPositionAndRotation(position, rotation);
        else
        {
            transform.DoTweenPositionNonAlloc(position, movementTween.Duration, movementTween).SetEasingFunction(EasingFunctions.EasingFunction.OUT_BACK);
            transform.DoTweenRotationNonAlloc(rotation, movementTween.Duration, rotationTween);
        }
    }
}
