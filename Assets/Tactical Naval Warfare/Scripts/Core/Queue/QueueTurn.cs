using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QueueTurn
{
    public LinkedList<PlayerGameDatabase> orderShifts = new LinkedList<PlayerGameDatabase>();

    public void AddShift(PlayerGameDatabase player)
    {
        orderShifts.AddLast(player);
    }

    public PlayerGameDatabase AdvanceShift()
    {
        if (orderShifts.Count == 0) return null;

        var currentPlayer = orderShifts.First.Value;
        orderShifts.RemoveFirst();
        orderShifts.AddLast(currentPlayer);

        return orderShifts.First.Value;
    }

    public PlayerGameDatabase GetCurrentPlayer()
    {
        return orderShifts.Count > 0 ? orderShifts.First.Value : null;
    }
}
