using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TrajectoryPredictor : MonoBehaviour
{
    [Header("Referencias")]
    public Transform firePoint;
    public ShootController shootController; //ShootController

    [Header("Configuracion de la Curva")]
    public int pointsCount = 30; 
    public float timeStep = 0.1f; 

    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.CanExecuteShot(shootController.ownerShip))
        {
            lineRenderer.enabled = true;
            DrawTrajectory();
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    void DrawTrajectory()
    {
        lineRenderer.positionCount = pointsCount;

        Vector3 currentPos = firePoint.position;
        Vector3 currentVel = firePoint.forward * shootController.impulseForce;

        for (int i = 0; i < pointsCount; i++)
        {
            lineRenderer.SetPosition(i, currentPos);

            // Simulamos la física
            currentPos += currentVel * timeStep;

            // Simulamos la gravedad
            currentVel += Physics.gravity * timeStep;

            // Raycast para detener la línea si choca contra algo antes de tiempo
            if (Physics.Raycast(currentPos, currentVel.normalized, out RaycastHit hit, 0.5f))
            {
                lineRenderer.positionCount = i + 1; // Cortamos la línea al chocar
                break;
            }
        }
    }
}
