using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class LogData : IState
{
    public void Enter()
    {
        Debug.Log("LogData::Enter"); 
    }

    public void Execute()
    {
    }

    public void Exit()
    {
        Debug.Log("LogData::Exit");
    }
}

