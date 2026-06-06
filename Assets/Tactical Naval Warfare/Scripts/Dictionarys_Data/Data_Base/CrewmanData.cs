using UnityEngine;

[CreateAssetMenu(fileName = "CrewmanData", menuName = "TacticalNavalWarfare/CrewmanData")]
public class CrewmanData : ScriptableObject
{
    public string NameCrewman; //Stumble
    public Color colorTeam;
    public float ImpactResistance = 10.50f;
}
