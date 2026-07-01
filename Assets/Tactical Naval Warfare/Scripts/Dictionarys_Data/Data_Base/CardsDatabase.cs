using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "CardsDatabase", menuName = "TacticalNavalWarfare/CardsDataSO")]

public class CardsDatabase : ScriptableObject
{
    [Title("Información Básica")]
    public string NameCart;

    [TextArea(3, 5)]
    public string Description;
    public CardType CardType;
    public Sprite cardIcon;

    [Title("Configuración de Habilidad")]
    public SkillCategory Category;

    [Title("Modificador Simple (Solo para StatBuff)")]
    [ShowIf("Category", SkillCategory.StatBuff)]
    public StatType statToBuff; // Eliges si la carta da Salud, Escudo o Soldados

    [ShowIf("Category", SkillCategory.StatBuff)]
    public int buffAmount;

    [Title("Configuración de Factory (Solo para ComplexAbility)")]
    [ShowIf("Category", SkillCategory.ComplexHability)]
    public string abilityID;
}
