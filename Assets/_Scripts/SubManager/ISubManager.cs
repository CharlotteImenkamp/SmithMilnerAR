
public class SubManager 
{
    public virtual void Reset(){ }
    public virtual void OnGameStateEntered(string newState) { }

    public virtual void OnGameStateLeft(string oldState) { }
}
