using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "TurnSystemSO", menuName = "NavalWarfare/TurnSystemSO")]
public class TurnSystemSO : ScriptableObject
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
