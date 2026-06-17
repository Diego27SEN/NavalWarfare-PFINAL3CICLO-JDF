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
        }
    }

    private void OnDisable()
    {
        if (bulletCam != null)
        {
            bulletCam.Target.TrackingTarget = null;
            bulletCam.Priority = 0;
        }
        OnBulletFinished?.Invoke(); // avisa que terminó
    }
}