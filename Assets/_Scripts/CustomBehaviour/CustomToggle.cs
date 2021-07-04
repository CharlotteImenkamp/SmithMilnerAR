using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI; 

public class CustomToggle : MonoBehaviour
{
    InteractableToggleCollection toggleCollection;
    Interactable[] newList; 

    // Start is called before the first frame update
    void Start()
    {
       
    }

    public void OnGenerateDynamicList()
    {
        toggleCollection = gameObject.AddComponent<InteractableToggleCollection>(); // GetComponent<InteractableToggleCollection>();
        newList = gameObject.GetComponentsInChildren<Interactable>();
        toggleCollection.ToggleList = newList;

    }
}
