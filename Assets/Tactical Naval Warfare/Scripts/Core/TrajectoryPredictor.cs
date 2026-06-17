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
        if (impactMarker != null)
        {
            impactMarker.SetActive(false);
        }

        lineRenderer.positionCount = pointsCount;

        Vector3 currentPos = firePoint.position;
        Vector3 currentVel = firePoint.forward * shootController.impulseForce;

        for (int i = 0; i < pointsCount; i++)
        {
            lineRenderer.SetPosition(i, currentPos);

            Vector3 nextPos = currentPos + currentVel * timeStep;

            // Raycast para detener la línea si choca contra algo antes de tiempo
            if (Physics.Linecast(currentPos, nextPos.normalized, out RaycastHit hit, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                lineRenderer.positionCount = i + 2;
                lineRenderer.SetPosition(i + 1, hit.point);

                if (impactMarker != null)
                {
                    impactMarker.transform.position = hit.point + (hit.normal * 0.05f);
                    impactMarker.transform.up = hit.normal; // Para que se acueste sobre la superficie
                    impactMarker.SetActive(true);
                }
                break;
            }
  
            currentPos = nextPos;
            currentVel += Physics.gravity * timeStep;
        }
    }
}
