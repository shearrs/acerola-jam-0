using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweens;
using System;

namespace CustomUI
{
    public class UIManager : StaticInstance<UIManager>
    {
        [SerializeField] private UIData defaultUIData;

        private List<Tween> tweens;
        private Dictionary<RectTransform, Tween> priorityTweens;

        public UIData DefaultUIData => defaultUIData;

        [Header("Elements")]
        [SerializeField] private RectTransform bottomBar;
        [SerializeField] private Vector3 bbTargetPosition;
        [SerializeField] private PlayerHealthbar healthbar;
        [SerializeField] private ShepherdPortrait portrait;
        private Vector3 bbOriginalPosition;

        [Header("Sub Classes")]
        [SerializeField] private ActionUI actionUI;
        [SerializeField] private LotsUI lotsUI;

        public ShepherdPortrait Portrait => portrait;
        public ActionUI ActionUI => actionUI;
        public LotsUI LotsUI => lotsUI;

        private void Start()
        {
            tweens = new List<Tween>(5);
            priorityTweens = new(5);

            for (int i = 0; i < tweens.Capacity; i++)
            {
                tweens.Insert(i, new());
            }

            actionUI.Initialize();

            bbOriginalPosition = bottomBar.anchoredPosition;
        }

        #region Game Specific
        public void EnterEncounter()
        {
            portrait.CombatPosition();
        }

        public void EndEncounter()
        {
            portrait.DefaultPosition();
        }

        public void ToggleBar(bool open, Action onComplete = null, bool toggleHealthbar = false)
        {
            Tween tween;

            if (open)
            {
                tween = Move(bottomBar, bbTargetPosition);

                if (toggleHealthbar)
                    tween.SetOnComplete(() => healthbar.gameObject.SetActive(true));
            }
            else
            {
                tween = Move(bottomBar, bbOriginalPosition);

                if (toggleHealthbar)
                    healthbar.gameObject.SetActive(false);
            }

            if (onComplete != null)
                tween.SetOnComplete(onComplete);
        }
        #endregion

        #region General
        public Tween Open(RectTransform element) => Open(element, defaultUIData);

        public Tween Open(RectTransform element, UIData uiData)
        {
            if (IsPriorityTweening(element))
                return null;

            element.gameObject.SetActive(true);
            element.localScale = uiData.InitialOpenScale;
            Tween tween = element
                .DoTweenScaleNonAlloc(Vector3.one, uiData.OpenDuration, GetTween())
                .SetEasingFunction(uiData.OpenEasingFunction);

            MarkPriorityTween(element, tween);

            return tween;
        }

        public Tween Close(RectTransform element) => Close(element, defaultUIData);
        public Tween Close(RectTransform element, UIData uiData)
        {
            if (IsPriorityTweening(element))
                return null;

            Tween tween = element
                .DoTweenScaleNonAlloc(uiData.InitialOpenScale, uiData.CloseDuration, GetTween())
                .SetOnComplete(() => DeactivateElement(element))
                .SetEasingFunction(uiData.CloseEasingFunction);

            MarkPriorityTween(element, tween);

            return tween;
        }

        public Tween Move(RectTransform element, Vector2 position) => Move(element, position, defaultUIData);
        public Tween Move(RectTransform element, Vector2 position, UIData uiData)
        {
            Tween tween = element
                .DoTweenPositionNonAlloc(position, uiData.MoveDuration, GetTween())
                .SetEasingFunction(uiData.MoveEasingFunction);

            return tween;
        }

        public Tween Shake(RectTransform element, float duration = -1, float amount = -1)
        {
            Vector2 originalPosition = element.anchoredPosition;

            if (duration == -1)
                duration = defaultUIData.ShakeDuration;

            if (amount == -1)
                amount = defaultUIData.ShakeAmount;


            Tween tween = TweenManager.DoTweenCustomNonAlloc
                (
                (percentage) => ShakeUpdate(element, originalPosition, percentage, amount),
                duration, 
                GetTween()
                )
                .SetOnComplete(() => ResetShake(element, originalPosition));

            return tween;
        }

        private void ShakeUpdate(RectTransform element, Vector2 originalPosition, float percentage, float amount)
        {
            float magnitude = amount * (1 - percentage);
            element.anchoredPosition = originalPosition + UnityEngine.Random.insideUnitCircle * magnitude;
        }

        private void ResetShake(RectTransform element, Vector2 originalPosition)
        {
            element.anchoredPosition = originalPosition;
        }
        #endregion

        #region Internal
        private Tween GetTween()
        {
            foreach (Tween tween in tweens)
            {
                if (!tween.IsPlaying)
                    return tween;
            }

            Tween newTween = new();
            tweens.Add(newTween);

            return newTween;
        }

        private void DeactivateElement(RectTransform element) => element.gameObject.SetActive(false);

        private bool IsPriorityTweening(RectTransform element)
        {
            return priorityTweens.ContainsKey(element);
        }

        private void MarkPriorityTween(RectTransform element, Tween tween)
        {
            priorityTweens.Add(element, tween);
            tween.SetOnComplete(() => priorityTweens.Remove(element));
        }
        #endregion
    }
}