using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Tweens;

public class EnemyIntent : MonoBehaviour
{
    [SerializeField] private SpriteRenderer intentSprite;
    [SerializeField] private TextMeshPro intentText;
    [SerializeField] private Sprite damageSprite;
    [SerializeField] private Sprite defenseSprite;
    [SerializeField] private Sprite healSprite;
    private Tween tween = new();

    public void Enable()
    {
        gameObject.SetActive(true);
        transform.localScale = Vector3.zero;
        transform.DoTweenScaleNonAlloc(Vector3.one, 0.25f, tween).SetEasingFunction(EasingFunctions.EasingFunction.OUT_BACK);

        Vector3 direction = (Level.Instance.Player.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(-direction, Vector3.up);
    }

    public void Disable()
    {
        void onComplete()
        {
            gameObject.SetActive(false);
            intentSprite.sprite = null;
            intentText.text = "";
        }

        transform.DoTweenScaleNonAlloc(Vector3.zero, 0.25f, tween).SetEasingFunction(EasingFunctions.EasingFunction.IN_BACK).SetOnComplete(onComplete);
    }

    public void SetAttack(int damage)
    {
        intentSprite.sprite = damageSprite;
        intentText.text = "x" + damage;
    }

    public void SetDefense(int defense)
    {
        intentSprite.sprite = defenseSprite;
        intentText.text = "x" + defense;
    }

    public void SetHeal(int heal)
    {
        intentSprite.sprite = healSprite;
        intentText.text = "x" + heal;
    }
}
