using UnityEngine;

[CreateAssetMenu(fileName = "CanonData", menuName = "NavalWarfare/CañonData")]
public class CanonData : ScriptableObject
{
    [Header("Stats")]
    public GameObject ModelCannon;
    public float ShotDamage;
    public float RangeMaximun;
}
