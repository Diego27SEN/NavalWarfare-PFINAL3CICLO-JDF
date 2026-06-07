using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QueueTurn
{
    public LinkedList<PlayerGame> orderShifts = new LinkedList<PlayerGame>();

    public void AddShift(PlayerGame player)
    {
        orderShifts.AddLast(player);
    }

    public PlayerGame AdvanceShift()
    {
        if (orderShifts.Count == 0) return null;

        var currentPlayer = orderShifts.First.Value;
        orderShifts.RemoveFirst();
        orderShifts.AddLast(currentPlayer);

        return orderShifts.First.Value;
    }

    public PlayerGame GetCurrentPlayer()
    {
        return orderShifts.Count > 0 ? orderShifts.First.Value : null;
    }
}
