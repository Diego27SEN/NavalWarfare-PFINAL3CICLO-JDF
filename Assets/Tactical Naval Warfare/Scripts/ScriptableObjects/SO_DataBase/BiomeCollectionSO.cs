using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BiomeCollectionSO", menuName = "NavalWarfare/BiomeCollectionSO")]
public class BiomeCollectionSO : ScriptableObject
{
    public List<BiomeData> AvailableBiomes = new List<BiomeData>();
}
