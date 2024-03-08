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
    private CombatManager combatManager;

    [Header("Type Colors")]
    [SerializeField] private Color damageColor;
    [SerializeField] private Color protectionColor;
    [SerializeField] private Color holyColor;
    [SerializeField] private Color temptationColor;
    private int previousChildIndex;

    private bool isKept = false;
    private bool keptHighlightPrevent = false;
    public bool IsKept 
    { 
        get => isKept; 
        set
        {
            isKept = value;
            if (isKept)
                keptHighlightPrevent = true;
        }
    }

    public bool IsLocked { get; set; }
    public Vector3 OriginalPosition { get; set; }
    public Quaternion OriginalRotation { get; set; }
    public LotType Type { get; private set; }

    private void Start()
    {
        combatManager = CombatManager.Instance;
    }

    public void Throw(Spline path, bool notifyUI = false)
    {
        lotImage.color = Color.white;
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

            transform.position = position;
            transform.localRotation = Quaternion.Euler(verticalRotations * progress, 0, horizontalRotations * progress);

            progress += Time.deltaTime * speed;

            yield return null;
        }

        Vector3 finalPosition = path.GetBezierPoint(1).position;
        transform.position = finalPosition;
        transform.localRotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);

        if (onComplete)
            combatManager.SelectLots();
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

        lotImage.color = color;
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
        {
            previousChildIndex = transform.GetSiblingIndex();
            alpha = 255;
            BringToFront();
        }
        else
        {
            alpha = 0;

            if (!keptHighlightPrevent)
                transform.SetSiblingIndex(previousChildIndex);
            else
                keptHighlightPrevent = false;
        }

        highlightImage.color = new(highlightColor.r, highlightColor.g, highlightColor.b, alpha);
    }

    public void BringToFront()
    {
        transform.SetSiblingIndex(transform.parent.childCount - 1);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (IsLocked) 
            return;

        combatManager.HoveredLot = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (combatManager.HoveredLot == this)
            combatManager.HoveredLot = null;
    }
}
