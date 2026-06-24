using UnityEngine;
using Unity.Cinemachine;

public class BulletCameraController : MonoBehaviour
{
    public static CinemachineCamera bulletCam;
    public static System.Action OnBulletFinished; // evento global
    private void OnEnable()
    {
        if (bulletCam != null)
        {
            bulletCam.Target.TrackingTarget = transform;
            bulletCam.Priority = 100;
            bulletCam.Lens.NearClipPlane = 0.01f; // Ajusta la velocidad de seguimiento para que sea más rápida
        }
    }

    private void OnDisable()
    {
        OnBulletFinished?.Invoke(); // avisa que terminó
    }
}