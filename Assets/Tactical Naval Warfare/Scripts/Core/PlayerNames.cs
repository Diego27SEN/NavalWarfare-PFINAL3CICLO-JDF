using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerNames", menuName = "Scriptable Objects/PlayerNames")]
public class PlayerNames : SerializedScriptableObject
{
    public Dictionary<ShipType, string> shipNames = new();

    public Dictionary<ShipType, string> ShipNames => shipNames;

    public ShipType WinnerShipType;

    public void SaveName(ShipType type, string name)
    {
        if (shipNames.ContainsKey(type))
        {
            shipNames[type] = name;
        }
        else
        {
            shipNames.Add(type, name);
        }
    }
    public string GetName(ShipType type)
    {
        if (shipNames.TryGetValue(type, out string name))
        {
            return name;
        }
        return null; // or return a default name if desired
    }

    public void SetWinner(ShipType type)
    {
        WinnerShipType = type;
    }
}
