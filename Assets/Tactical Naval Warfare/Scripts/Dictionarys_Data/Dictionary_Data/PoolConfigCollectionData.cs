using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PoolConfigCollectionData", menuName = "TacticalNavalWarfare/PoolConfigCollectionSO")]
public class PoolConfigCollectionData : SerializedScriptableObject
{
    [ShowInInspector]
    public Dictionary<string, PoolConfigDatabase> PoolConfigurations = new ();
}
