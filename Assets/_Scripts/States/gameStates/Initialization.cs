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
        GameManager.Instance.debugText.text = "Initialization::Enter()"; 
        Debug.Log("Initialization::Enter()");
        var SubManagers = GameManager.Instance.AttachedSubManagers; 

        foreach (SubManager subManager in SubManagers)
        {
            subManager.OnGameStateEntered(this.ToString());  
        }
    }

    public void Execute()
    {
    }

    public void Exit()
    {
        GameManager.Instance.debugText.text = "Initialization::Exit()"; 
        
        Debug.Log("Initialization::Exit()");

        var SubManagers = GameManager.Instance.AttachedSubManagers;
        foreach (SubManager subManager in SubManagers)
        {
            subManager.OnGameStateLeft(this.ToString());
        }
    }
}
