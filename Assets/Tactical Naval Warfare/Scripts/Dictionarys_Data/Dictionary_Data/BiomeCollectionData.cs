using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BiomeCollectionSO", menuName = "TacticalNavalWarfare/BiomeCollectionSO")]
public class BiomeCollectionData : SerializedScriptableObject
{
    [ShowInInspector]
    public Dictionary<string, BiomeData> AvailableBiomes = new Dictionary<string, BiomeData>();
}

