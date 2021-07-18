using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UserInputHelper : MonoBehaviour
{
    #region serialized
    [SerializeField]
    private TextMeshPro idObj;
    [SerializeField]
    private TextMeshPro setObj;
    [SerializeField]
    private CustomScrollableListPopulator newObjectList;

    #endregion serialized

    #region private params
    private string userID;
    private string userSet;

    private string idText;
    private string setText;
    private UserSettingsData.userSet set;
    private DataManager.Data newData;

    #endregion private params

    #region public params
    public string UserID { get => userID; set => userID = value; }
    public UserSettingsData.userSet Set { get => set; set => set = value; }
    public DataManager.Data NewData { get => newData; set => newData = value; }

    #endregion public params


    // Start is called before the first frame update
    void Start()
    {
        idText = "User ID:";
        setText = "User Set: ";
        idObj.text = idText;

        userID = "";
        userSet = "";

        set = new UserSettingsData.userSet();
        newData = new DataManager.Data(); 

    }


    public void GetKeyInput(GameObject obj)
    {
        string text = obj.GetComponent<ButtonConfigHelper>().MainLabelText; 

        if(text == "1") //\TODO besser?
        {
             userID += "1"; 
        }
        else if(text == "2")
        {
            userID += "2";
        }
        else if (text == "3")
        {
            userID += "3";
        }
        else if (text == "4")
        {
            userID += "4";
        }
        else if (text == "5")
        {
            userID += "5";
        }
        else if (text == "6")
        {
            userID += "6";
        }
        else if (text == "7")
        {
            userID += "7";
        }
        else if (text == "8")
        {
            userID += "8";
        }
        else if (text == "9")
        {
            userID += "9";
        }
        else if(text == "Clear")
        {
            userID = ""; 
        }
        else if (text == "AG")
        {
            userSet = "AG";
            set = UserSettingsData.userSet.AG;
        }
        else if (text == "JG")
        {
            userSet = "JG";
            set = UserSettingsData.userSet.JG; 
        }
        else if (text == "AE")
        {
            userSet = "AE";
            set = UserSettingsData.userSet.AK; 
        }

        idObj.text = idText + userID;
        setObj.text = setText + userSet; 
    }

    /// <summary>
    /// called on apply button
    /// </summary>
    public void GenerateNewData()
    {
        UserSettingsData userData = new UserSettingsData(UserID, Set, GameManager.Instance.UpdateRate);
        ObjectData objData = newObjectList.GetInstantiatedObjects(); 

        DataManager.Instance.SetAndSaveNewSettings(new DataManager.Data(objData, userData));
    }
}
