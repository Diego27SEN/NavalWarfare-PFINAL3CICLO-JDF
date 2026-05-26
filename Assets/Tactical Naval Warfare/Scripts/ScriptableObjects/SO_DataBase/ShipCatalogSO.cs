using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShipCatalogSO", menuName = "NavalWarfare/ShipCatalogSO")]
public class ShipCatalogSO : ScriptableObject
{
    [ShowInInspector]
    public Dictionary<string, DataShipBase> CatalogBoats = new Dictionary<string, DataShipBase>();

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
