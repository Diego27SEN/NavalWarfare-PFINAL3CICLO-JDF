using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CrewmanCollectionSO", menuName = "TacticalNavalWarfare/CrewmanCollectionSO")]
public class CrewmanCollectionData : SerializedScriptableObject
{
    [Header("Lista Global de Tripulantes")]
    [ShowInInspector]
    public Dictionary<string, CrewmanData> AvailableCrew = new ();

    public CrewmanData GetCrewmanByName(string name)
    {
        if (AvailableCrew.TryGetValue(name, out CrewmanData crewman))
        {
            return crewman;
        }

        Debug.LogWarning("No se encontró el tripulante con el nombre: " + name);
        return null;
    }
}
