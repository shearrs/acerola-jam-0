using CustomUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CombatPhase { LOTS, ACTION, TURN };

public class CombatManager : Singleton<CombatManager>
{
    [SerializeField] private RectTransform startButton;

    [Header("Managers")]
    [SerializeField] private LotsManager lotsManager;
    [SerializeField] private PlayerActionManager actionManager;
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private LotsBox lotsBox;
    [SerializeField] private DefenseDisplay defenseDisplay;

    [Header("Combat Settings")]
    [SerializeField] private float timeBetweenTurns;

    private UIManager uiManager;

    public float TimeBetweenTurns => timeBetweenTurns;
    public int Roll => lotsManager.Roll;
    public Lot HoveredLot { get => lotsManager.HoveredLot; set => lotsManager.HoveredLot = value; }
    public LotsBox LotsBox => lotsBox;
    public DefenseDisplay DefenseDisplay => defenseDisplay;
    public CombatPhase Phase { get; private set; }

    private void Start()
    {
        uiManager = UIManager.Instance;

        lotsManager.Initialize();
        actionManager.Initialize();
        turnManager.Initialize();
    }

    public void Enable()
    {
        void onComplete()
        {
            startButton.gameObject.SetActive(true);
            lotsBox.gameObject.SetActive(true);
            lotsManager.LotsParent.gameObject.SetActive(true);
        }

        uiManager.ToggleBar(true, onComplete, true);
    }

    public void Disable()
    {
        uiManager.ToggleBar(false, null, true);

        lotsManager.ExitPhase();
        actionManager.ExitPhase();
        turnManager.ExitPhase();
        lotsBox.Empty();
        lotsBox.gameObject.SetActive(false);
        lotsManager.LotsParent.gameObject.SetActive(false);
        DefenseDisplay.IsEnabled = false;
        DefenseDisplay.UpdateDefense(0);
    }

    public void EnterPhase(CombatPhase phase)
    {
        Phase = phase;
        Debug.Log("enter phase: " + phase);

        switch (phase)
        {
            case CombatPhase.LOTS:
                LotsPhase();
                break;
            case CombatPhase.ACTION:
                ActionPhase();
                break;
            case CombatPhase.TURN:
                TurnPhase();
                break;
        }
    }

    private void LotsPhase()
    {
        turnManager.ExitPhase();
        lotsManager.EnterPhase();
    }

    private void ActionPhase()
    {
        lotsManager.ExitPhase();
        actionManager.EnterPhase();
    }

    private void TurnPhase()
    {
        actionManager.ExitPhase();
        turnManager.EnterPhase();
    }

    public void ConfirmLots() => lotsManager.ConfirmLots();
    public void ThrowLots() => lotsManager.ThrowLots();
    public void SetLotsActive(bool active) => lotsManager.SetLotsActive(active);
    public void SelectLots() => lotsManager.SelectLots();
    public void RetireLot(Lot lot) => lotsManager.RetireLot(lot);

    public void ChooseTurn(PlayerTurnType type) => actionManager.ChooseTurn(type);
}