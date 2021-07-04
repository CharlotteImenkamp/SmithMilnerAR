/// <summary>
/// Interface to define state methods
/// </summary>
public interface IState
{
    void Enter();

    void Execute();

    void Exit();

}
