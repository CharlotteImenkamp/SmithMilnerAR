﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriceEstimation : IState
{
    public void Enter()
    {
        Debug.Log("PriceEstimation::Enter()");
    }

    public void Execute()
    {
        throw new System.NotImplementedException();
    }

    public void Exit()
    {
        Debug.Log("PriceEstimation::Exit()");
    }
}