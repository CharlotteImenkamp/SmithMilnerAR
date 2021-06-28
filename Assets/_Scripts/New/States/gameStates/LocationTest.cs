using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationTest : IState
{
    public void Enter()
    {
        Debug.Log("LocationTest::Enter()");
    }

    public void Execute()
    {
        throw new System.NotImplementedException();
    }

    public void Exit()
    {
        Debug.Log("LocationTest::Exit()");
    }
}
