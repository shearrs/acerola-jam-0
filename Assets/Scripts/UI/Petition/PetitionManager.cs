using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweens;

public class PetitionManager : Singleton<PetitionManager>
{
    [SerializeField] private PurifyMenu purifyMenu;
    [SerializeField] private PetitionSelection selectionMenu;

    public bool IsEnabled { get; private set; }
    public LotsBox LotsBox { get; private set; }

    public PetitionSelection Selection => selectionMenu;
    public PurifyMenu PurifyMenu => purifyMenu;

    private void Start()
    {
        LotsBox = CombatManager.Instance.LotsBox;
    }

    public void Enable()
    {
        // enable petition selection
        // use actionUI to disable actions
        selectionMenu.Enable();

        IsEnabled = true;
    }

    public void Disable()
    {
        IsEnabled = false;
    }

    public void SelectMenuOption(bool heal)
    {
        selectionMenu.Disable();

        if (heal)
        {
            Level.Instance.Player.SelectedHeal = LotsBox.ReleaseLotsOfType(LotType.HOLY).Count;
            Disable();
        }
        else
        {
            purifyMenu.Enable();
        }
    }
}
