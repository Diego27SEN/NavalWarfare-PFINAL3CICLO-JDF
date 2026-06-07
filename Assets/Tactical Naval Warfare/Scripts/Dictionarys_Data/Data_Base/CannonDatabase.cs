using UnityEngine;

[CreateAssetMenu(fileName = "CanonDatabase", menuName = "TacticalNavalWarfare/CannonData")]
public class CannonDatabase : ScriptableObject
{
    [Header("Stats")]
    public GameObject ModelCannon;
    public float ShotDamage;
    public float RangeMaximun;
}
