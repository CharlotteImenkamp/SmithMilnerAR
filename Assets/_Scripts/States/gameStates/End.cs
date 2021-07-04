using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End : IState
{
    public void Enter()
    {
        // Call Submanagers
        Debug.Log("End::Enter()");
        var SubManagers = GameManager.Instance.AttachedSubManagers;
        foreach (SubManager subManager in SubManagers)
        {
            subManager.OnGameStateEntered(this.ToString());
        }

        // Save Data
        GameManager.Instance.SaveGeneralSettings();
        Debug.Log("General Settings saved.");

        // End Game
        Application.Quit(); 
    }

    /// <summary>
    /// Not called, cause game quits on enter
    /// </summary>
    public void Execute()
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Not called, cause game quits on enter
    /// </summary>
    public void Exit()
    {
        throw new System.NotImplementedException();
    }
}
