using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweens;
using CustomUI;
using TMPro;

public class UpgradeUI : Singleton<UpgradeUI>
{
    [SerializeField] private RectTransform container;
    [SerializeField] private UpgradeButton upgradeButton1;
    [SerializeField] private UpgradeButton upgradeButton2;
    [SerializeField] private TextMeshProUGUI blessedText;
    private readonly Tween tween1 = new();
    private readonly Tween tween2 = new();

    public UpgradeButton UpgradeButton1 => upgradeButton1;
    public UpgradeButton UpgradeButton2 => upgradeButton2;
    public bool HasTwoOptions { get; set; } = false;

    public void Enable(bool blessed)
    {
        void onComplete()
        {
            upgradeButton1.gameObject.SetActive(true);
            upgradeButton2.gameObject.SetActive(true);

            if (blessed)
            {
                blessedText.gameObject.SetActive(true);
                blessedText.rectTransform.localScale = Vector3.zero;
                blessedText.rectTransform.DoTweenScaleNonAlloc(Vector3.one, 0.3f, tween2).SetEasingFunction(EasingFunctions.EasingFunction.OUT_BACK);
            }
        }

        UIManager uiManager = UIManager.Instance;

        if (!blessed)
            blessedText.gameObject.SetActive(false);

        container.gameObject.SetActive(true);
        container.localScale = Vector3.zero;
        container.DoTweenScaleNonAlloc(Vector3.one, uiManager.DefaultUIData.MoveDuration, tween1).SetOnComplete(onComplete).SetEasingFunction(EasingFunctions.EasingFunction.OUT_BACK);
        uiManager.ToggleBar(true);
    }

    public void Disable()
    {
        UIManager uiManager = UIManager.Instance;

        upgradeButton1.gameObject.SetActive(false);
        upgradeButton2.gameObject.SetActive(false);
        container.DoTweenScaleNonAlloc(Vector3.zero, 0.5f, tween1).SetOnComplete(() => container.gameObject.SetActive(false)).SetEasingFunction(EasingFunctions.EasingFunction.IN_BACK);
        uiManager.ToggleBar(false, () => Level.Instance.EndEncounter());
    }
}