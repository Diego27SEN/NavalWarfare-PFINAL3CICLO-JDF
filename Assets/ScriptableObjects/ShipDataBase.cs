using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShipDataBase", menuName = "NavalWarfare/ShipDataBase")]
public class ShipDataBase : ScriptableObject
{
    public List<DataShip> allShips;
}
