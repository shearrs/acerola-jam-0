using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweens;

public class ShepherdPortrait : MonoBehaviour
{
    [SerializeField] private Vector2 defaultPosition;
    [SerializeField] private Vector2 combatPosition;
    [SerializeField] private Tween movementTween;
    [SerializeField] private float movementHeight;
    private RectTransform rect;


    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void CombatPosition()
    {
        Move(combatPosition);
    }

    public void DefaultPosition()
    {
        Move(defaultPosition);
    }

    private void Move(Vector2 to)
    {
        Vector2 from = rect.anchoredPosition;

        void update(float percentage)
        {
            Vector2 lerped = Vector2.Lerp(from, to, percentage);
            float z = movementHeight * Mathf.Sin(percentage * Mathf.PI);
            Vector3 newPos = new(lerped.x, lerped.y, z);

            rect.anchoredPosition3D = newPos;
        }

        TweenManager.DoTweenCustomNonAlloc(update, movementTween.Duration, movementTween).SetOnComplete(() => rect.anchoredPosition3D = to);
    }
}