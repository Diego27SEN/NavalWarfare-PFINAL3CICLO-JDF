public class StateMachine
{
    private IState currentState;
    public IState CurrentState => currentState; // agrega esta línea

    public void Initialize(IState startingState)
    {
        currentState = startingState;
        currentState.Enter();
    }

    public void ChangeState(IState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void Update()
    {
        currentState.Update();
    }
}