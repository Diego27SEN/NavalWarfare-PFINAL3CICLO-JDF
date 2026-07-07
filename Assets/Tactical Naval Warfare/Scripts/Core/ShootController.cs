using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public class ShootController : MonoBehaviour
{
    [Header("Referencias de Disparo")]
    public PlayerGameDatabase ownerShip;
    public Transform firePoint;
    public float impulseForce = 100.00f;
    public string poolId = "CannonBall";

    [Header("Animacion de Retroceso")]
    public Transform cannonModel;
    public float recoilDistance = 0.9f;
    public float recoilDuration = 0.1f;
    public float returnDuration = 0.5f;
    public AnimationCurve recoilCurve;

    [SerializeField] private TurnManager turnManager;
    private bool isBallInFlight = false;

    [Button("Disparar Cañon", ButtonSizes.Large)]
    public void FireCannon()
    {
        ownerShip = GameManager.Instance.currentPlayer;
        if (ownerShip == null || ownerShip.SelectedShip == null) return;

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
        GameObject ball = PoolManager.Instance?.GetObject(poolId, firePoint.position, firePoint.rotation);
        if (ball == null) return;

        Rigidbody rb = ball.GetComponent<Rigidbody>();
        if (rb == null) return;

        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("CannonBall"), LayerMask.NameToLayer("Ship"), true);

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

        cannonModel.DOLocalMoveZ(originalZ - recoilDistance, recoilDuration)
            .SetEase(recoilCurve)
            .OnComplete(() =>
            {
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
        if (BulletCameraController.bulletCam != null)
            BulletCameraController.bulletCam.Target.TrackingTarget = null;

        yield return new WaitForSeconds(2f);

        if (BulletCameraController.bulletCam != null)
            BulletCameraController.bulletCam.Priority = 0;

        isBallInFlight = false;
        turnManager?.ResumeTimer();
    }
}