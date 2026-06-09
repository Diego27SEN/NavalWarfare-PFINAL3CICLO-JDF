using Sirenix.OdinInspector;
using UnityEngine;

public class ShootController : MonoBehaviour
{
    [Header("Referencias de Disparo")]
    [Tooltip("Script PlayerGame")]
    public PlayerGameDatabase ownerShip;

    [Tooltip("El objeto vacío en la punta del cañón")]
    public Transform firePoint;

    [Tooltip(" ID del PoolConfigData de la bala")]
    public float impulseForce = 50.00f;

    public string poolId = "CannonBall";
    [Button("Disparar Cañón", ButtonSizes.Large)]
    public void FireCannon()
    {
        // Le preguntamos si podemos disparar. Si no, cortamos aquí.
        if (GameManager.Instance == null || !GameManager.Instance.CanExecuteShot(ownerShip))
        {
            Debug.LogWarning($"El jugador {ownerShip?.PlayerID} no cumple las condiciones para disparar.");
            return;
        }

        // Pedimos la bala. Si el Pool falla, cortamos.
        GameObject ball = PoolManager.Instance?.GetObject(poolId, firePoint.position, firePoint.rotation);
        if (ball == null) return;

        // Buscamos el Rigidbody. Si no tiene, cortamos.
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        if (rb == null) return;

        // Fisicas
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.AddForce(firePoint.forward * impulseForce, ForceMode.Impulse);

        GameManager.Instance.RegisterShotEfectuated();
        
    }
}
