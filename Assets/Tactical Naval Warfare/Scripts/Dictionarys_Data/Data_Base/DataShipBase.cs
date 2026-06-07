using UnityEngine;
[CreateAssetMenu(fileName = "DataShipBase", menuName = "TacticalNavalWarfare/DataShipBase")]
public class DataShipBase : ScriptableObject
{
    [Header("NameShip")]
    public string NameBoat;
    public CannonData EquippedCannon;

    [Header("Identifiacion")]
    public string NameColor;
    public Color color;
    public GameObject Ship;

    [Header("Estadisticas")]
    public float hpMaximum = 400.00f;
  
}
