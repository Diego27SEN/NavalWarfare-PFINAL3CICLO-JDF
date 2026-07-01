using UnityEngine;

public class MysteryCratePowerUp : BasePowerUp
{
    [Header("Configuración Misteriosa")]
    public float effectAmount = 50f;

    [Tooltip("Probabilidad de que sea positivo (0.5 = 50%, 0.8 = 80%)")]
    [Range(0f, 1f)]
    public float winProbability = 0.5f;

    // POLIMORFISMO
    protected override void ApplyEffect(GameObject ship)
    {
        // Si no tiene ShipHealth, salimos inmediatamente. 
        if (!ship.TryGetComponent<ShipHealth>(out ShipHealth health)) return;

        // 2. Evaluamos la probabilidad directamente
        if (Random.value <= winProbability)
        {
            // Si la suma supera los 400, se quedará en 400 automáticamente
            health.currentHealth = Mathf.Min(health.currentHealth + effectAmount, 400f);

            Debug.Log($"¡Suerte! El barco recuperó vida. Vida actual: {health.currentHealth}");
        }
        else
        {
            // Resultado Negativo
            health.TakeDamage(effectAmount);
            Debug.Log($"¡Trampa! La caja explotó y el barco perdió {effectAmount} de vida.");
        }
    }
}
