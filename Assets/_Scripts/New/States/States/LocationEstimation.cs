using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationEstimation : IState
{
    public void Enter()
    {
        Debug.Log("LocationEstimation::Enter()");
    }

    public void Execute()
    {
        throw new System.NotImplementedException();
    }

    public void Exit()
    {
        Debug.Log("LocationEstimation::Exit()");    
    }
}
