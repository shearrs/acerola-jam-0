using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CombatDropSelectable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private bool lot;
    [SerializeField] private RectTransform highlight;
    private bool selected = false;

    private void OnDisable()
    {
        selected = false;
    }

    private void Update()
    {
        if (selected)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (lot)
                {
                    AudioManager.Instance.LotSound();
                    CombatDropUI.Instance.LotSelected = true;
                }
                else
                {
                    AudioManager.Instance.HealthSound();
                    CombatDropUI.Instance.HealthSelected = true;
                }

                gameObject.SetActive(false);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (lot)
            AudioManager.Instance.HighlightSound();
        else
            AudioManager.Instance.HighlightSound(2);

        highlight.gameObject.SetActive(true);
        selected = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        highlight.gameObject.SetActive(false);
        selected = false;
    }
}
