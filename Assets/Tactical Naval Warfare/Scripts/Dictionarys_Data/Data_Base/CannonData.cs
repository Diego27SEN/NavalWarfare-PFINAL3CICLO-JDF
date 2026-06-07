using UnityEngine;

[CreateAssetMenu(fileName = "CanonData", menuName = "TacticalNavalWarfare/CannonData")]
public class CannonData : ScriptableObject
{
    [Header("Stats")]
    public GameObject ModelCannon;
    public float ShotDamage;
    public float RangeMaximun;
}
