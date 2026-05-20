using UnityEngine;

public class PlayerGame : MonoBehaviour
{
    [Header("Datos del Jugador")]
    public string IDJugador;
    public DataShipBase SelectedShip;
    public TripulanteData soldierNPC;

    [Header("Estado Dinámico en Partida")]
    public int scoreActual;
    public int npcsVivos = 4; 
    public bool barcoDestruido = false;
}
