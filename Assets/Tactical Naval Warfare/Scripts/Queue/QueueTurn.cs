using System.Collections.Generic;
using UnityEngine;

public class QueueTurn : MonoBehaviour
{
    public LinkedList<PlayerGame> orderTurns = new LinkedList<PlayerGame>();

    public void AddShift(PlayerGame jugador)
    {
        orderTurns.AddLast(jugador);
    }
}
