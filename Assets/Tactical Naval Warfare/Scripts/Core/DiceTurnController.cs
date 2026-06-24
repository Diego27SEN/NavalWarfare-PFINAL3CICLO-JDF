using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiceTurnController
{
    private TurnSystemData turnData;

    public PlayerGameDatabase currentPlayer;
    public int remainingShots = 0;
    public bool hasRolledDice = false;

    [SerializeField] private TextMeshProUGUI DiceResult;
    [SerializeField] private TextMeshProUGUI ShootCount;
    [SerializeField] private Button shootButton;

    public DiceTurnController(TurnSystemData data, TextMeshProUGUI diceResultText, TextMeshProUGUI shootCountText, Button shootButton)
    {
        turnData = data;
        this.DiceResult = diceResultText;
        this.ShootCount = shootCountText;
        this.shootButton = shootButton;
    }

    public void SetInitialPlayer()
    {
        if (currentPlayer == null && turnData != null)
        {
            currentPlayer = turnData.SistemShift.GetCurrentPlayer();
            UpdateShotTexts();
        }
    }
    public void RollDice()
    {
        if (hasRolledDice)
        {
            Debug.LogWarning($"[DiceTurnController] El jugador {currentPlayer?.PlayerID} ya ha lanzado el dado este turno.");
            return; // Evitamos que el jugador tire el dado más de una vez por turno
        }
        if (currentPlayer == null)
        {
            Debug.LogWarning("[DiceTurnController] No hay un jugador actual asignado al turno.");   
            return;
        }


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

        if (DiceResult != null)
        {
            DiceResult.text = $"{currentPlayer.PlayerID} obtuvo {remainingShots} disparos";
        }

        UpdateShotTexts();
    }
    public void RegisterShotEfectuated()
    {
        remainingShots--;
        Debug.Log($"Disparos restantes: {remainingShots}");

        UpdateShotTexts();
    }
    public void EndTurn()
    {
        currentPlayer = turnData.SistemShift.AdvanceShift();
        remainingShots = 0;
        hasRolledDice = false;

        Debug.Log($"¡Nuevo turno para: {currentPlayer?.PlayerID}!");

        if (DiceResult != null)
        {
            DiceResult.text = "";
        }

        if (shootButton != null)
        {
            shootButton.gameObject.SetActive(true);
        }

        UpdateShotTexts();
    }

    public bool CanExecuteShot(PlayerGameDatabase player)
    {
        return currentPlayer == player && hasRolledDice && remainingShots > 0;
    }

    private void UpdateShotTexts()
    {
        if (ShootCount == null) return;

        if (!hasRolledDice)
        {
            ShootCount.text = "Tira el dado para conseguir disparos.";
            return;
        }

        if (remainingShots > 0)
        {
            string shotWord = remainingShots == 1 ? "disparo" : "disparos";
            ShootCount.text = $"Tienes {remainingShots} {shotWord}";
        }
        else
        {
            ShootCount.text = "Ya no tienes más disparos";

            if (shootButton != null)
            {
                shootButton.gameObject.SetActive(false);
            }
        }
    }
}
