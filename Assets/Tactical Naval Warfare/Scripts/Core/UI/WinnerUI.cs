using TMPro;
using UnityEngine;

public class WinnerUI : MonoBehaviour
{
    [SerializeField] private PlayerNames playerNamesSO;
    [SerializeField] private TextMeshProUGUI winnerText;

    private void Start()
    {
        winnerText.text = playerNamesSO.GetName(playerNamesSO.WinnerShipType);
    }
}