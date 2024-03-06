using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomUI;
using UnityEngine.EventSystems;

public class Lot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float speed;
    [SerializeField] private float gravity;
    private LotsUI lotsUI;

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
        
        while (progress <= 1)
        {
            OrientedPoint sample = path.GetBezierPoint(progress);
            Vector3 position = sample.position;
            transform.position = position;
            transform.rotation = Quaternion.Euler(verticalRotations * progress, 0, horizontalRotations * progress);

            progress += Time.deltaTime * speed;

            yield return null;
        }

        if (notifyUI)
            lotsUI.SelectLots();
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
