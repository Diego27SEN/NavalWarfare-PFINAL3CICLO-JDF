using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Arquitectura de Datos")]
    public BiomeCollectionSO biomeDataContainer;
    public PlayerRuntimeSetSO playerDataContainer;
    public ShipCatalogSO shipDataContainer;
    public TurnSystemSO turnDataContainer;

    [FoldoutGroup("Control de Turno")]
    public PlayerGame currentPlayer; 

    [FoldoutGroup("Control de Turno")]
    public int remainingShots = 0;   

    [FoldoutGroup("Control de Turno")]
    public bool hasRolledDice = false; 

    private void Awake() 
    {
        if (Instance == null) 
        { 
            Instance = this;
            InitializeContainers();
        }
        else 
        { 
            Destroy(gameObject); 
        }
    }
    private void InitializeContainers()
    {
        if (playerDataContainer != null) playerDataContainer.Initialize();
        if (shipDataContainer != null) shipDataContainer.Initialize();
        if (turnDataContainer != null) turnDataContainer.Initialize();
    }
    public void RegisterPlayer(PlayerGame newPlayer)
    {
        playerDataContainer.AddPlayer(newPlayer);
        turnDataContainer.SistemShift.AddShift(newPlayer);

        if (newPlayer.SelectedShip != null)
        {
            shipDataContainer.RegisterShip(newPlayer.SelectedShip.NameBoat, newPlayer.SelectedShip);
        }

        if (currentPlayer == null)
        {
            currentPlayer = turnDataContainer.SistemShift.GetCurrentPlayer();
        }
    }

    #region Lógica de Turnos (Dado De 8 caras)

    [FoldoutGroup("Control de Turno")]
    [Button("Lanzar Dado de 8 caras", ButtonSizes.Medium)]
    [DisableIf("hasRolledDice")]
    public void RollDice()
    {
        if (currentPlayer == null) return;

        hasRolledDice = true;
        int diceResult = Random.Range(1, 9); // Resultado del dado 1 al 8
        Debug.Log($"El Dado {currentPlayer.PlayerID} obtuvo un: {diceResult}");

        if (diceResult == 1 || diceResult == 3 || diceResult == 5)
        {
            remainingShots = 1; // Acciones para 1 disparo
        }
        else if (diceResult == 4 || diceResult == 6)
        {
            remainingShots = 2; // Acciones para 2 disparos
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

    [FoldoutGroup("Control de Turno")]
    [Button("Terminar Turno", ButtonSizes.Medium)]
    public void EndTurn()
    {
        currentPlayer = turnDataContainer.SistemShift.AdvanceShift();
        remainingShots = 0;
        hasRolledDice = false;
        Debug.Log($"¡Nuevo turno para: {currentPlayer.PlayerID}!");
    }
    #endregion

    #region Metodos Linq

    [Button("Primer jugador en peligro")]
    public void FindPlayerInDanger()
    {
        //Consulta LINQ apunta al contenedor de jugadores
        var playerdanger = playerDataContainer.PlayersActive
           .FirstOrDefault(j => j.npcsLive == 1 && !j.shipDestroyed);
        Debug.Log(playerdanger != null ? $"El: {playerdanger.PlayerID} esta peligro." : "Todos los barcos tienen a sus tripulantes");
    }

    [Button("Barcos de alto daño")]
    public void FilterPowerfulShips()
    {
        // Consulta LINQ apunta al contenedor del catálogo de barcos
        var nameShip = shipDataContainer.CatalogBoats.Values
            .Where(b => b.EquippedCannon != null && b.EquippedCannon.ShotDamage > 30.00f)
            .Select(b => b.NameBoat);

        Debug.Log("Barcos de alto daño detectados");
        foreach (var name in nameShip)
          Debug.Log($"{name}");
    }

    [Button("Mostrar Ranking Top")]
    public void ShowRankingTop()
    {
        var leader = playerDataContainer.PlayersActive
            .OrderByDescending(j => j.currentScore)
            .Take(1)
            .FirstOrDefault();

        if (leader != null)
            Debug.Log($"EL Primer puesto es: {leader.PlayerID} con {leader.currentScore} puntos.");
        else
            Debug.Log("No hay jugadores activos para mostrar ranking.");
    }

    [Button("Mostrar Estado de la partida")]
    public void ShowMatchStatus()
    {
        int eliminated = playerDataContainer.PlayersActive.Count(j => j.shipDestroyed || j.npcsLive == 0);
        bool hayKraken = biomeDataContainer.AvailableBiomes.Any(b => b.EnvironmentalHazard == "Kraken");

        Debug.Log($"Total flotas eliminadas: {eliminated} | ¿Presencia de Kraken?: {hayKraken}");
    }
    #endregion 
}