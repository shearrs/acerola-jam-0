using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandUI : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Sprite openHand;
    [SerializeField] private Sprite closedHand;
    [SerializeField] private Vector2 handOffset;
    private RectTransform rect;
    private Image image;
    private Vector2 previousPosition = Vector2.zero;
    
    public Vector2 Velocity { get; private set; }
    private Vector2 MousePosition => ((Vector2)Input.mousePosition / canvas.scaleFactor) + handOffset;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        Cursor.visible = true;
    }

    private void Update()
    {
        FollowMouse();
    }

    public void Open()
    {
        image.sprite = openHand;
    }

    public void Close()
    {
        image.sprite = closedHand;
    }

    private void FollowMouse()
    {
        Vector2 mousePosition = MousePosition;
        rect.anchoredPosition = mousePosition;

        Velocity = mousePosition - previousPosition;
        previousPosition = mousePosition;
    }
}