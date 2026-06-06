using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "TurnSystemSO", menuName = "TacticalNavalWarfare/TurnSystemSO")]
public class TurnSystemData : SerializedScriptableObject
{
    [ShowInInspector]
    public QueueTurn SistemShift = new QueueTurn();

    public void Initialize()
    {
        if (SistemShift.orderShifts != null)
        {
            SistemShift.orderShifts.Clear();
        }
    }
}
