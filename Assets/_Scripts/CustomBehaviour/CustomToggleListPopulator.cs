using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;

public class CustomToggleListPopulator : MonoBehaviour
{

    [SerializeField]
    [Tooltip("Object to duplicate in ScrollCollection")]
    private GameObject dynamicItem;

    [SerializeField]
    [Tooltip("Parent Transform for Collection")]
    private GameObject parentTransform;

    /// <summary>
    /// Object to duplicate/>. 
    /// </summary>
    public GameObject DynamicItem
    {
        get { return dynamicItem; }
        set { dynamicItem = value; }
    }

    private List<GameObject> ItemsInScene;
    public List<userSettingsData> chosenSet; 
    private GridObjectCollection gridObjectCollection = null;
    private InteractableToggleCollection toggleCollection = null;

    private void Start()
    {
        //  Check components
        if (gridObjectCollection == null)
        {
            gridObjectCollection = parentTransform.GetComponent<GridObjectCollection>();
        }

        if (toggleCollection == null)
        {
            toggleCollection = parentTransform.GetComponent<InteractableToggleCollection>();
        }

        // Grid Object Collection to organize Buttons
        if (gridObjectCollection == null)
        {
            gridObjectCollection = parentTransform.AddComponent<GridObjectCollection>();
            gridObjectCollection.CellWidth = 0.15f;
            gridObjectCollection.CellHeight = 0.03f;
            gridObjectCollection.SurfaceType = ObjectOrientationSurfaceType.Plane;
            gridObjectCollection.Layout = LayoutOrder.ColumnThenRow;
            gridObjectCollection.Columns = 1;
            gridObjectCollection.Anchor = LayoutAnchor.UpperLeft;
        }

        // Toggle Collection 
        if (toggleCollection == null)
        {
            toggleCollection = parentTransform.AddComponent<InteractableToggleCollection>();
        }
        ItemsInScene = new List<GameObject>();
        chosenSet = new List<userSettingsData>();
    }


    public void MakeToggleList(string userSet)
    {
        ClearList(); 
        
        if(userSet == "incompleteSet")
        {
            chosenSet = DataManager.Instance.IncompleteUserData;
        }
        else if(userSet == "completeSet")
        {
            chosenSet = DataManager.Instance.CompleteUserData;
        }
        else
        {
            throw new System.Exception("CustomToggle List Populator, incorrect input"); 
        }

        Interactable[] newToggleList = new Interactable[chosenSet.Count]; 

        // Generate Objects
        for (int i = 0; i < chosenSet.Count; i++)
        {
            GameObject itemInstance = Instantiate(dynamicItem, parentTransform.transform);
            itemInstance.GetComponent<ButtonConfigHelper>().MainLabelText = "UserID " + chosenSet[i].UserID.ToString() +
                                                                            " SetType " + chosenSet[i].set.ToString() ; 
            itemInstance.SetActive(true);
            newToggleList[i] = itemInstance.GetComponent<Interactable>();

            ItemsInScene.Add(itemInstance); 
        }

        gridObjectCollection.UpdateCollection();
        toggleCollection.ToggleList = newToggleList; 
    }

    private void ClearList()
    {
        foreach (GameObject item in ItemsInScene)
        {
            Destroy(item); 
        }
    }
}
