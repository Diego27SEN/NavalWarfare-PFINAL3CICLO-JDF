using UnityEngine;

[CreateAssetMenu(fileName = "CrewmanData", menuName = "NavalWarfare/TripulanteData")]
public class CrewmanData : ScriptableObject
{
    public string nameNpc; //Stumble
    public Color colorTeam;
    public float ImpactResistance = 10.50f;
}
