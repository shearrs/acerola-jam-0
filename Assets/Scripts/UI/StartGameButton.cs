using System.Collections;
using TMPro;
using Tweens;
using UnityEngine;
using CustomUI;

public class StartGameButton : ToggleableButton
{
    [Header("Menu")]
    [SerializeField] private RectTransform mainMenu;
    [SerializeField] private RectTransform titleCard;
    private readonly Tween buttonTween = new();
    private readonly Tween titleTween = new();

    [Header("Start Sequence")]
    [SerializeField] private Vector3 walkLocation;
    [SerializeField] private float rotationTime;

    private readonly int isWalkingID = PlayerAnimationData.IsWalking;

    protected override void OnClickedInternal()
    {
        titleCard.DoTweenScaleNonAlloc(TweenManager.TWEEN_ZERO, 0.45f, titleTween)
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
    }

    private IEnumerator IEOnComplete()
    {
        Player player = Level.Instance.Player;
        Transform playerTransform = player.transform;

        Vector3 direction = (walkLocation - playerTransform.position).normalized;
        Path firstPath = Level.Instance.CurrentEncounter.Path;
        Vector3 rotationDirection = (firstPath.GetPosition(1) - firstPath.GetPosition(0)).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(rotationDirection, Vector3.up);
        targetRotation.z = 0;

        player.Animator.SetBool(isWalkingID, true);
        float walkSpeed = player.MovementSpeed * 0.5f;
        float elapsedTime = 0;
        float timeToRotate = rotationTime * 3;

        Vector3 currentVelocity = Vector3.zero;
        UIManager.Instance.ToggleSideBars(true);

        while ((walkLocation - playerTransform.position).sqrMagnitude > 0.1)
        {
            Vector3 position = Vector3.MoveTowards(playerTransform.position, walkLocation, walkSpeed * Time.deltaTime);

            if (elapsedTime < timeToRotate)
            {
                playerTransform.rotation = ExtensionMethods.SmoothDampQuaternion(playerTransform.rotation, targetRotation, ref currentVelocity, rotationTime);
                elapsedTime += Time.deltaTime;
            }

            playerTransform.position = position;

            yield return null;
        }

        player.Animator.SetBool(isWalkingID, false);

        while (elapsedTime < timeToRotate)
        {
            Quaternion rotation = ExtensionMethods.SmoothDampQuaternion(playerTransform.rotation, targetRotation, ref currentVelocity, rotationTime);
            playerTransform.rotation = rotation;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        UIManager.Instance.Portrait.Enable();
        player.Move();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(walkLocation, 0.5f);
    }
}