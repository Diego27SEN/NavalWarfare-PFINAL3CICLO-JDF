using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CrewmanCollectionSO", menuName = "TacticalNavalWarfare/CrewmanCollectionSO")]
public class CrewmanCollectionData : SerializedScriptableObject
{
    [Header("Lista Global de Tripulantes")]
    [ShowInInspector]
    public Dictionary<string, CrewmanDatabase> AvailableCrew = new ();

    public CrewmanDatabase GetCrewmanByName(string name)
    {
        if (AvailableCrew.TryGetValue(name, out CrewmanDatabase crewman))
        {
            return crewman;
        }

        Debug.LogWarning("No se encontró el tripulante con el nombre: " + name);
        return null;
    }
}
