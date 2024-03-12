using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PetitionCancelButton : ToggleableButton
{
    private PetitionManager petitionManager;

    protected override void OnEnable()
    {
        base.OnEnable();

        petitionManager = PetitionManager.Instance;
    }

    public override void OnClickedInternal()
    {
        petitionManager.Selection.Disable();
    }
}