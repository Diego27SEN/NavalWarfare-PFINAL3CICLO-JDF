using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] private Button shootButton;
    [SerializeField] private Button finishTurnButton;
    [SerializeField] private Button rollDiceButton;

    private ShootController currentShootController;
    [SerializeField] private SoundManager soundManager;

    private void Start()
    {
        finishTurnButton.onClick.AddListener(() =>
        {     
            GameManager.Instance.EndTurn();
        });

        rollDiceButton.onClick.AddListener(() =>
        {
            GameManager.Instance.RollDice();

            if (soundManager != null)
            {
                soundManager.PlayExtra(1);
            }
        });
    }

    public void UpdateActiveShip(ShipController ship)
    {
        currentShootController = ship.GetComponentInChildren<ShootController>();

        shootButton.onClick.RemoveAllListeners();
        shootButton.onClick.AddListener(() => currentShootController?.FireCannon());
    }
}