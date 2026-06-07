using UnityEngine;

[CreateAssetMenu(fileName = "CardsDatabase", menuName = "NavalWarfare/CardsDataSO")]

public class CardsDatabase : ScriptableObject
{
    public CardType CardType;
    public string NameCart;
    public string Description;
}
