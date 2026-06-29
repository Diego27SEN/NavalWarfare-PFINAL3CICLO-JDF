using UnityEngine;

public class ShipStats : MonoBehaviour
{
    [Header("Estadísticas Actuales")]
    [SerializeField] private int maxHealth = 400;
    public int currentHealth;
    public int currentShield = 0;
    public int currentSoldiers = 3;

    void Start()
    {
        currentHealth = maxHealth;
    }
    public void ApplyStatBuff(CardsDatabase cardBuff)
    {
        // Tipos de Cartas
        switch (cardBuff.statToBuff)
        {
            case StatType.Salud:
                currentHealth = Mathf.Min(currentHealth + cardBuff.buffAmount, maxHealth);
                break;

            case StatType.Escudo:
                currentShield += cardBuff.buffAmount;
                break;

            case StatType.Soldados:
                currentSoldiers += cardBuff.buffAmount;
                break;
        }
    }

}
