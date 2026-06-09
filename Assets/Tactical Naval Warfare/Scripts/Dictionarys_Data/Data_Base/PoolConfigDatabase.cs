using UnityEngine;

[CreateAssetMenu(fileName = "PoolConfigDatabase", menuName = "TacticalNavalWarfare/PoolConfigData")]
public class PoolConfigDatabase : ScriptableObject
{
    public string poolID;
    public GameObject prefab;
    public int initialSize = 15;
}
