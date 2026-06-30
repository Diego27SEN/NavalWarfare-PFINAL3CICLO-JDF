using UnityEngine;
using UnityEngine.AI;

public class animalsController : MonoBehaviour
{
    [Header("Configuracion")]
    public float roamRadius = 30f;
    public float waitTime = 3f;

    private NavMeshAgent agent;
    private Vector3 startPosition;
    private float timer;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        startPosition = transform.position;

        agent.speed = Random.Range(2f, 4f);
        waitTime = Random.Range(2f, 5f);
    }

    private void Start()
    {
        SetRandomDestination();
    }

    private void Update()
    {
        if (!agent.pathPending && agent.remainingDistance <= 1f)
        {
            timer += Time.deltaTime;

            if (timer >= waitTime)
            {
                SetRandomDestination();
                timer = 0f;
            }
        }

        ElephantRotation();
    }

    private void ElephantRotation()
    {
        if (agent.velocity.sqrMagnitude > 0.1f)
        {
            Vector3 dir = agent.velocity.normalized; // Normalizamos la dirección de la velocidad
            Quaternion targetRot = Quaternion.LookRotation(dir);// Creamos una rotación que mire hacia la dirección de la velocidad
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 5f * Time.deltaTime); // Interpolamos suavemente entre la rotación actual y la rotación objetivo
        }
    }

    private void SetRandomDestination()
    {
        Vector3 randomPoint = startPosition + Random.insideUnitSphere * roamRadius;
        randomPoint.y = startPosition.y;

        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, roamRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, roamRadius);
    }
}