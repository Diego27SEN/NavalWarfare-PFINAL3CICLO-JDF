using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Array de Biomas")]
    public BiomaData[] biomasDisponibles;

    [Header("List Jugadores")]
    public List<PlayerGame> jugadoresActivos = new List<PlayerGame>();

    [Header("Dictionary Busqueda por Catalogo)")]
    public Dictionary<string, DataShipBase> catalogoBarcos = new Dictionary<string, DataShipBase>();

    [Header("Coleccion Personalizada LinkedList - Turnos")]
    public QueueTurn sistemaDeTurnos = new QueueTurn();

    private void Awake() 
    { 
        Instance = this; 
    }

    public void RegisterPlayer(PlayerGame newPlayer)
    {
        jugadoresActivos.Add(newPlayer);
        sistemaDeTurnos.AddShift(newPlayer);
        if (newPlayer.SelectedShip != null)
            catalogoBarcos[newPlayer.SelectedShip.nombreBarco] = newPlayer.SelectedShip;
    }

    #region Metodos Linq

    [Button("Primer jugador en peligro")]
    public void FindPlayerInDanger()
    {
        var playerdanger = jugadoresActivos.FirstOrDefault(j => j.npcsVivos == 1 && !j.barcoDestruido);
        Debug.Log(playerdanger != null ? $"El: {playerdanger.IDJugador} esta peligro." : "Todos los barcos tienen a sus tripulantes");
    }

    [Button("Barcos de alto daño")]
    public void FilterPowerfulShips()
    {
        var nameShip = catalogoBarcos.Values
            .Where(b => b.cañonEquipado.dañoPorDisparo > 30.00f)
            .Select(b => b.nombreBarco);

        Debug.Log("Barcos de alto daño detectados");
        foreach (var name in nameShip)
          Debug.Log($"{name}");
    }

    [Button("Mostrar Ranking Top")]
    public void ShowRankingTop()
    {
        var lider = jugadoresActivos
            .OrderByDescending(j => j.scoreActual)
            .Take(1)
            .FirstOrDefault();

        if (lider != null)
            Debug.Log($"EL Primer puesto es: {lider.IDJugador} con {lider.scoreActual} puntos.");
        else
            Debug.Log("No hay jugadores activos para mostrar ranking.");
    }

    [Button("Mostrar Estado de la partida")]
    public void ShowMatchStatus()
    {
        int eliminados = jugadoresActivos.Count(j => j.barcoDestruido || j.npcsVivos == 0);
        bool hayKraken = biomasDisponibles.Any(b => b.environmentalHazard == "Kraken");

        Debug.Log($"Total flotas eliminadas: {eliminados} | ¿Presencia de Kraken?: {hayKraken}");
    }
    #endregion 
}