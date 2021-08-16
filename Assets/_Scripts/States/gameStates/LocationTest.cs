using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationTest : IState
{
    public void Enter()
    {
        GameManager.Instance.debugText.text = "LocationTest::Enter()";

        Debug.Log("LocationTest::Enter()");
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
        GameManager.Instance.debugText.text = "LocationTest::Exit()";

        var SubManagers = GameManager.Instance.AttachedSubManagers;
        foreach (SubManager subManager in SubManagers)
        {
            subManager.OnGameStateLeft(this.ToString());
        }
    }
}
