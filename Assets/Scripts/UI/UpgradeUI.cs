using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweens;
using CustomUI;
using TMPro;

public class UpgradeUI : Singleton<UpgradeUI>
{
    [SerializeField] private TextMeshProUGUI title;
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
            CombatManager.Instance.LotsBox.gameObject.SetActive(true);
            upgradeButton1.gameObject.SetActive(true);
            title.gameObject.SetActive(true);

            if (HasTwoOptions)
                upgradeButton2.gameObject.SetActive(true);

            // if player has greed, disable one of the buttons
            // tell the button to do its thang
            if (Level.Instance.Player.HasSin(SinType.GREED) && HasTwoOptions)
            {
                SinUI.Instance.ActivateUI(SinType.GREED);

                int random = Random.Range(0, 2);

                if (random == 0)
                {
                    upgradeButton1.Enable();
                    upgradeButton2.Disable(true);
                }
                else
                {
                    upgradeButton1.Disable(true);
                    upgradeButton2.Enable();
                }
            }
            else
            {
                upgradeButton1.Enable();

                if (HasTwoOptions)
                    upgradeButton2.Enable();
            }

            if (blessed)
            {
                blessedText.gameObject.SetActive(true);
                blessedText.rectTransform.localScale = TweenManager.TWEEN_ZERO;
                blessedText.rectTransform.DoTweenScaleNonAlloc(Vector3.one, 0.1f, tween2).SetEasingFunction(EasingFunctions.EasingFunction.OUT_BACK);
            }
        }

        UIManager uiManager = UIManager.Instance;

        if (!blessed)
            blessedText.gameObject.SetActive(false);

        container.gameObject.SetActive(true);
        container.localScale = TweenManager.TWEEN_ZERO;
        container.DoTweenScaleNonAlloc(Vector3.one, uiManager.DefaultUIData.MoveDuration, tween1).SetOnComplete(onComplete).SetEasingFunction(EasingFunctions.EasingFunction.OUT_BACK);
        uiManager.ToggleBar(true);
    }

    public void Disable()
    {
        UIManager uiManager = UIManager.Instance;

        CombatManager.Instance.LotsBox.gameObject.SetActive(false);
        upgradeButton1.gameObject.SetActive(false);
        upgradeButton2.gameObject.SetActive(false);
        container.DoTweenScaleNonAlloc(TweenManager.TWEEN_ZERO, 0.5f, tween1).SetOnComplete(() => container.gameObject.SetActive(false)).SetEasingFunction(EasingFunctions.EasingFunction.IN_BACK);
        uiManager.ToggleBar(false, () => Level.Instance.EndEncounter());
    }
}