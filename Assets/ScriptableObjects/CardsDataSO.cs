using UnityEngine;

[CreateAssetMenu(fileName = "CardsDataSO", menuName = "Scriptable Objects/CardsDataSO")]

public class CardsDataSO : ScriptableObject
{
    public CardType CardType;
    public string Name;
    public string Description;
}
