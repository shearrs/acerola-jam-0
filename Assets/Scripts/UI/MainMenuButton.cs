using System.Collections;
using System.Collections.Generic;
using Tweens;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : ToggleableButton
{
    protected override void OnClickedInternal()
    {
        SceneManager.LoadScene("GameScene");
    }
}