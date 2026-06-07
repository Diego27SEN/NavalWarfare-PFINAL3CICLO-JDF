using UnityEngine;

[CreateAssetMenu(fileName = "CrewmanDatabase", menuName = "TacticalNavalWarfare/CrewmanData")]
public class CrewmanDatabase : ScriptableObject
{
    public string NameCrewman; //Stumble
    public Color colorTeam;
    public float ImpactResistance = 10.50f;
}
