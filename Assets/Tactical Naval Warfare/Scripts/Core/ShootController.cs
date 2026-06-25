using Sirenix.OdinInspector;
using UnityEngine;
using DG.Tweening;
using System.Collections;


public class ShootController : MonoBehaviour
{
    [Header("Referencias de Disparo")]
    [Tooltip("Script PlayerGame")]
    public PlayerGameDatabase ownerShip;

    [Tooltip("El objeto vacío en la punta del cañón")]
    public Transform firePoint;

    [Tooltip(" ID del PoolConfigData de la bala")]
    public float impulseForce = 100.00f;

    public string poolId = "CannonBall";


    [Header("Animación de Retroceso")]
    [Tooltip("El modelo 3D del cañón que se moverá hacia atrás")]
    public Transform cannonModel;
    public float recoilDistance = 0.9f; // Qué tanto se hace para atrás
    public float recoilDuration = 0.1f; // Tiempo que tarda en ir hacia atrás
    public float returnDuration = 0.5f; // Tiempo que tarda en volver a su lugar

    [Tooltip("Curva para el impacto inicial")]
    public AnimationCurve recoilCurve; // Curva de Animación

    [SerializeField] private TurnManager turnManager;
    private bool isBallInFlight = false;


    [Button("Disparar Cañón", ButtonSizes.Large)]
    public void FireCannon()
    {

        if (isBallInFlight) return; // bloquea si ya hay una bala volando

        if (!GameManager.Instance.hasRolledDice)
        {
            Debug.LogWarning($"[ShootController] No se puede disparar: El jugador actual ({GameManager.Instance.currentPlayer?.PlayerID}) no ha lanzado el dado.");
            return;
        }

        if (GameManager.Instance.remainingShots <= 0)
        {
            Debug.LogWarning($"[ShootController] No se puede disparar: Al jugador actual ({GameManager.Instance.currentPlayer?.PlayerID}) se le agotaron los disparos.");
            return;
        }

        if (ownerShip != GameManager.Instance.currentPlayer)
        {
            ownerShip = GameManager.Instance.currentPlayer;
        }

        
        GameObject ball = PoolManager.Instance?.GetObject(poolId, firePoint.position, firePoint.rotation); //solicitamos la bala al PoolManager
        if (ball == null) return;

       
        Rigidbody rb = ball.GetComponent<Rigidbody>(); //busca el componente Rigidbody de la bala
        if (rb == null) return;

        // Fisicas
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.AddForce(firePoint.forward * impulseForce, ForceMode.Impulse);


        isBallInFlight = true;

        if (turnManager != null)
        {
            turnManager.PauseTimer(); // pausa el timer
        }

        GameManager.Instance.RegisterShotEfectuated();

        ApplyRecoilAnimation();
    }

    private void ApplyRecoilAnimation()
    {
        if (cannonModel == null) return;
        cannonModel.DOKill();
        float originalZ = 0f; 

        // Movimiento hacia atrás
        cannonModel.DOLocalMoveZ(originalZ - recoilDistance, recoilDuration)
            .SetEase(recoilCurve)
            .OnComplete(() =>
            {
                //  Movimiento de regreso al punto original
                cannonModel.DOLocalMoveZ(originalZ, returnDuration).SetEase(Ease.OutElastic);
            });
    }
    private void OnEnable()
    {
        BulletCameraController.OnBulletFinished += StartCameraDelay;
    }

    private void OnDisable()
    {
        BulletCameraController.OnBulletFinished -= StartCameraDelay;
    }

    private void StartCameraDelay()
    {
        StartCoroutine(CameraReturnDelay());
    }

    private IEnumerator CameraReturnDelay()
    {
        // Congela la cámara inmediatamente al impactar
        if (BulletCameraController.bulletCam != null)
            BulletCameraController.bulletCam.Target.TrackingTarget = null;

        yield return new WaitForSeconds(2f);

        if (BulletCameraController.bulletCam != null)
            BulletCameraController.bulletCam.Priority = 0;

        isBallInFlight = false;
        turnManager?.ResumeTimer();
    }
}
