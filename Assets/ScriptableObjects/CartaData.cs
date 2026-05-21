using UnityEngine;

[CreateAssetMenu(fileName = "NuevaCarta", menuName = "NavalWarfare/CartaData")]
public class CartaData : ScriptableObject
{
    public string nombreCarta;
    public string tipoEfecto;
    public bool esEfectoPositivo;
}
