using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Arquitectura de Datos")]
    public BiomeCollectionData biomeDataContainer;
    public PlayerRuntimeSetData playerDataContainer;
    public ShipCatalogData shipDataContainer;
    public TurnSystemData turnDataContainer;
    public PoolConfigCollectionData poolDataContainer;
    public CardDeckData cardDataContainer;
    public CrewmanCollectionData crewmanDataContainer;

    private DiceTurnController turnController;
    private MatchAnalyzer matchAnalyzer;

    [FoldoutGroup("Control de Turno"), ShowInInspector, ReadOnly]
    public PlayerGame currentPlayer => turnController?.currentPlayer;

    [FoldoutGroup("Control de Turno"), ShowInInspector, ReadOnly]
    public int remainingShots => turnController != null ? turnController.remainingShots : 0;

    [FoldoutGroup("Control de Turno"), ShowInInspector, ReadOnly]
    public bool hasRolledDice => turnController != null && turnController.hasRolledDice;

    private void Awake() 
    {
        if (Instance == null) 
        { 
            Instance = this;
            InitializeContainers();

            turnController = new DiceTurnController(turnDataContainer);
            matchAnalyzer = new MatchAnalyzer(biomeDataContainer, playerDataContainer, shipDataContainer);
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
        // Delegado
        turnController.SetInitialPlayer();
    }

    #region Redirección de Lógica de Turnos
    [FoldoutGroup("Control de Turno")]
    [Button("Lanzar Dado de 8 caras", ButtonSizes.Medium)]
    [DisableIf("hasRolledDice")]
    public void RollDice() => turnController.RollDice();

    public void RegisterShotEfectuated() => turnController.RegisterShotEfectuated();

    public bool CanExecuteShot(PlayerGame player) => turnController != null && turnController.CanExecuteShot(player);

    [FoldoutGroup("Control de Turno")]
    [Button("Terminar Turno Dado", ButtonSizes.Medium)]
    public void EndTurn() => turnController.EndTurn();
    #endregion

    #region Redirección de Metodos Linq
    [FoldoutGroup("Análisis de Partida")]
    [Button("Primer jugador en peligro")]
    public void FindPlayerInDanger() => matchAnalyzer.FindPlayerInDanger();

    [FoldoutGroup("Análisis de Partida")]
    [Button("Barcos de alto daño")]
    public void FilterPowerfulShips() => matchAnalyzer.FilterPowerfulShips();

    [FoldoutGroup("Análisis de Partida")]
    [Button("Mostrar Ranking Top")]
    public void ShowRankingTop() => matchAnalyzer.ShowRankingTop();

    [FoldoutGroup("Análisis de Partida")]
    [Button("Mostrar Estado de la partida")]
    public void ShowMatchStatus() => matchAnalyzer.ShowMatchStatus();
    #endregion

}