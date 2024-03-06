using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tweens
{
    public static class EasingFunctions
    {
        public enum EasingFunction
        {
            LINEAR,
            IN_BACK,
            OUT_BACK,
            IN_OUT_BACK,
            IN_BOUNCE,
            OUT_BOUNCE,
            IN_OUT_BOUNCE
        }

        public static float EasePercentage(EasingFunction ease, float p)
        {
            return ease switch
            {
                EasingFunction.LINEAR => p,
                EasingFunction.IN_BACK => InBack(p),
                EasingFunction.OUT_BACK => OutBack(p),
                EasingFunction.IN_OUT_BACK => InOutBack(p),
                EasingFunction.IN_BOUNCE => InBounce(p),
                EasingFunction.OUT_BOUNCE => OutBounce(p),
                EasingFunction.IN_OUT_BOUNCE => InOutBounce(p),
                _ => p,
            };
        }

        private static float InBack(float p)
        {
            const float c1 = 1.70158f;
            const float c3 = c1 + 1;

            return (c3 * p * p * p) - (c1 * p * p);
        }

        private static float OutBack(float p)
        {
            const float c1 = 1.70158f;
            const float c3 = c1 + 1;

            return 1 + c3 * Mathf.Pow(p - 1, 3) + c1 * Mathf.Pow(p - 1, 2);
        }

        private static float InOutBack(float p)
        {
            const float c1 = 1.70158f;
            const float c2 = c1 * 1.525f;

            return (p < 0.5)
                ? (Mathf.Pow(2 * p, 2) * ((c2 + 1) * 2 * p - c2)) / 2
                : (Mathf.Pow(2 * p - 2, 2) * ((c2 + 1) * (p * 2 - 2) + c2) + 2) / 2;
        }

        private static float OutBounce(float p)
        {
            const float n1 = 7.5625f;
            const float d1 = 2.75f;
            float scale1;
            float scale2;

            if (p < 1 / d1)
            {
                scale1 = 0;
                scale2 = 0;
            }
            else if (p < 2 / d1)
            {
                scale1 = 1.5f;
                scale2 = 0.75f;
            }
            else if (p < 2.5f / d1)
            {
                scale1 = 2.25f;
                scale2 = 0.9375f;
            }
            else
            {
                scale1 = 2.625f;
                scale2 = 0.984375f;
            }

            return n1 * (p -= scale1 / d1) * p + scale2;
        }

        private static float InBounce(float p)
        {
            return 1 - OutBounce(1 - p);
        }

        private static float InOutBounce(float p)
        {
            return (p < 0.5)
                ? (1 - OutBounce(1 - 2 * p)) / 2
                : (1 + OutBounce(2 * p - 1)) / 2;
        }
    }
}