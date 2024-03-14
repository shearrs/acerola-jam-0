using System.Collections;
using System.Collections.Generic;
using Tweens;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : ToggleableButton
{
    [SerializeField] private Scene gameScene;
    private readonly Tween tween = new();

    public override void Enable()
    {
        base.Enable();

        transform.localScale = TweenManager.TWEEN_ZERO;
        transform.DoTweenScaleNonAlloc(Vector3.one, 1f, tween);
    }

    protected override void OnClickedInternal()
    {
        SceneManager.LoadScene(gameScene.buildIndex);
    }
}