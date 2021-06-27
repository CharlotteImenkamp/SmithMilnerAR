using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gamestate, which is called on GameManager::Start()
/// </summary>
public class Initialization : IState
{
    /// <summary>
    /// Initialize all subManagers
    /// </summary>
    public void Enter()
    {
        Debug.Log("Initialization::Enter()");
        var SubManagers = GameManager.Instance.AttachedSubManagers; 
        foreach (ISubManager subManager in SubManagers)
        {
            subManager.Initialize(); 
        }
    }

    public void Execute()
    {
    }

    public void Exit()
    {
        Debug.Log("Initialization::Exit()");

        var SubManagers = GameManager.Instance.AttachedSubManagers;
        foreach (ISubManager subManager in SubManagers)
        {
            subManager.OnGameStateLeft(this.ToString());
        }
    }
}
