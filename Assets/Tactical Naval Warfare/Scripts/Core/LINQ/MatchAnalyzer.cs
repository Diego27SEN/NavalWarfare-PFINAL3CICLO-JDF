using System.Linq;
using UnityEngine;

public class MatchAnalyzer
{
    private BiomeCollectionData biomeData;
    private PlayerRuntimeSetData playerData;
    private ShipCatalogData shipData;

    public MatchAnalyzer(BiomeCollectionData biome, PlayerRuntimeSetData player, ShipCatalogData ship)
    {
        biomeData = biome;
        playerData = player;
        shipData = ship;
    }

    public void FindPlayerInDanger()
    {
        var playerdanger = playerData.PlayersActive.Values
           .FirstOrDefault(j => j.npcsLive == 1 && !j.shipDestroyed);

        Debug.Log(playerdanger != null ? $"El: {playerdanger.PlayerID} esta en peligro." : "Todos los barcos tienen a sus tripulantes");
    }

    public void FilterPowerfulShips()
    {
        var nameShip = shipData.CatalogBoats.Values
            .Where(b => b.EquippedCannon != null && b.EquippedCannon.ShotDamage > 30.00f)
            .Select(b => b.NameBoat);

        Debug.Log("Barcos de alto daño detectados");
        foreach (var name in nameShip)
            Debug.Log($"{name}");
    }

    public void ShowRankingTop()
    {
        var leader = playerData.PlayersActive.Values
            .OrderByDescending(j => j.currentScore)
            .FirstOrDefault();

        if (leader != null)
            Debug.Log($"EL Primer puesto es: {leader.PlayerID} con {leader.currentScore} puntos.");
        else
            Debug.Log("No hay jugadores activos para mostrar ranking.");
    }

    public void ShowMatchStatus()
    {
        int eliminated = playerData.PlayersActive.Values.Count(j => j.shipDestroyed || j.npcsLive == 0);
        bool hayKraken = biomeData.AvailableBiomes.Values.Any(b => b.EnvironmentalHazard == "Kraken");

        Debug.Log($"Total flotas eliminadas: {eliminated} | ¿Presencia de Kraken?: {hayKraken}");
    }
}
