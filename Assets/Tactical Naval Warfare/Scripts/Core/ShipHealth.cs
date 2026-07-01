using UnityEngine;
using TMPro;
using UnityEngine.Rendering;

public class ShipHealth : MonoBehaviour, IDamageable
{
    [Header("Datos del Barco")]
    public ShipDatabase shipData;
    public TextMeshProUGUI healthText;

    [Header("Estado Actual")]
    public float currentHealth;
    [SerializeField] private SoundManager soundManager;


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

        if (soundManager != null)
        {
            soundManager.PlaySFX(1);
        }
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }
}
