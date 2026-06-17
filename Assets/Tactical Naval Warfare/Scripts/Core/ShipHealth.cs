using UnityEngine;

public class ShipHealth : MonoBehaviour
{
    [Header("Datos del Barco")]
    public ShipDatabase shipData;

    [Header("Estado Actual")]
    [SerializeField] private float currentHealth;
    void Start()
    {
        if (shipData != null)
        {
            currentHealth = shipData.hpMaximum;
        }
        else
        {
            currentHealth = 400f;
        }
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"¡Impacto! {gameObject.name} recibió {damage} de daño. Vida restante: {currentHealth}");

        if (currentHealth <= 0)
        {
            Debug.Log($"¡El barco {gameObject.name} ha sido hundido!");
            gameObject.SetActive(false);
        }
    }
}
