using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Tweens;

public class DefenseDisplay : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float growDuration;
    private readonly Tween tween = new(); 

    public bool IsEnabled { get; set; } = false;

    public void UpdateDefense(int defense)
    {
        if (!IsEnabled)
        {
            image.gameObject.SetActive(false);
            text.text = "";
        }
        else
        {
            if (!image.gameObject.activeSelf)
            {
                image.gameObject.SetActive(true);
                image.rectTransform.localScale = TweenManager.TWEEN_ZERO;
                image.rectTransform.DoTweenScaleNonAlloc(Vector3.one, growDuration, tween);
            }
            
            text.text = "x" + defense;
        }
    }
}