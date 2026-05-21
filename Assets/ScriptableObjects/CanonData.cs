using UnityEngine;

[CreateAssetMenu(fileName = "CanonData", menuName = "NavalWarfare/CañonData")]
public class CanonData : ScriptableObject
{
    [Header("Stats")]
    public GameObject modeloCañon;
    public float dañoPorDisparo;
    public float rangoMaximo;
}
