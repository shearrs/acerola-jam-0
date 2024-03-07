using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomUI;

public class Level : Singleton<Level>
{
    [SerializeField] private Player player;
    [SerializeField] private List<Encounter> encounters;

    public Encounter CurrentEncounter => encounters[0];
    public Player Player => player;

    public void Start()
    {
        player.Move();
    }

    public void StartEncounter()
    {
        UIManager.Instance.EnterEncounter();
        CurrentEncounter.Enter();
    }

    public void EndEncounter()
    {
        UIManager.Instance.EndEncounter();
        encounters.RemoveAt(0);

        if (!Player.IsDead)
            Player.Move();
    }
}