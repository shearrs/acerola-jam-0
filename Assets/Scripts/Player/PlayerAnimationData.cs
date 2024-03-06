using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerAnimationData
{
    public static int IsWalking => Animator.StringToHash("isWalking");
    public static int Attack => Animator.StringToHash("attack");
}