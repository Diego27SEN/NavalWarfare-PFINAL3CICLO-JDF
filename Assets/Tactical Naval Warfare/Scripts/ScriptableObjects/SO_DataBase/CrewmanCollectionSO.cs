using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CrewmanCollectionSO", menuName = "NavalWarfare/CrewmanCollectionSO")]
public class CrewmanCollectionSO : ScriptableObject
{
    [Header("Lista Global de Tripulantes")]
    public List<CrewmanData> AvailableCrew = new List<CrewmanData>();

    public CrewmanData GetCrewmanByName(string name)
    {  
        return AvailableCrew.Find(crew => crew.NameCrewman == name);
    }
}
