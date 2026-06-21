using UnityEngine;

public class DiceTurnController
{
    private TurnSystemData turnData;

    public PlayerGameDatabase currentPlayer;
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

        // Generamos un número entre 1 y 6 (dado de 6 caras)
        int diceResult = Random.Range(1, 7);
        Debug.Log($"El Dado {currentPlayer.PlayerID} obtuvo un: {diceResult}");

        if (diceResult <= 3)
        {
            // Caras 1, 2, 3 -> 1 disparo (3 caras)
            remainingShots = 1;
        }
        else if (diceResult <= 5)
        {
            // Caras 4, 5 -> 2 disparos (2 caras)
            remainingShots = 2;
        }
        else
        {
            // Cara 6 -> 3 disparos (1 cara)
            remainingShots = 3;
        }

        Debug.Log($"{currentPlayer.PlayerID} obtuvo {remainingShots} disparos.");
    }
    public void RegisterShotEfectuated()
    {
        remainingShots--;
        Debug.Log($"Disparos restantes: {remainingShots}");
    }
    public void EndTurn()
    {
        currentPlayer = turnData.SistemShift.AdvanceShift();
        remainingShots = 0;
        hasRolledDice = false;
        Debug.Log($"¡Nuevo turno para: {currentPlayer?.PlayerID}!");
    }

    public bool CanExecuteShot(PlayerGameDatabase player)
    {
        return currentPlayer == player && hasRolledDice && remainingShots > 0;
    }
}
