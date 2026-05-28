using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PoolConfigCollectionSO", menuName = "NavalWarfare/PoolConfigCollectionSO")]
public class PoolConfigCollectionSO : ScriptableObject
{
    public List<PoolConfigData> PoolConfigurations = new List<PoolConfigData>();
}
