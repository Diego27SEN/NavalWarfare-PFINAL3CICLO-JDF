using UnityEngine;

[CreateAssetMenu(fileName = "BiomaData", menuName = "NavalWarfare/BiomaData")]
public class BiomaData : ScriptableObject
{
    public string nameBiome;
    public string environmentalHazard;
    public float WindMaximum = 6.00f;
}
