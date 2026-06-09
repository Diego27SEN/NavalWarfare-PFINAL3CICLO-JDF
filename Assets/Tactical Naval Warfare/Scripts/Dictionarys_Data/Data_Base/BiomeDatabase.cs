using UnityEngine;

[CreateAssetMenu(fileName = "BiomeDatabase", menuName = "TacticalNavalWarfare/BiomeData")]
public class BiomeDatabase : ScriptableObject
{
    public string NameBiome;
    public string EnvironmentalHazard;
    public float WindMaximum = 6.00f;
}
