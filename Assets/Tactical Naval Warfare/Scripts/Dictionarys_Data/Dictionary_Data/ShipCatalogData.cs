using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShipCatalogSO", menuName = "TacticalNavalWarfare/ShipCatalogSO")]
public class ShipCatalogData : SerializedScriptableObject
{
    [ShowInInspector]
    public Dictionary<string, DataShipBase> CatalogBoats = new();

    // Método para limpiar el diccionario al iniciar
    public void Initialize()
    {
        CatalogBoats.Clear();
    }

    public void RegisterShip(string name, DataShipBase ship)
    {
        CatalogBoats[name] = ship;
    }
}
