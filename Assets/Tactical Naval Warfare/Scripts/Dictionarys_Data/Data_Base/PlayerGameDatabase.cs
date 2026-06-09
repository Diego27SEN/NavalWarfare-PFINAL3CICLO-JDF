using UnityEngine;

[CreateAssetMenu(fileName = "PlayerGameDatabase", menuName = "TacticalNavalWarfare/Player")]
public class PlayerGameDatabase : ScriptableObject
{
    [Header("Datos del Jugador")]
    public string PlayerID;
    public ShipDatabase SelectedShip;
    public CrewmanDatabase soldierNPC;

    [Header("Estado Dinamico en Partida")]
    public int currentScore;
    public int npcsLive = 8; 
    public bool shipDestroyed = false;
}
