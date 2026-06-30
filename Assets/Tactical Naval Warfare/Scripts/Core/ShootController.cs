using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class ShootController : MonoBehaviour
{
    [Header("Referencias de Disparo")]
    [Tooltip("Script PlayerGame")]
    public PlayerGameDatabase ownerShip;

    [Tooltip("El objeto vacio en la punta del cañon")]
    public Transform firePoint;

    [Tooltip(" ID del PoolConfigData de la bala")]
    public float impulseForce = 100.00f;

    public string poolId = "CannonBall";

    [Header("Animacion de Retroceso")]
    [Tooltip("El modelo 3D del cañon que se movera hacia atras")]
    public Transform cannonModel;
    public float recoilDistance = 0.9f; // Qué tanto se hace para atrás
    public float recoilDuration = 0.1f; // Tiempo que tarda en ir hacia atrás
    public float returnDuration = 0.5f; // Tiempo que tarda en volver a su lugar

    [Tooltip("Curva para el impacto inicial")]
    public AnimationCurve recoilCurve; // Curva de Animación

    [SerializeField] private TurnManager turnManager;
    private bool isBallInFlight = false;

    [Button("Disparar Cañon", ButtonSizes.Large)]
    public void FireCannon()
    {
        // Referencia del barco
        ownerShip = GameManager.Instance.currentPlayer;
        if (ownerShip == null || ownerShip.SelectedShip == null) return;

        // Buscamos el arma Legendaria
        GameObject barcoActual = ownerShip.SelectedShip.Ship;
        ITemporaryWeapon armaEspecial = ownerShip.SelectedShip.Ship.GetComponent<ITemporaryWeapon>();

        if (armaEspecial != null)
        {
            armaEspecial.FireShot();
            return; 
        }

        if (isBallInFlight || !GameManager.Instance.CanExecuteShot(ownerShip))
        {
            Debug.LogWarning("[ShootController] Disparo normal bloqueado.");
            return;
        }

        EjecutarDisparoNormal();
    }
    private void EjecutarDisparoNormal()
    {
        GameObject ball = PoolManager.Instance?.GetObject(poolId, firePoint.position, firePoint.rotation); //solicitamos la bala al PoolManager
            if (ball == null) return;

        Rigidbody rb = ball.GetComponent<Rigidbody>(); //busca el componente Rigidbody de la bala
            if (rb == null) return;

        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("CannonBall"),LayerMask.NameToLayer("Ship"),true);
        // Fisicas
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.AddForce(firePoint.forward * impulseForce, ForceMode.Impulse);

        isBallInFlight = true;
        turnManager?.PauseTimer();

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
