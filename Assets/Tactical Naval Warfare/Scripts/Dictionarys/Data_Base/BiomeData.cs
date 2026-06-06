using UnityEngine;

[CreateAssetMenu(fileName = "BiomeData", menuName = "TacticalNavalWarfare/BiomeData")]
public class BiomeData : ScriptableObject
{
    public string NameBiome;
    public string EnvironmentalHazard;
    public float WindMaximum = 6.00f;
}
