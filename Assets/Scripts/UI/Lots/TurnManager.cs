using CustomUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TurnManager
{
    private ActionUI actionUI;
    private bool enabled = false;

    public void Initialize()
    {
        actionUI = UIManager.Instance.ActionUI;
    }

    public void EnterPhase()
    {
        enabled = true;
        actionUI.SetActions(false);
    }

    public void ExitPhase()
    {
        if (!enabled)
            return;

        Debug.Log("exiting turn phase");

        actionUI.Disable();
        enabled = false;
    }
}