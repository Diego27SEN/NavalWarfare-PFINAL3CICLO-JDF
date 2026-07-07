using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsEventManager : MonoBehaviour
{
    public static CardsEventManager Instance;

    [Header("Referencias")]
    public CardDeckData deck;
    public CardSelectionUI cardUI;
    public TurnManager turnManager;

    private const float SELECTION_TIME = 20f;

    void Awake() => Instance = this;

    public void StartCardEvent(ShipController[] ships)
    {
        StartCoroutine(RunSequential(ships));
    }

    private IEnumerator RunSequential(ShipController[] ships)
    {
        turnManager.PauseTimer();

        foreach (var ship in ships)
        {
            if (ship == null) continue;
            yield return StartCoroutine(ShowChoiceFor(ship));
        }

        turnManager.ResumeTimer();
        turnManager.StartTurn();
    }

    private IEnumerator ShowChoiceFor(ShipController ship)
    {
        turnManager.FocusCameraOn(ship);

        List<CardsDatabase> options = new List<CardsDatabase>
        {
            deck.AvailableCards["2"],
            deck.AvailableCards["1"],
            deck.AvailableCards["4"],
            deck.AvailableCards["3"]
        };

        CardsDatabase selected = null;
        bool locked = false;

        cardUI.ShowCards(ship, options, card =>
        {
            if (locked) return;
            selected = card;
            locked = true;
        });

        float t = SELECTION_TIME;
        while (t > 0)
        {
            cardUI.UpdateTimer(t);
            t -= Time.deltaTime;
            yield return null;
        }

        if (!locked)
        {
            selected = options[Random.Range(0, options.Count)];
            Debug.Log($"[CardsEventManager] Tiempo agotado para {ship.name}. Carta aleatoria: {selected.NameCart}");
        }

        cardUI.HideCards();
        ApplyCard(selected, ship);
    }

    private void ApplyCard(CardsDatabase card, ShipController ship)
    {
        switch (card.abilityID)
        {
            case "3":
                SkillManager.Instance.EquiparHabilidadAlBarco(ship, "3");
                break;
            case "4":
                SkillManager.Instance.EquiparHabilidadAlBarco(ship, "4");
                break;
            case "2":
                ship.gameObject.AddComponent<AquaticAgilitySkil>();
                break;
            case "1":
                ship.gameObject.AddComponent<MaritimeBlockadeSkill>();
                break;
            default:
                Debug.LogWarning($"[CardsEventManager] abilityID '{card.abilityID}' no reconocido.");
                break;
        }
    }
}