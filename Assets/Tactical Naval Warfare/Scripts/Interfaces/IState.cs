
public interface IState
{
    //->se llama UNA VEZ
    void Enter();

    //-> se llamada CADA FRAME 
    void Update();

    //-> se lllama UNA VEZ cuando el estado esta apunto de cambiar
    void Exit();
}