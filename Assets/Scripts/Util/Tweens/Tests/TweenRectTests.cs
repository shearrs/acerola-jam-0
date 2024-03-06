using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tweens
{
    public class TweenRectTests : MonoBehaviour
    {
        [SerializeField] private Image menu;
        [SerializeField] private Tween tween;

        public void OpenMenu()
        {
            menu.gameObject.SetActive(true);
            menu.rectTransform.CreateTweenScaleNonAlloc(Vector3.one, tween.Duration, tween).Start();
        }
    }
}