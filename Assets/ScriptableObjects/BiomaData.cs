using UnityEngine;

[CreateAssetMenu(fileName = "BiomaData", menuName = "NavalWarfare/BiomaData")]
public class BiomaData : ScriptableObject
{
    public string nombreBioma;
    public string peligroAmbiental;
    public float factorVientoMaximo = 6.00f;
}
