using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomUI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Lot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image lotImage;
    [SerializeField] private Image highlightImage;
    private LotsUI lotsUI;

    public bool IsKept { get; set; }
    public Vector3 OriginalPosition { get; set; }
    public Quaternion OriginalRotation { get; set; }

    private void Awake()
    {
        lotsUI = UIManager.Instance.LotsUI;
    }

    public void Throw(Spline path, bool notifyUI = false)
    {
        StartCoroutine(IEThrow(path, notifyUI));
    }

    private IEnumerator IEThrow(Spline path, bool notifyUI)
    {
        float progress = 0;
        float verticalRotations = Random.Range(1, 5) * 360;
        float horizontalRotations = Random.Range(1, 3) * 180;
        float speed = Random.Range(1.5f, 2.25f);
        
        while (progress <= 1)
        {
            OrientedPoint sample = path.GetBezierPoint(progress);
            Vector3 position = sample.position;

            transform.SetPositionAndRotation
                (
                position, 
                Quaternion.Euler(verticalRotations * progress, 0, horizontalRotations * progress)
                );

            progress += Time.deltaTime * speed;

            yield return null;
        }

        Vector3 finalPosition = path.GetBezierPoint(1).position;
        transform.SetPositionAndRotation
            (
                finalPosition,
                Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z)
            );

        if (notifyUI)
            lotsUI.SelectLots();
    }

    public void SetColor(Color color)
    {
        lotImage.color = color;
    }

    public void Highlight(bool enable)
    {
        Color highlightColor = highlightImage.color;
        float alpha;

        if (enable)
            alpha = 255;
        else
            alpha = 0;

        highlightImage.color = new(highlightColor.r, highlightColor.g, highlightColor.b, alpha);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        lotsUI.HoveredLot = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (lotsUI.HoveredLot == this)
            lotsUI.HoveredLot = null;
    }
}
