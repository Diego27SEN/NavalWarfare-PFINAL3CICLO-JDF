using UnityEngine;
using TMPro;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TurnManager turnManager;

    private void Update()
    {
        if (turnManager == null) return;

        float time = Mathf.Min(turnManager.currentTime, turnManager.turnDuration);
        time = Mathf.Max(0, time);
        timerText.text = Mathf.CeilToInt(time).ToString();
    }
}
