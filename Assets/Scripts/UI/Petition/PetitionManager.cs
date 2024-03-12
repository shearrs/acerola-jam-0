using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweens;

public class PetitionManager : Singleton<PetitionManager>
{
    [SerializeField] private PurifyMenu purifyMenu;
    [SerializeField] private PetitionSelection selectionMenu;

    public LotsBox LotsBox { get; private set; }

    public PetitionSelection Selection => selectionMenu;
    public PurifyMenu PurifyMenu => purifyMenu;

    private void Start()
    {
        LotsBox = CombatManager.Instance.LotsBox;
    }

    public void SelectMenuOption(bool heal)
    {
        selectionMenu.Disable();
        Player player = Level.Instance.Player;

        if (heal)
        {
            player.SelectedHeal = LotsBox.ReleaseLotsOfType(LotType.HOLY).Count;
        }
        else
        {
            player.PurifyingSin = true;
        }

        CombatManager.Instance.ActionManager.Petition();
    }
}
