using UnityEngine;

[CreateAssetMenu(fileName = "BiomaData", menuName = "Scriptable Objects/BiomaData")]
public class BiomaData : ScriptableObject
{
    public string nombreBioma;
    public string peligroAmbiental;
    public float factorVientoMaximo = 6.00f;
}
