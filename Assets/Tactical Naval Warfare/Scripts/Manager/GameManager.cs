using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Array de Biomas")]
    public BiomeData[] AvailableBiomes;

    [Header("List Jugadores")]
    public List<PlayerGame> PlayersActive = new List<PlayerGame>();

    [Header("Dictionary Busqueda por Catalogo)")]
    public Dictionary<string, DataShipBase> CatalogBoats = new Dictionary<string, DataShipBase>();

    [Header("Coleccion Personalizada LinkedList - Turnos")]
    public QueueTurn SistemShift = new QueueTurn();

    private void Awake() 
    { 
        Instance = this; 
    }

    public void RegisterPlayer(PlayerGame newPlayer)
    {
        PlayersActive.Add(newPlayer);
        SistemShift.AddShift(newPlayer);
        if (newPlayer.SelectedShip != null)
            CatalogBoats[newPlayer.SelectedShip.NameBoat] = newPlayer.SelectedShip;
    }

    #region Metodos Linq

    [Button("Primer jugador en peligro")]
    public void FindPlayerInDanger()
    {
        var playerdanger = PlayersActive.FirstOrDefault(j => j.npcsLive == 1 && !j.shipDestroyed);
        Debug.Log(playerdanger != null ? $"El: {playerdanger.PlayerID} esta peligro." : "Todos los barcos tienen a sus tripulantes");
    }

    [Button("Barcos de alto daño")]
    public void FilterPowerfulShips()
    {
        var nameShip = CatalogBoats.Values
            .Where(b => b.EquippedCannon.ShotDamage > 30.00f)
            .Select(b => b.NameBoat);

        Debug.Log("Barcos de alto daño detectados");
        foreach (var name in nameShip)
          Debug.Log($"{name}");
    }

    [Button("Mostrar Ranking Top")]
    public void ShowRankingTop()
    {
        var lider = PlayersActive
            .OrderByDescending(j => j.currentScore)
            .Take(1)
            .FirstOrDefault();

        if (lider != null)
            Debug.Log($"EL Primer puesto es: {lider.PlayerID} con {lider.currentScore} puntos.");
        else
            Debug.Log("No hay jugadores activos para mostrar ranking.");
    }

    [Button("Mostrar Estado de la partida")]
    public void ShowMatchStatus()
    {
        int eliminados = PlayersActive.Count(j => j.shipDestroyed || j.npcsLive == 0);
        bool hayKraken = AvailableBiomes.Any(b => b.EnvironmentalHazard == "Kraken");

        Debug.Log($"Total flotas eliminadas: {eliminados} | ¿Presencia de Kraken?: {hayKraken}");
    }
    #endregion 
}