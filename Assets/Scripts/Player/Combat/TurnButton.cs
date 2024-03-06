using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnButton : MonoBehaviour
{
    [SerializeField] private Attack attack;

    public void SubmitTurn()
    {
        Player player = Level.Instance.Player;
    }
}