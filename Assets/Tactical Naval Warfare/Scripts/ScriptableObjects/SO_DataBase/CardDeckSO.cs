using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardDeckSO", menuName = "NavalWarfare/CardDeckSO")]
public class CardDeckSO : ScriptableObject
{
    public List<CardsDataSO> AvailableCards = new List<CardsDataSO>();
}
