using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardDeckSO", menuName = "TacticalNavalWarfare/CardDeckSO")]
public class CardDeckData : SerializedScriptableObject
{
    [ShowInInspector]
    public Dictionary<string, CardsDataSO> AvailableCards = new ();
}
