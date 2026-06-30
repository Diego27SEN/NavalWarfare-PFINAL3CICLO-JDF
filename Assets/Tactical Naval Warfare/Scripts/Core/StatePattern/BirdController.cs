using UnityEngine;
using UnityEngine.AI;

public class BirdController : MonoBehaviour
{
    [Header("COnfiguracion de ave")]
    public Transform centerPoint;
    public float roamRadius = 300f;
    public float waitTime = 2f;

    private NavMeshAgent agent;
    private float timer;
    private Vector3 startPosition;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        startPosition = transform.position;

        agent.speed = Random.Range(10f, 20f);
        waitTime = Random.Range(1.5f, 4f);
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

        BirdRotation();
    }

    private void BirdRotation()
    {
        if (agent.velocity.sqrMagnitude > 0.1f)
        {
            Vector3 dir = agent.velocity.normalized; // Normalizamos la dirección de la velocidad
            Quaternion targetRot = Quaternion.LookRotation(dir); // Creamos una rotación que mire hacia la dirección de la velocidad
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 5f * Time.deltaTime); // Interpolamos suavemente entre la rotación actual y la rotación objetivo
        }
    }

    private void SetRandomDestination()
    {
        Vector3 randomPoint = startPosition + Random.insideUnitSphere * roamRadius; // Genera un punto aleatorio dentro de un radio alrededor de la posición inicial
        randomPoint.y = startPosition.y;

        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, roamRadius, NavMesh.AllAreas)) 
        {
            agent.SetDestination(hit.position);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(startPosition, roamRadius);
    }
}