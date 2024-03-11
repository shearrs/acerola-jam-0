using System;
using UnityEngine;

namespace Tweens
{
    public static class TweenManager
    {
        public static Vector3 TWEEN_ZERO = Vector3.one * 0.001f;

        #region Transforms

        #region Positions
        public static Tween DoTweenPosition(this Transform transform, Vector3 to, float duration)
        {
            Tween tween = CreateTweenPosition(transform, to, duration);

            tween.Start();

            return tween;
        }

        public static Tween CreateTweenPosition(this Transform transform, Vector3 to, float duration)
        {
            Tween tween = new();
            CreateTweenPositionNonAlloc(transform, to, duration, tween);

            return tween;
        }

        public static Tween DoTweenPositionNonAlloc(this Transform transform, Vector3 to, float duration, Tween tween)
        {
            CreateTweenPositionNonAlloc(transform, to, duration, tween);
            tween.Start();

            return tween;
        }

        public static Tween DoTweenPositionNonAlloc(this RectTransform transform, Vector3 to, float duration, Tween tween)
        {
            Vector3 from = transform.anchoredPosition3D;
            void update(float percentage) => transform.anchoredPosition3D = Vector3.LerpUnclamped(from, to, percentage);

            UpdateTweenData(tween, duration, update);
            tween.SetOnComplete(() => transform.anchoredPosition3D = to);
            tween.Start();

            return tween;
        }

        public static Tween CreateTweenPositionNonAlloc(this Transform transform, Vector3 to, float duration, Tween tween)
        {
            Vector3 from = transform.position;
            void update(float percentage) => transform.position = Vector3.LerpUnclamped(from, to, percentage);

            UpdateTweenData(tween, duration, update);
            tween.SetOnComplete(() => transform.position = to);

            return tween;
        }
        #endregion

        #region Rotations
        public static Tween DoTweenRotation(this Transform transform, Quaternion to, float duration)
        {
            Tween tween = CreateTweenRotation(transform, to, duration);
            tween.Start();

            return tween;
        }

        public static Tween CreateTweenRotation(this Transform transform, Quaternion to, float duration)
        {
            Tween tween = new();
            CreateTweenRotationNonAlloc(transform, to, duration, tween);

            return tween;
        }

        public static Tween DoTweenRotationNonAlloc(this Transform transform, Quaternion to, float duration, Tween tween)
        {
            CreateTweenRotationNonAlloc(transform, to, duration, tween);
            tween.Start();

            return tween;
        }

        public static Tween DoTweenRotationNonAlloc(this RectTransform transform, Quaternion to, float duration, Tween tween)
        {
            CreateTweenRotationNonAlloc(transform, to, duration, tween);
            tween.Start();

            return tween;
        }

        public static Tween CreateTweenRotationNonAlloc(this Transform transform, Quaternion to, float duration, Tween tween)
        {
            Quaternion from = transform.rotation;
            void update(float percentage) => transform.rotation = Quaternion.LerpUnclamped(from, to, percentage);

            UpdateTweenData(tween, duration, update);
            tween.SetOnComplete(() => transform.rotation = to);

            return tween;
        }

        public static Tween CreateTweenRotationNonAlloc(this RectTransform transform, Quaternion to, float duration, Tween tween)
        {
            Quaternion from = transform.localRotation;
            void update(float percentage) => transform.localRotation = Quaternion.LerpUnclamped(from, to, percentage);

            UpdateTweenData(tween, duration, update);
            tween.SetOnComplete(() => transform.localRotation = to);

            return tween;
        }
        #endregion

        #region Scales

        public static Tween DoTweenScale(this Transform transform, Vector3 to, float duration)
        {
            Tween tween = CreateTweenScale(transform, to, duration);
            tween.Start();

            return tween;
        }

        public static Tween CreateTweenScale(this Transform transform, Vector3 to, float duration)
        {
            Tween tween = new();
            CreateTweenScaleNonAlloc(transform, to, duration, tween);

            return tween;
        }

        public static Tween DoTweenScaleNonAlloc(this Transform transform, Vector3 to, float duration, Tween tween)
        {
            CreateTweenScaleNonAlloc(transform, to, duration, tween);
            tween.Start();

            return tween;
        }

        public static Tween CreateTweenScaleNonAlloc(this Transform transform, Vector3 to, float duration, Tween tween)
        {
            Vector3 from = transform.localScale;
            void update(float percentage) => transform.localScale = Vector3.LerpUnclamped(from, to, percentage);

            UpdateTweenData(tween, duration, update);
            tween.SetOnComplete(() => transform.localScale = to);

            return tween;
        }

        #endregion

        #endregion

        #region Custom
        public static Tween DoTweenCustom(Action<float> update, float duration)
        {
            Tween tween = new(update, duration);

            tween.Start();

            return tween;
        }

        public static Tween CreateTweenCustom(Action<float> update, float duration)
        {
            Tween tween = new(update, duration);

            return tween;
        }

        public static Tween DoTweenCustomNonAlloc(Action<float> update, float duration, Tween tween)
        {
            CreateTweenCustomNonAlloc(update, duration, tween);
            tween.Start();

            return tween;
        }

        public static Tween CreateTweenCustomNonAlloc(Action<float> update, float duration, Tween tween)
        {
            UpdateTweenData(tween, duration, update);

            return tween;
        }

        #endregion

        public static Tween Shake(this Transform transform, float amount, float duration, Tween tween)
        {
            Vector3 originalPosition = transform.position;
            void update(float percentage)
            {
                float magnitude = (1 - percentage) * amount;
                transform.position = originalPosition + (Vector3)UnityEngine.Random.insideUnitCircle * magnitude;
            }

            UpdateTweenData(tween, duration, update);
            tween.SetOnComplete(() => transform.position = originalPosition);
            tween.Start();

            return tween;
        }
        private static void UpdateTweenData(Tween tween, float duration, Action<float> update)
        {
            tween.Stop();
            tween.SetDuration(duration);
            tween.SetOnComplete(null);
            tween.SetEasingFunction(EasingFunctions.EasingFunction.LINEAR);
            tween.SetUpdate(update);
        }
    }
}