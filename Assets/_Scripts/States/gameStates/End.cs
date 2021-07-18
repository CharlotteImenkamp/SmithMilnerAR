using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End : IState
{
    public void Enter()
    {
        // Save Data
        DataFile.Override<ApplicationData>(GameManager.Instance.GeneralSettings, GameManager.Instance.mainFolder, "generalSettings");


        // Call Submanagers
        GameManager.Instance.debugText.text = "End::Enter()";

        Debug.Log("End::Enter()");
        var SubManagers = GameManager.Instance.AttachedSubManagers;
        foreach (SubManager subManager in SubManagers)
        {
            subManager.OnGameStateEntered(this.ToString());
        }

        // debug
        GameManager.Instance.debugText.text = "General Settings saved.";
        Debug.Log("General Settings saved.");
    }


    public void Execute()
    {
        throw new System.NotImplementedException();
    }


    public void Exit()
    {
    }
}
