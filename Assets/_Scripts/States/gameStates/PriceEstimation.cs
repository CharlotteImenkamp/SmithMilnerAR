﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriceEstimation : IState
{
    public void Enter()
    {
        Debug.Log("PriceEstimation::Enter()");
        GameManager.Instance.debugText.text = "PriceEstimation::Enter()";

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
        GameManager.Instance.debugText.text = "PriceEstimation::Exit()";

        Debug.Log("PriceEstimation::Exit()");
        var SubManagers = GameManager.Instance.AttachedSubManagers;
        foreach (SubManager subManager in SubManagers)
        {
            subManager.OnGameStateLeft(this.ToString());
        }
    }
}