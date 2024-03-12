using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Sin", menuName = "Turn/Sin")]
public class SinAction : TurnAction
{
    [SerializeField] private SinType[] sinTypes;
    [SerializeField] private bool random;

    protected override void PerformInternal(Turn turn)
    {
        if (!random)
        {
            for (int i = 0; i < sinTypes.Length; i++)
            {
                Level.Instance.Player.AddSin(Sin.GetSinForType(sinTypes[i]));
                Debug.Log(turn.User.Name + " inflicted " + turn.Target.Name + " with " + sinTypes[i]);
            }
        }
        else
        {
            Sin sin = Sin.GetSinForType(CombatManager.Instance.LotsBox.GetRandomSin());
            Level.Instance.Player.AddSin(sin);
        }
    }
}
