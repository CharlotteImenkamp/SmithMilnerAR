using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriceTest : IState
{
    public void Enter()
    {
        Debug.Log("PriceTest::Enter()");
    }

    public void Execute()
    {
        throw new System.NotImplementedException();
    }

    public void Exit()
    {
        Debug.Log("PriceTest::Exit()");
    }
}
