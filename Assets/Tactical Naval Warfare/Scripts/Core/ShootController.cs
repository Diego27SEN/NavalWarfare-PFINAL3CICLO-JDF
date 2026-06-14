using Sirenix.OdinInspector;
using UnityEngine;
using DG.Tweening;

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

    [Header("Animación de Retroceso")]
    [Tooltip("El modelo 3D del cañón que se moverá hacia atrás")]
    public Transform cannonModel;
    public float recoilDistance = 0.9f; // Qué tanto se hace para atrás
    public float recoilDuration = 0.1f; // Tiempo que tarda en ir hacia atrás
    public float returnDuration = 0.5f; // Tiempo que tarda en volver a su lugar

    [Tooltip("Curva para el impacto inicial")]
    public AnimationCurve recoilCurve; // Curva de Animación

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

        ApplyRecoilAnimation();
    }
    private void ApplyRecoilAnimation()
    {
        if (cannonModel == null) return;
        cannonModel.DOKill();
        float originalZ = 0f; 

        // 1. Movimiento hacia atrás
        cannonModel.DOLocalMoveZ(originalZ - recoilDistance, recoilDuration)
            .SetEase(recoilCurve)
            .OnComplete(() =>
            {
                // 2. Movimiento de regreso al punto original
                cannonModel.DOLocalMoveZ(originalZ, returnDuration).SetEase(Ease.OutElastic);
            });
    }
}
