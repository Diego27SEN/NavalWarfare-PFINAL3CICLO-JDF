using UnityEngine;

[CreateAssetMenu(fileName = "BiomaData", menuName = "NavalWarfare/BiomaData")]
public class BiomaData : ScriptableObject
{
    public string NameBiome;
    public string EnvironmentalHazard;
    public float WindMaximum = 6.00f;
}
