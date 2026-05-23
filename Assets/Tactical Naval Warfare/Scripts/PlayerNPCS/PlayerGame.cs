using UnityEngine;

public class PlayerGame : MonoBehaviour
{
    [Header("Datos del Jugador")]
    public string PlayerID;
    public DataShipBase SelectedShip;
    public CrewmanData soldierNPC;

    [Header("Estado Dinamico en Partida")]
    public int currentScore;
    public int npcsLive = 4; 
    public bool shipDestroyed = false;
    void Start()
    {
        GameManager.Instance.RegisterPlayer(this);
    }
}
