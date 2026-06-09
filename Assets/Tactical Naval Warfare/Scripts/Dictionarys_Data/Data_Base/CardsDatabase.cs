using UnityEngine;

[CreateAssetMenu(fileName = "CardsDatabase", menuName = "TacticalNavalWarfare/CardsDataSO")]

public class CardsDatabase : ScriptableObject
{
    public CardType CardType;
    public string NameCart;
    public string Description;
}
