using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : IState
{
    public void Enter()
    {
        GameManager.Instance.debugText.text = "Pause::Enter()";

        Debug.Log("Pause::Enter()");
        var SubManagers = GameManager.Instance.AttachedSubManagers;
        foreach (SubManager subManager in SubManagers)
        {
            subManager.OnGameStateEntered(this.ToString());
        }

        // Save Data
        DataFile.Override<ApplicationData>(GameManager.Instance.generalSettings, GameManager.Instance.mainFolder, "generalSettings");
    }

    public void Execute()
    {
        throw new System.NotImplementedException();
    }

    public void Exit()
    {
        Debug.Log("Pause::Exit()");
        GameManager.Instance.debugText.text = "Pause::Exit()";

        var SubManagers = GameManager.Instance.AttachedSubManagers;
        foreach (SubManager subManager in SubManagers)
        {
            subManager.OnGameStateLeft(this.ToString());
        }
    }
}
