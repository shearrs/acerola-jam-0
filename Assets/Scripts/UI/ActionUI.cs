using CustomUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActionUI
{
    [Header("Images")]
    [SerializeField] private RectTransform combatImages;

    [Header("Image Groups")]
    [SerializeField] private List<PlayerActionButton> playerActions;
    [SerializeField] private CombatSelectionButton previousButton;
    [SerializeField] private CombatSelectionButton nextButton;
    [SerializeField] private PlayerHealthbar healthbar;

    [Header("Pointer")]
    [SerializeField] private EnemyPointer enemyPointerPrefab;

    private UIManager uiManager;
    private RectTransform portrait;
    private EnemyPointer enemyPointer;

    private Battle battle;
    private Player player;

    public EnemyPointer Pointer => enemyPointer;

    public void Initialize()
    {
        uiManager = UIManager.Instance;
        player = Level.Instance.Player;

        enemyPointer = Object.Instantiate(enemyPointerPrefab, UIManager.Instance.transform);
        enemyPointer.gameObject.SetActive(false);

        portrait = uiManager.Portrait.GetComponent<RectTransform>();
        healthbar.GenerateHearts();
    }

    public void Enable()
    {
        battle = player.Battle;

        enemyPointer.StartCombat(battle);

        combatImages.gameObject.SetActive(true);
        ToggleSelectionButtons(true);
        UpdateSelectionButtons();
        SetActions(true);
        enemyPointer.Enable();
    }

    public void Disable()
    {
        enemyPointer.CombatEnded();

        combatImages.gameObject.SetActive(false);
    }

    public void SetActions(bool enabled)
    {
        if (enabled)
        {
            foreach (PlayerActionButton action in playerActions)
                action.Enable();

            UpdateSelectionButtons();
            enemyPointer.Enable();
        }
        else
        {
            foreach(PlayerActionButton action in playerActions)
                action.Disable();

            nextButton.Disable();
            previousButton.Disable();
            enemyPointer.Disable();
        }
    }

    public void UpgradeActionButton(int index)
    {
        playerActions[index].Upgrade();
    }

    public void OnPlayerHealthChanged(int change)
    {
        if (change < 0)
            uiManager.Shake(portrait);

        healthbar.UpdateHealth();
    }

    public void UpdateEnemyPointer(int change)
    {
        player.EnemyIndex += change;
        UpdateSelectionButtons();
        enemyPointer.SetPosition();
    }

    public void ToggleSelectionButtons(bool active)
    {
        nextButton.gameObject.SetActive(active);
        previousButton.gameObject.SetActive(active);
    }

    private void UpdateSelectionButtons()
    {
        if (battle.Enemies.Count <= 1)
        {
            nextButton.Disable();
            previousButton.Disable();
            return;
        }

        if (player.EnemyIndex >= battle.Enemies.Count)
            player.EnemyIndex = battle.Enemies.Count - 1;

        if (player.EnemyIndex + 1 >= battle.Enemies.Count)
            nextButton.Disable();
        else
            nextButton.Enable();

        if (player.EnemyIndex - 1 < 0)
            previousButton.Disable();
        else
            previousButton.Enable();
    }
}
