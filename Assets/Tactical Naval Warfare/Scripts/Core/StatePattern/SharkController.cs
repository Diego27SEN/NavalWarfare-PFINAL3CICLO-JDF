
using UnityEngine;
using UnityEngine.AI;

public class SharkController : MonoBehaviour
{
    public float roamRadius = 100f;
    public float waitTime = 2f;

    private NavMeshAgent agent;
    private float timer;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        agent.updateRotation = false;
        SetRandomDestination();
    }

    private void Update()
    {


        if (agent.velocity.sqrMagnitude > 0.1f)
        {
            Vector3 dir = agent.velocity.normalized;
            Quaternion targetRot = Quaternion.LookRotation(dir);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 5f * Time.deltaTime);
        }

        if (!agent.pathPending && agent.remainingDistance <= 1f)
        {
            Debug.Log("Llegó al destino");

            timer += Time.deltaTime;

            if (timer >= waitTime)
            {
                Debug.Log("Buscando nuevo destino");

                SetRandomDestination();

                timer = 0f;
            }
        }
    }

    private void SetRandomDestination()
    {
        Vector3 randomPoint =
            transform.position + Random.insideUnitSphere * roamRadius;

        randomPoint.y = transform.position.y;

        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, roamRadius, NavMesh.AllAreas))
        {
              agent.SetDestination(hit.position);
        }
        
    }
}