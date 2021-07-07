using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class userSettingsData
{
    #region constructors
    public userSettingsData(float updateRate, int userId, userSet set, gameState state)
    {
        this.updateRate = updateRate;
        this.UserID = userId;
        this.set = set;
        this.state = state;
    }

    public userSettingsData() { }
    #endregion  constructors

    #region parameters

    // User
    public int UserID;
    public userSet set;
    public gameState state;
    public enum userSet { JG, AG, AK };
    public enum gameState { locationsCompleted, pricesCompleted, None }


    // Saving
    public float updateRate;
    #endregion 

}



