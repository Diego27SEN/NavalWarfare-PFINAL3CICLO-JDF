using UnityEngine;

public abstract class BasePowerUp : MonoBehaviour
{
    // Polimorfismo: Método que los hijos DEBEN definir
    protected abstract void ApplyEffect(GameObject ship);

    // Herencia: Lógica común para detectar al barco y destruirse
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ship"))
        {
            // Llama al efecto polimórfico
            ApplyEffect(other.gameObject);
            
            // Lógica compartida: Feedback y destrucción
            Debug.Log("Power-Up recogido.");
            Destroy(gameObject);
        }
    }
}
