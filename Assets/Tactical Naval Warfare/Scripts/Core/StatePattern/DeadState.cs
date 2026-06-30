using Unity.VisualScripting;
using UnityEngine;

public class DeadState : IState
{
    private StateMachine stateMachine;
    private SubmarineBombState submarineBomb;
    private float destroyDelay = 1f;

    public DeadState(StateMachine stateMachine, SubmarineBombState submarineBomb)
    {
        this.stateMachine = stateMachine;
        this.submarineBomb = submarineBomb;
    }

    public void Enter()
    {
        if (submarineBomb.Agent != null && submarineBomb.Agent.isActiveAndEnabled && submarineBomb.Agent.isOnNavMesh)
        {
            submarineBomb.Agent.ResetPath();
            submarineBomb.Agent.enabled = false;
        }

        Collider col = submarineBomb.GetComponent<Collider>();
        if (col != null) col.enabled = false;

        Object.Destroy(submarineBomb.gameObject, destroyDelay);
        Debug.Log("Destruyendose");
    }

    public void Update() { }

    public void Exit()
    {
        Debug.Log("Mina eliminada");
    }
}