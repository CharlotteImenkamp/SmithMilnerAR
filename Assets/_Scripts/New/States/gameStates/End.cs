using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End : IState
{
    public void Enter()
    {
        Debug.Log("End::Enter()");
    }

    public void Execute()
    {
        throw new System.NotImplementedException();
    }

    public void Exit()
    {
        Debug.Log("End::Exit()");
    }
}
