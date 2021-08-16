using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriceTest : IState
{
    public void Enter()
    {
        GameManager.Instance.debugText.text = "PriceTest::Enter()";

        Debug.Log("PriceTest::Enter()");
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
        GameManager.Instance.debugText.text = "PriceTest::Exit()";

        var SubManagers = GameManager.Instance.AttachedSubManagers;
        foreach (SubManager subManager in SubManagers)
        {
            subManager.OnGameStateLeft(this.ToString());
        }
    }
}
