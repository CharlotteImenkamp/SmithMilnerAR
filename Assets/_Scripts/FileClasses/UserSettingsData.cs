using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class UserSettingsData
{
    #region constructors
    public UserSettingsData(float updateRate, string userId, userSet set, gameState state)
    {
        this.updateRate = updateRate;
        this.UserID = userId;
        this.set = set;
        this.state = state;
    }

    public UserSettingsData() { }

    public UserSettingsData(string userID, userSet set)
    {
        UserID = userID;
        this.set = set;
    }

    #endregion  constructors

    #region parameters

    // User
    public string UserID;
    public userSet set;
    public gameState state;
    public enum userSet { JG, AG, AK };
    public enum gameState { locationsCompleted, pricesCompleted, None } // \TODO ändern in GameType


    // Saving
    public float updateRate;
    #endregion 

}



