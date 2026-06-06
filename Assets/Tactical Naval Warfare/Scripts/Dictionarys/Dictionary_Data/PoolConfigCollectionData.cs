using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PoolConfigCollectionSO", menuName = "TacticalNavalWarfare/PoolConfigCollectionSO")]
public class PoolConfigCollectionData : SerializedScriptableObject
{
    [ShowInInspector]
    public Dictionary<string, PoolConfigData> PoolConfigurations = new ();
}
