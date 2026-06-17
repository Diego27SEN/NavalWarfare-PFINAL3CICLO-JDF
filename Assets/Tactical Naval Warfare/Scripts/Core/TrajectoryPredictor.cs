using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TrajectoryPredictor : MonoBehaviour
{
    [Header("Referencias")]
    public Transform firePoint;
    public ShootController shootController; //ShootController

    [Header("Configuracion de la Curva")]
    public int pointsCount = 80; 
    public float timeStep = 0.1f;

    [Header("Marcador de Impacto")]
    [Tooltip("PrefabPunto")]
    public GameObject markerPrefab;
    private GameObject impactMarker;

    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();

        if (markerPrefab == null) return;

        impactMarker = Instantiate(markerPrefab);
        impactMarker.SetActive(false);
    }
    void Update()
    {
        // Si no existe GameManager, da false
        bool canShoot = GameManager.Instance?.CanExecuteShot(shootController.ownerShip) ?? false;

        lineRenderer.enabled = canShoot;

        // Si no es el turno, apagamos marcador y salimos
        if (!canShoot)
        {
            impactMarker?.SetActive(false);
            return;
        }

        DrawTrajectory();
    }
    private void DrawTrajectory()
    {
        lineRenderer.positionCount = pointsCount;
        Vector3 currentPos = firePoint.position;
        Vector3 currentVel = firePoint.forward * shootController.impulseForce;

        // Apagamos el marcador por defecto; si choca contra algo, lo encendemos abajo
        impactMarker?.SetActive(false);

        for (int i = 0; i < pointsCount; i++)
        {
            lineRenderer.SetPosition(i, currentPos);

            currentPos += currentVel * timeStep;
            currentVel += Physics.gravity * timeStep;

            // fisicas
            if (Physics.Raycast(currentPos, currentVel.normalized, out RaycastHit hit, 0.5f))
            {
                lineRenderer.positionCount = i + 1;

                if (impactMarker)
                {
                    impactMarker.transform.position = hit.point;
                    impactMarker.transform.up = hit.normal;
                    impactMarker.SetActive(true);
                }
                break;
            }
        }
    }
}
