
public interface ISubManager 
{

    void Initialize();
    void Reset();
    void OnGameStateEntered(string newState);

    void OnGameStateLeft(string oldState); 
}
