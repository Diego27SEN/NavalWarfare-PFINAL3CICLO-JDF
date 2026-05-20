using UnityEngine;

[CreateAssetMenu(fileName = "CanonData", menuName = "NavalWarfare/CañonData")]
public class CanonData : ScriptableObject
{
    [Header("Stats")]
    public string modeloCañon;
    public float dañoPorDisparo;
    public float rangoMaximo;
}
