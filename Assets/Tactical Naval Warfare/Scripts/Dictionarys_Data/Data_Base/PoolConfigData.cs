using UnityEngine;

[CreateAssetMenu(fileName = "PoolConfigData", menuName = "TacticalNavalWarfare/PoolConfigData")]
public class PoolConfigData : ScriptableObject
{
    public string poolID;
    public GameObject prefab;
    public int initialSize = 15;
}
