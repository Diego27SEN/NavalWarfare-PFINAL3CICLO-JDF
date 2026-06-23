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

    [Title("Modificadores Directos (Solo para StatBuff)")]
    [ShowIf("Category", SkillCategory.StatBuff)]
    [Tooltip("Recuperación")]
    public int healthBonus;

    [ShowIf("Category", SkillCategory.StatBuff)]
    [Tooltip("Blindaje")]
    public int shieldBonus;

    [ShowIf("Category", SkillCategory.StatBuff)]
    [Tooltip("Refuerzos")]
    public int soldierBonus;

    [Title("Configuración de Factory")]
    [ShowIf("Category", SkillCategory.ComplexHability)]
    [Tooltip("ID exacto para el Switch de la Factory 'GatlingGun', 'Mortero', 'Bloqueo'")]
    public string abilityID;
}
