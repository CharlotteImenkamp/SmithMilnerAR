using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : IState
{
    public void Enter()
    {
        Debug.Log("Pause::Enter()");
    }

    public void Execute()
    {
        throw new System.NotImplementedException();
    }

    public void Exit()
    {
        Debug.Log("Pause::Exit()");
    }
}
