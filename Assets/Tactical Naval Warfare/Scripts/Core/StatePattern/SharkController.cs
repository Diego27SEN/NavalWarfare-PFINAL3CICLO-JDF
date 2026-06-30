using UnityEngine;
using UnityEngine.AI;

public class SharkController : MonoBehaviour
{
    [Header("config")]
    public Transform centerPoint;
    public float orbitRadius = 500f;
    public float orbitSpeed = 20f; // grados por segundo

    [Header("Combat")]
    public float damage = 15f;

    private NavMeshAgent agent;
    private float currentAngle;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;

        orbitSpeed = 4f;
        orbitRadius = 500f;
        currentAngle = Random.Range(0f, 360f);
    }

    private void Update()
    {
        OrbitAroundCenter();
    }

    private void OrbitAroundCenter()
    {
        currentAngle += orbitSpeed * Time.deltaTime;

        Quaternion rotation = Quaternion.Euler(0f, currentAngle, 0f);
        Vector3 offset = rotation * Vector3.forward * orbitRadius;
        Vector3 targetPosition = centerPoint.position + offset;

        agent.Warp(targetPosition);
        transform.rotation = rotation * Quaternion.Euler(0f, 90f, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        ShipHealth health = other.GetComponentInParent<ShipHealth>();
        if (health != null)
        {
            health.TakeDamage(damage);
        }
    }

    private void OnDrawGizmos()
    {
        if (centerPoint == null) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(centerPoint.position, orbitRadius);
    }
}