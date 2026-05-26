using UnityEngine;

[CreateAssetMenu(fileName = "PoolConfigData", menuName = "System Naval/PoolConfigData")]
public class PoolConfigData : ScriptableObject
{
    public string poolID;
    public GameObject prefab;
    public int initialSize = 15;
}
