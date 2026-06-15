using UnityEngine;

public class DeadState : IState
{
    private StateMachine stateMachine;
    private SubmarineBombController submarineBomb;
    private float destroyDelay = 1f;

    public DeadState(StateMachine stateMachine, SubmarineBombController submarineBomb)
    {
        this.stateMachine = stateMachine;
        this.submarineBomb = submarineBomb;
    }

    public void Enter()
    {
        submarineBomb.Agent.ResetPath();
        submarineBomb.Agent.enabled = false;
        Collider col = submarineBomb.GetComponent<Collider>();
        if (col != null) col.enabled = false;
        Object.Destroy(submarineBomb.gameObject, destroyDelay);
        Debug.Log("Mina destruida");
    }

    public void Update() { }

    public void Exit()
    {
        Debug.Log("Mina eliminada");
    }
}