using CustomUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetryButton : ToggleableButton
{
    protected override void OnClickedInternal()
    {
        Player player = Level.Instance.Player;
        player.Revive();
        Level.Instance.StartEncounter();
        UIManager.Instance.DeathMenu.Disable();
    }
}
