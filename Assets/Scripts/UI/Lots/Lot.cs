using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using CustomUI;

public enum LotType { DAMAGE, PROTECTION, HOLY, TEMPTATION };

public class Lot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Images")]
    [SerializeField] private Image lotImage;
    [SerializeField] private Image highlightImage;
    [SerializeField] private Image shadowImage;
    [SerializeField] private float startingShadowDistance;
    private CombatManager combatManager;

    [Header("Type Colors")]
    [SerializeField] private Color damageColor;
    [SerializeField] private Color protectionColor;
    [SerializeField] private Color holyColor;
    [SerializeField] private Color temptationColor;
    private int previousChildIndex;

    [Header("Audio")]
    [SerializeField] private AudioClip collisionSound;
    [SerializeField] private AudioSource audioSource;

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

    private void OnDisable()
    {
        shadowImage.gameObject.SetActive(false);
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

        shadowImage.gameObject.SetActive(true);

        while (progress <= 1)
        {
            OrientedPoint sample = path.GetBezierPoint(progress);
            Vector3 position = sample.position;

            transform.position = position;
            transform.localRotation = Quaternion.Euler(verticalRotations * progress, 0, horizontalRotations * progress);

            RectTransform shadowRect = shadowImage.rectTransform;

            shadowRect.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * 1.5f, progress);
            float scaleFactor = UIManager.Instance.Canvas.transform.localScale.x;
            float distance = (1 - progress) * (startingShadowDistance * scaleFactor);
            shadowRect.position = transform.position + distance * Camera.main.transform.forward;

            progress += Time.deltaTime * speed;

            yield return null;
        }

        Invoke(nameof(PlayCollisionSound), Random.Range(0, 0.1f));

        Vector3 finalPosition = path.GetBezierPoint(1).position;
        transform.position = finalPosition;
        transform.localRotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);
        shadowImage.gameObject.SetActive(false);

        if (onComplete)
            combatManager.SelectLots();
    }

    private void PlayCollisionSound()
    {
        if (audioSource.clip != collisionSound)
            audioSource.clip = collisionSound;

        AudioManager.RandomizePitch(audioSource, 1.15f, 1.5f);
        audioSource.Play();
    }

    public void RandomizeType()
    {
        int lowRange;

        if (Level.Instance.Player.HasSin(SinType.ENVY))
            lowRange = -2;
        else
            lowRange = 0;

        int typeIndex = Random.Range(lowRange, 9);

        Type = GetTypeForIndex(typeIndex);
        SetColor();
    }

    private LotType GetTypeForIndex(int index)
    {
        if (index < 0)
            SinUI.Instance.ActivateUI(SinType.ENVY);

        if (index <= 0)
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
            AudioManager.Instance.HighlightSound();

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
