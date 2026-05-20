using System.Collections.Generic;
using UnityEngine;

public class QueueTurn : MonoBehaviour
{
    public LinkedList<PlayerGame> ordenTurnos = new LinkedList<PlayerGame>();

    public void AddShift(PlayerGame jugador)
    {
        ordenTurnos.AddLast(jugador);
    }
}
