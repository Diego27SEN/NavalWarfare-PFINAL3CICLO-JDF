using UnityEngine;

public abstract class BasePowerUp : MonoBehaviour
{
    protected abstract void ApplyEffect(GameObject ship);

    // Logica común para detectar al barco y destruirse
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ship"))
        {
            // Llama al efecto polimórfico
            ApplyEffect(other.gameObject);
            
            // Logica compartida: Feedback y destrucción
            Debug.Log("Power-Up recogido.");
            Destroy(gameObject);
        }
    }
}
