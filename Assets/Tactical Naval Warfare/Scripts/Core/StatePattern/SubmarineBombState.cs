using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;

public class SubmarineBombController : MonoBehaviour
{
    [FoldoutGroup("Detection")]
    public float explosionRadius = 3f;
    public LayerMask shipLayer;

    [FoldoutGroup("Combat")]
    public float damage = 35f;
    public float explosionDuration = 0.8f;
    public GameObject explosionVFX;

    [FoldoutGroup("Drift")]
    public float driftSpeed = 1.2f;
    public float driftChangeTime = 3f;
    public float driftRange = 8f;

    [FoldoutGroup("HealthPoints")]
    public float MaxHealth = 50f;
    public float CurrentHealth;

    public NavMeshAgent Agent;
    public StateMachine stateMachine;
    public DriftState driftState;
    public ExplodeState explodeState;
    public DeadState deadState;

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        CurrentHealth = MaxHealth;
        stateMachine = new StateMachine();

        driftState = new DriftState(this, stateMachine);
        explodeState = new ExplodeState(this, stateMachine);
        deadState = new DeadState(stateMachine, this);

        stateMachine.Initialize(driftState);
    }

    void Update()
    {
        stateMachine.Update();
    }

    public void TakeDamage(float amount)
    {
        CurrentHealth -= amount;
        if (CurrentHealth <= 0)
            stateMachine.ChangeState(deadState);
    }

    void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & shipLayer) != 0)
        {
            if (stateMachine.CurrentState == driftState)
                stateMachine.ChangeState(explodeState);
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, driftRange);
    }
}