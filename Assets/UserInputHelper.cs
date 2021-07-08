using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UserInputHelper : MonoBehaviour
{
    public TextMeshPro idText;
    public TextMeshPro setText; 

    // Start is called before the first frame update
    void Start()
    {
        idText.text = "User ID:"; 
    }


    public void GetKeyInput(GameObject obj)
    {
        string text = obj.GetComponent<ButtonConfigHelper>().MainLabelText; 

        if(text == "1") //\TODO besser?
        {
            idText.text += "1"; 
        }
        else if(text == "2")
        {
            idText.text += "2";
        }
        else if (text == "3")
        {
            idText.text += "3";
        }
        else if (text == "4")
        {
            idText.text += "4";
        }
        else if (text == "5")
        {
            idText.text += "5";
        }
        else if (text == "6")
        {
            idText.text += "6";
        }
        else if (text == "7")
        {
            idText.text += "7";
        }
        else if (text == "8")
        {
            idText.text += "8";
        }
        else if (text == "9")
        {
            idText.text+= "9";
        }
        else if(text == "Clear")
        {
            idText.text = "User ID: "; 
        }
        else if (text == "AG")
        {
            setText.text = "User Set: AG";
        }
        else if (text == "JG")
        {
            setText.text = "User Set: JG";
        }
        else if (text == "AE")
        {
            setText.text = "User Set: AE";
        }

    }
}
