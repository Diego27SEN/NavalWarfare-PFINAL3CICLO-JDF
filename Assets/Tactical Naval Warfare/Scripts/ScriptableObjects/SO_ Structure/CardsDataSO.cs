using UnityEngine;

[CreateAssetMenu(fileName = "CardsDataSO", menuName = "NavalWarfare/CardsDataSO")]

public class CardsDataSO : ScriptableObject
{
    public CardType CardType;
    public string NameCart;
    public string Description;
}
