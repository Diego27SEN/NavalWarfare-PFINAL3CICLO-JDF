using UnityEngine;
[CreateAssetMenu(fileName = "DataShip", menuName = "NavalWarfare/DataShip")]
public class DataShipBase : ScriptableObject
{
    [Header("NameShip")]
    public string nombreBarco;
    public CanonData cañonEquipado;

    [Header("Identifiacion")]
    public string NameColor;
    public Color color;
    public GameObject Ship;

    [Header("Estadisticas")]
    public float hpMaximo = 400.00f;
  
}
