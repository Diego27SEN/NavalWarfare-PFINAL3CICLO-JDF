using UnityEngine;
[CreateAssetMenu(fileName = "ShipDataBase", menuName = "TacticalNavalWarfare/DataShipBase")]
public class ShipDatabase : ScriptableObject
{
    [Header("NameShip")]
    public string NameBoat;
    public CannonDatabase EquippedCannon;

    [Header("Identificacion")]
    public string NameColor;
    public Color color;
    public GameObject Ship;

    [Header("Estadisticas")]
    public float hpMaximum = 400.00f;
  
}
