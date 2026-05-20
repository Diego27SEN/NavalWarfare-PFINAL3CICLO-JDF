using UnityEngine;

[CreateAssetMenu(fileName = "CanonData", menuName = "Scriptable Objects/CañonData")]
public class CanonData : ScriptableObject
{
    [Header("Stats")]
    public string modeloCañon;
    public float dañoPorDisparo;
    public float rangoMaximo;
}
