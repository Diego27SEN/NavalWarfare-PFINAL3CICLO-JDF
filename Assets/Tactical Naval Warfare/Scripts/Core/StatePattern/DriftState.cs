using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class DriftState : IState
{
    private SubmarineBombState submarineBomb;
    private StateMachine sm;
    private NavMeshAgent agent;
    private float timer;

    public DriftState(SubmarineBombState submarineBomb, StateMachine sm)
    {
        this.submarineBomb = submarineBomb;
        this.sm = sm;
    }

    public void Enter()
    {
        agent = submarineBomb.GetComponent<NavMeshAgent>();
        agent.enabled = true;
        timer = 0f;
        PickNewDestination();
    }

    public void Update()
    {
        timer += Time.deltaTime;

        // Cuando llega al destino o pasa el timer, elige otro punto
        if (timer >= submarineBomb.driftChangeTime || !agent.pathPending && agent.remainingDistance < 0.5f)
        {
            timer = 0f;
            PickNewDestination();
        }
    }

    public void PhysicsUpdate() { }

    public void Exit()
    {
        agent.ResetPath();
        agent.enabled = false;
    }

    private void PickNewDestination()
    {
        // Punto aleatorio alrededor de la posición actual
        Vector3 randomDir = Random.insideUnitSphere * submarineBomb.driftRange;
        randomDir += submarineBomb.transform.position;
        randomDir.y = submarineBomb.transform.position.y; // mantiene la profundidad

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDir, out hit, submarineBomb.driftRange, NavMesh.AllAreas))
            agent.SetDestination(hit.position);
    }
}