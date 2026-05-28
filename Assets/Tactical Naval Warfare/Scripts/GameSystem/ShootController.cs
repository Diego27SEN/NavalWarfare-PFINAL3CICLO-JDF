using Sirenix.OdinInspector;
using UnityEngine;

public class ShootController : MonoBehaviour
{
    [Header("Referencias de Disparo")]
    public PlayerGame ownerShip;
    public Transform firePoint;
    public float impulseForce = 50.00f;

    [Button("Disparar Cañón", ButtonSizes.Large)]
    public void FireCannon()
    {
        // Verficar si es el turno de este jugador
        if (GameManager.Instance.currentPlayer != ownerShip)
        {
            Debug.LogWarning("No es tu turno.");
            return;
        }

        // Validar si ya lanzo el dado
        if (!GameManager.Instance.hasRolledDice)
        {
            Debug.LogWarning("Primero debes lanzar el dado.");
            return;
        }

        // Pedir la balla al PollManager
        GameObject ball = PoolManager.Instance.GetObject("CannonBall", firePoint.position, firePoint.rotation);

        if (ball != null)
        {
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                rb.AddForce(firePoint.forward * impulseForce, ForceMode.Impulse);
            }

            GameManager.Instance.RegisterShotEfectuated();
        }
    }
}
