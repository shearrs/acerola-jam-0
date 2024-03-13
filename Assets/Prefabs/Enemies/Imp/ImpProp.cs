using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpProp : MonoBehaviour
{
    private Animator animator;
    private float animationDelay;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        animationDelay = Random.Range(0, 4.5f);

        animator.speed = 0;

        Invoke(nameof(ActivateAnimator), animationDelay);
    }

    private void ActivateAnimator()
    {
        animator.speed = 1;
    }
}
