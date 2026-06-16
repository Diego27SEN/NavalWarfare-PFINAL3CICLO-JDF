using Unity.VisualScripting;
using UnityEngine;

public class ExplodeState : IState
{
    private SubmarineBombController submarineBomb;
    private StateMachine sm;
    private float timer;

    public ExplodeState(SubmarineBombController submarineBombController, StateMachine sm)
    {
        this.submarineBomb = submarineBombController;
        this.sm = sm;
    }

    public void Enter()
    {
        timer = 0f;


        Collider[] hits = Physics.OverlapSphere(submarineBomb.transform.position,submarineBomb.explosionRadius);

        foreach (var hit in hits)
        {
            // if (hit.TryGetComponent<ShipHealth>(out var health))
            // health.TakeDamage(submarineBombController.damage);
        }

        if (submarineBomb.explosionVFX != null)
            Object.Instantiate(submarineBomb.explosionVFX, submarineBomb.transform.position, Quaternion.identity);

        submarineBomb.GetComponent<MeshRenderer>().enabled = false;
    }

    public void Update()
    {
        timer += Time.deltaTime;
        if (timer >= submarineBomb.explosionDuration) sm.ChangeState(submarineBomb.deadState);
    }

    public void PhysicsUpdate() { }
    public void Exit() { }
}