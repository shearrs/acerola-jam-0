using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Sin", menuName = "Turn/Sin")]
public class SinAction : TurnAction
{
    [SerializeField] private SinType[] sinTypes;

    protected override void PerformInternal(Turn turn)
    {
        for (int i = 0; i < sinTypes.Length; i++)
        {
            Level.Instance.Player.AddSin(Sin.GetSinForType(sinTypes[i]));
            Debug.Log(turn.User.Name + " inflicted " + turn.Target.Name + " with " + sinTypes[i]);
        }
    }
}
