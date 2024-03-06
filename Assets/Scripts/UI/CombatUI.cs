using CustomUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CombatUI
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

    [Header("Settings")]
    [SerializeField] private float timeBetweenTurns;
    private Battle battle;
    private Player player;

    public float TimeBetweenTurns => timeBetweenTurns;
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

    public void StartEncounter(CombatEncounter encounter)
    {
        battle = encounter.Battle;
        uiManager.ToggleBar(true, InitializeUI);
    }

    public void EndEncounter()
    {
        CombatSelectionUI(false);
        uiManager.ToggleBar(false);
    }

    private void InitializeUI()
    {
        enemyPointer.StartCombat(battle);
        CombatSelectionUI(true);
    }

    public void CombatSelectionUI(bool open)
    {
        if (open)
        {
            combatImages.gameObject.SetActive(true);

            UpdateSelectionButtons();
            SetActions(true);
            enemyPointer.Enable();
        }
        else
        {
            combatImages.gameObject.SetActive(false);

            enemyPointer.CombatEnded();
        }
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

    private void UpdateSelectionButtons()
    {
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
