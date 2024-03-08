using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DefenseDisplay : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI text;

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
            image.gameObject.SetActive(true);
            text.text = "x" + defense;
        }
    }
}