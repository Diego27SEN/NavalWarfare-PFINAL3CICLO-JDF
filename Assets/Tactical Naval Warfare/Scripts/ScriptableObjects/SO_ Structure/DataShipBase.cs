using UnityEngine;
[CreateAssetMenu(fileName = "DataShip", menuName = "NavalWarfare/DataShip")]
public class DataShipBase : ScriptableObject
{
    [Header("NameShip")]
    public string NameBoat;
    public CanonData EquippedCannon;

    [Header("Identifiacion")]
    public string NameColor;
    public Color color;
    public GameObject Ship;

    [Header("Estadisticas")]
    public float hpMaximum = 400.00f;
  
}
