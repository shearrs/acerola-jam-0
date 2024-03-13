using System.Collections;
using TMPro;
using Tweens;
using UnityEngine;
using CustomUI;

public class StartGameButton : ToggleableButton
{
    [Header("Menu")]
    [SerializeField] private RectTransform mainMenu;
    [SerializeField] private TextMeshProUGUI titleCard;
    private readonly Tween buttonTween = new();
    private readonly Tween titleTween = new();

    [Header("Start Sequence")]
    [SerializeField] private Vector3 walkLocation;
    [SerializeField] private Vector3 walkRotation;

    protected override void OnClickedInternal()
    {
        titleCard.rectTransform.DoTweenScaleNonAlloc(TweenManager.TWEEN_ZERO, 0.45f, titleTween)
            .SetEasingFunction(EasingFunctions.EasingFunction.IN_BACK)
            .SetOnComplete(() => titleCard.gameObject.SetActive(false));

        Invoke(nameof(TweenButton), 0.15f);
    }

    private void TweenButton()
    {
        transform.DoTweenScaleNonAlloc(TweenManager.TWEEN_ZERO, 0.45f, buttonTween)
            .SetEasingFunction(EasingFunctions.EasingFunction.IN_BACK)
            .SetOnComplete(OnComplete);
    }

    private void OnComplete()
    {
        StartCoroutine(IEOnComplete());

        // rotate
        // move to first position
        // rotate
        // tween bars up
        // move
    }

    private IEnumerator IEOnComplete()
    {
        Player player = Level.Instance.Player;
        Transform playerTransform = player.transform;

        Vector3 direction = (walkLocation - playerTransform.position).normalized;
        Quaternion targetRotation1 = Quaternion.LookRotation(direction, Vector3.up);

        while (Quaternion.Angle(transform.rotation, targetRotation1) > 0.1f)
        {
            playerTransform.rotation = Quaternion.RotateTowards(playerTransform.rotation, targetRotation1, player.RotationSpeed * Time.deltaTime);

            yield return null;
        }

        Quaternion targetRotation2 = Quaternion.Euler(walkRotation);

        while ((walkLocation - transform.position).sqrMagnitude > 0.1)
        {
            Vector3 position = Vector3.MoveTowards(transform.position, walkLocation, player.MovementSpeed * Time.deltaTime);
            Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation2, player.RotationSpeed * Time.deltaTime);

            transform.SetPositionAndRotation(position, rotation);

            yield return null;
        }

        UIManager.Instance.ToggleSideBars(true, () => player.Move());
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(walkLocation, 0.5f);
    }
}