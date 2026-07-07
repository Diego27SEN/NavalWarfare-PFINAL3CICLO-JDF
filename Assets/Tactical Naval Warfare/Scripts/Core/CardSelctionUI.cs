using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardSelectionUI : MonoBehaviour
{
    [Header("Las 4 cartas ya existentes en la escena")]
    public Button cardMaritimeBlockadeButton; // CardsUICard_Sumerian
    public Button cardAquaticAgilityButton;   // CardsUICard_Fireproof
    public Button cardMortarButton;           // CardsUICard_Lady
    public Button cardGatlingButton;          // CardsUICard_Stormeye

    [Header("Timer UI")]
    public TextMeshProUGUI timerText;

    [Header("Panel contenedor (para mostrar/ocultar todo junto)")]
    public GameObject cardsPanel; // el objeto "Container" padre

    private Action<CardsDatabase> onCardSelected;
    private Dictionary<Button, CardsDatabase> buttonToCard;

    public void ShowCards(ShipController ship, List<CardsDatabase> options, Action<CardsDatabase> callback)
    {
        onCardSelected = callback;
        cardsPanel.SetActive(true);

        // Mapeamos cada botón fijo a la carta correspondiente por abilityID
        buttonToCard = new Dictionary<Button, CardsDatabase>();
        foreach (var card in options)
        {
            switch (card.abilityID)
            {
                case "MaritimeBlockade":
                    buttonToCard[cardMaritimeBlockadeButton] = card;
                    break;
                case "AquaticAgility":
                    buttonToCard[cardAquaticAgilityButton] = card;
                    break;
                case "Mortar":
                    buttonToCard[cardMortarButton] = card;
                    break;
                case "Gatling":
                    buttonToCard[cardGatlingButton] = card;
                    break;
            }
        }

        // Reactivamos y enganchamos el click a cada botón
        foreach (var kvp in buttonToCard)
        {
            Button btn = kvp.Key;
            CardsDatabase card = kvp.Value;

            btn.interactable = true;
            btn.onClick.RemoveAllListeners(); // limpiamos listeners viejos
            btn.onClick.AddListener(() => OnCardClicked(card));
        }
    }

    private void OnCardClicked(CardsDatabase card)
    {
        
        foreach (var kvp in buttonToCard) // bloqueamos todos los botones excepto el seleccionado
        {
            if (kvp.Value != card)
            {
                kvp.Key.interactable = false;
            }
        }

        onCardSelected?.Invoke(card);
    }

    public void UpdateTimer(float time)
    {
        if (timerText != null)
            timerText.text = Mathf.CeilToInt(time).ToString();
    }

    public void HideCards()
    {
        cardsPanel.SetActive(false);
    }
}