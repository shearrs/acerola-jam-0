using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DefenseDisplay : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI text;

    public void UpdateDefense(int defense)
    {
        if (defense == 0 && CombatManager.Instance.Phase != CombatPhase.TURN)
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