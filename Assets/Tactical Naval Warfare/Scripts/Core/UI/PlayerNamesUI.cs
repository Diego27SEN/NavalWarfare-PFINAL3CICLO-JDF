using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;

public class PlayerNamesUI : MonoBehaviour
{
    [Title("Configuración del Almacenamiento")]
    [Required("Arrastra aquí tu Scriptable Object de nombres")]
    [SerializeField] private PlayerNames playerNamesSO;

    [Title("Inputs de Texto para los Barcos")]
    [SerializeField] private TMP_InputField inputShipA;
    [SerializeField] private TMP_InputField inputShipB;
    [SerializeField] private TMP_InputField inputShipC;
    [SerializeField] private TMP_InputField inputShipD;

    [Title("Tipos de Barcos (Asignación)")]
    [SerializeField] private ShipType shipTypeA;
    [SerializeField] private ShipType shipTypeB;
    [SerializeField] private ShipType shipTypeC;
    [SerializeField] private ShipType shipTypeD;

    public void SavePlayerNames()
    {
        if (playerNamesSO == null)
        {
            Debug.LogError("No has asignado el Scriptable Object 'PlayerNames' en el inspector.");
            return;
        }

        playerNamesSO.ShipNames.Clear();

        SaveIndName(shipTypeA, inputShipA, "Ship A");
        SaveIndName(shipTypeB, inputShipB, "Ship B");
        SaveIndName(shipTypeC, inputShipC, "Ship C");
        SaveIndName(shipTypeD, inputShipD, "Ship D");
    }

    private void SaveIndName(ShipType type, TMP_InputField inputField, string defaultName)
    {
        if (inputField != null)
        {
            string nameToSave = string.IsNullOrWhiteSpace(inputField.text) ? defaultName : inputField.text;
            playerNamesSO.SaveName(type, nameToSave);
            Debug.Log($"Nombre guardado para {type}: {nameToSave}");
        }
    }

}
