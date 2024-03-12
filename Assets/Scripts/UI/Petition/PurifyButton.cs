using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PurifyButton : ToggleableButton
{
    [SerializeField] private bool heal;
    private PetitionManager petitionManager;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (petitionManager == null)
            petitionManager = PetitionManager.Instance;
    }

    public override void OnClickedInternal()
    {
        petitionManager.SelectMenuOption(heal);
    }
}