using UnityEngine;
[CreateAssetMenu(fileName = "DataShip", menuName = "NavalWarfare/DataShip")]
public class DataShip : ScriptableObject
{
    [Header("Identifiacion")]
    public string NameColor;
    public Color color;
    public GameObject Ship;

    [Header("Estadisticas")]
    public float maxHealth;
    
}
