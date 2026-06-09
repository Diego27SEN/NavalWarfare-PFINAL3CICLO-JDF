using UnityEngine;

public class PlayerGame : MonoBehaviour
{
    [Header("Datos del Jugador")]
    public string PlayerID;
    public ShipDatabase SelectedShip;
    public CrewmanDatabase soldierNPC;

    [Header("Estado Dinamico en Partida")]
    public int currentScore;
    public int npcsLive = 6; 
    public bool shipDestroyed = false;
    void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterPlayer(this);
        }
        else
        {
            Debug.LogWarning("No se encontró el GameManager para registrar a " + PlayerID);
        }

    }
}
