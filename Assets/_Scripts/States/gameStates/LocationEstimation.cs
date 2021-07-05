using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationEstimation : IState
{
    public void Enter()
    {
        GameManager.Instance.debugText.text = "LocationEstimation::Enter()"; 
        Debug.Log("LocationEstimation::Enter()");
        var SubManagers = GameManager.Instance.AttachedSubManagers;
        foreach (SubManager subManager in SubManagers)
        {
            subManager.OnGameStateEntered(this.ToString());
        }
    }

    public void Execute()
    {
        throw new System.NotImplementedException();
    }

    public void Exit()
    {
        GameManager.Instance.debugText.text = "LocationEstimation::Exit()";

        Debug.Log("LocationEstimation::Exit()");
        var SubManagers = GameManager.Instance.AttachedSubManagers;
        foreach (SubManager subManager in SubManagers)
        {
            subManager.OnGameStateLeft(this.ToString());
        }
    }
}
