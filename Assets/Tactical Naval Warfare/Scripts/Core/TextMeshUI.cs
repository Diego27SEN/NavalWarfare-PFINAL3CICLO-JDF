using UnityEngine;
using TMPro;

public class TurnMeshUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI infoText;

    private void Update()
    {
        if (GameManager.Instance == null) return;

        if (!GameManager.Instance.hasRolledDice)
        {
            infoText.text = "¡Tira el dado primero antes de pasar de turno!";
            infoText.color = Color.yellow;
        }
        else if (GameManager.Instance.remainingShots <= 0)
        {
            infoText.text = "¡Sin disparos!";
            infoText.color = Color.red;
        }
        else
        {        
            infoText.text = "Disparos: " + GameManager.Instance.remainingShots;
            infoText.color = Color.white;
        }
    }
}