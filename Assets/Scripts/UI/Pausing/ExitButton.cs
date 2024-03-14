using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweens;

public class ExitButton : ToggleableButton
{
    protected override void OnClickedInternal()
    {
        Application.Quit();
    }
}
