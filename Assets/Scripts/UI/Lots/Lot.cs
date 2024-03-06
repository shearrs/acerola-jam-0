using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Tweens;

public enum LotType { DAMAGE, PROTECTION, HOLY, TEMPTATION };

public class Lot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image lotImage;
    [SerializeField] private Image highlightImage;
    private LotsManager lotsManager;

    [Header("Type Colors")]
    [SerializeField] private Color damageColor;
    [SerializeField] private Color protectionColor;
    [SerializeField] private Color holyColor;
    [SerializeField] private Color temptationColor;
    [SerializeField] private Tween colorTween;
    private Color defaultColor;

    public bool IsKept { get; set; }
    public Vector3 OriginalPosition { get; set; }
    public Quaternion OriginalRotation { get; set; }
    public LotType Type { get; private set; }

    private void Start()
    {
        lotsManager = LotsManager.Instance;
        defaultColor = lotImage.color;
    }

    public void Throw(Spline path, bool notifyUI = false)
    {
        StartCoroutine(IEThrow(path, notifyUI));
    }

    private IEnumerator IEThrow(Spline path, bool onComplete)
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

        if (onComplete)
            lotsManager.SelectLots();
    }

    public void RandomizeType()
    {
        int typeIndex = Random.Range(0, 9);

        Type = GetTypeForIndex(typeIndex);
        SetColor();
    }

    private LotType GetTypeForIndex(int index)
    {
        if (index == 0)
            return LotType.TEMPTATION;
        else if (index == 7)
            return LotType.HOLY;
        else if (index < 5)
            return LotType.DAMAGE;
        else if (index <= 8)
            return LotType.PROTECTION;
        else
            return LotType.DAMAGE;
    }

    private void SetColor()
    {
        Color color = GetColorForType(Type);

        void updateColor(float percentage)
        {
            lotImage.color = Color.Lerp(defaultColor, color, percentage);
        }

        TweenManager.DoTweenCustomNonAlloc(updateColor, colorTween.Duration, colorTween);
    }

    private Color GetColorForType(LotType type)
    {
        return type switch
        {
            LotType.DAMAGE => damageColor,
            LotType.PROTECTION => protectionColor,
            LotType.HOLY => holyColor,
            LotType.TEMPTATION => temptationColor,
            _ => damageColor,
        };
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
        lotsManager.HoveredLot = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (lotsManager.HoveredLot == this)
            lotsManager.HoveredLot = null;
    }
}
