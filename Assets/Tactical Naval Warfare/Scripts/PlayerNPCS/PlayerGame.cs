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
