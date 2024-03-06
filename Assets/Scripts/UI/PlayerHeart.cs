using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeart : MonoBehaviour
{
    private RectTransform rect;

    public RectTransform RectTransform
    {
        get
        {
            if (rect == null)
                rect = GetComponent<RectTransform>();

            return rect;
        }
    }
}
