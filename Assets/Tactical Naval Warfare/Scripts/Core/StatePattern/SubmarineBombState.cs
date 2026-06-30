using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;

public class SubmarineBombState : MonoBehaviour
{
    [FoldoutGroup("Detection")]
    public float explosionRadius = 3f;

   

    [FoldoutGroup("Combat")]
    public float damage = 60f;
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
        if (stateMachine.CurrentState != driftState) return;

        ShipHealth health = other.GetComponentInParent<ShipHealth>();
        if (health != null)
        {
            health.TakeDamage(damage);
            stateMachine.ChangeState(explodeState);
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, driftRange);
    }
}