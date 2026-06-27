using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Referencias UI de Disparos")]
    [SerializeField] private TextMeshProUGUI diceResultText;
    [SerializeField] private TextMeshProUGUI shootCountText;
    [SerializeField] private Button shootButton;

    private DiceTurnController turnController;
    private MatchAnalyzer matchAnalyzer;

    public PlayerGameDatabase[] jugadoresDeEstaPartida;

    [FoldoutGroup("Control de Turno"), ShowInInspector, ReadOnly]
    public PlayerGameDatabase currentPlayer => turnController?.currentPlayer;

    [FoldoutGroup("Control de Turno"), ShowInInspector, ReadOnly]
    public int remainingShots => turnController != null ? turnController.remainingShots : 0;

    [FoldoutGroup("Control de Turno"), ShowInInspector, ReadOnly]
    public bool hasRolledDice => turnController != null && turnController.hasRolledDice;

    [SerializeField] private TurnManager turnManager;

    private void Awake() 
    {
        if (Instance == null) 
        { 
            Instance = this;
            InitializeContainers();

            turnController = new DiceTurnController(turnDataContainer, diceResultText, shootCountText, shootButton);
            matchAnalyzer = new MatchAnalyzer(biomeDataContainer, playerDataContainer, shipDataContainer); ;
        }
        else 
        { 
            Destroy(gameObject); 
        }
    }
    void Start()
    {
        playerDataContainer.Initialize();

        foreach (var jugador in jugadoresDeEstaPartida)
        {
            RegisterPlayer(jugador);
        }

    }
    private void InitializeContainers()
    {
        if (playerDataContainer != null) playerDataContainer.Initialize();
        if (shipDataContainer != null) shipDataContainer.Initialize();
        if (turnDataContainer != null) turnDataContainer.Initialize();

    }
    public void RegisterPlayer(PlayerGameDatabase newPlayer)
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

    public void ForceEndTurn()
    {
        turnController.EndTurn();
        turnManager.NextTurn();
    }
    #region Redirección de Lógica de Turnos
    [FoldoutGroup("Control de Turno")]
    [Button("Lanzar Dado de 8 caras", ButtonSizes.Medium)]
    [DisableIf("hasRolledDice")]
    public void RollDice() => turnController.RollDice();

    public void RegisterShotEfectuated() => turnController.RegisterShotEfectuated();

    /*public bool CanExecuteShot(PlayerGameDatabase player) => turnController != null && turnController.CanExecuteShot(player);*/

    public bool CanExecuteShot(PlayerGameDatabase player)
    {
        if (currentPlayer == null)
        {
            Debug.LogWarning("[DiceTurnController] No hay un jugador actual asignado al turno.");
            return false;
        }
        string shootingPlayerID = player != null ? player.PlayerID : currentPlayer.PlayerID;
        bool isCorrectTurn = currentPlayer.PlayerID == shootingPlayerID;

        return isCorrectTurn && hasRolledDice && remainingShots > 0;
    }

    [FoldoutGroup("Control de Turno")]
    [Button("Terminar Turno Dado", ButtonSizes.Medium)]
    public void EndTurn()
    {

        if (!hasRolledDice)
        {
            Debug.LogWarning("¡Debes tirar el dado antes de terminar el turno!");
            return;
        }
        turnController.EndTurn();
        turnManager.NextTurn(); // cambia cámara 
    }
    private void LimpiarArmasTemporales()
    {
        if (currentPlayer == null) return;

        GameObject barcoEnEscena = currentPlayer.SelectedShip.Ship; 

        if (barcoEnEscena != null)
        {
            // Busca la interfaz
            ITemporaryWeapon armaTemporal = barcoEnEscena.GetComponent<ITemporaryWeapon>();

            // Desactiva si no lo encuentra
            armaTemporal?.DisableWeapon();
        }
    }
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