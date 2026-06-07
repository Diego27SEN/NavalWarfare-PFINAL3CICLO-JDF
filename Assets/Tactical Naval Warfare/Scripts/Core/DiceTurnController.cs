using UnityEngine;

public class DiceTurnController
{
    private TurnSystemData turnData;

    public PlayerGame currentPlayer;
    public int remainingShots = 0;
    public bool hasRolledDice = false;

    public DiceTurnController(TurnSystemData data)
    {
        turnData = data;
    }

    public void SetInitialPlayer()
    {
        if (currentPlayer == null && turnData != null)
        {
            currentPlayer = turnData.SistemShift.GetCurrentPlayer();
        }
    }
    public void RollDice()
    {
        if (currentPlayer == null) return;

        hasRolledDice = true;
        int diceResult = Random.Range(1, 9);
        Debug.Log($"El Dado {currentPlayer.PlayerID} obtuvo un: {diceResult}");

        if (diceResult == 1 || diceResult == 3 || diceResult == 5)
        {
            remainingShots = 1;
        }
        else if (diceResult == 4 || diceResult == 6)
        {
            remainingShots = 2;
        }
        else
        {
            remainingShots = 0;
            Debug.Log($"{currentPlayer.PlayerID} obtuvo una CARTA. Fin de fase de disparo.");
            EndTurn();
        }
    }
    public void RegisterShotEfectuated()
    {
        remainingShots--;
        if (remainingShots <= 0) EndTurn();
    }
    public void EndTurn()
    {
        currentPlayer = turnData.SistemShift.AdvanceShift();
        remainingShots = 0;
        hasRolledDice = false;
        Debug.Log($"¡Nuevo turno para: {currentPlayer?.PlayerID}!");
    }

    public bool CanExecuteShot(PlayerGame player)
    {
        return currentPlayer == player && hasRolledDice && remainingShots > 0;
    }
}
