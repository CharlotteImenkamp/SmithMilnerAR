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

    private List<GameObject> instantiatedButtons;
    public List<DataManager.Data> chosenSet; 
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

        instantiatedButtons = new List<GameObject>();
        chosenSet = new List<DataManager.Data>();
    }


    public void MakeToggleList(string userSet)
    {
        try
        {
            ClearList();

            if (userSet == "incompleteSet")
            {
                chosenSet = DataManager.Instance.IncompleteUserData;
            }
            else if (userSet == "completeSet")
            {
                chosenSet = DataManager.Instance.CompleteUserData;
            }
            else if (userSet == "newSet")
            {
                chosenSet = DataManager.Instance.NewUserData;
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

                itemInstance.GetComponent<ButtonConfigHelper>().MainLabelText = "UserID " + chosenSet[i].UserData.UserID.ToString() +
                                                                                " SetType " + chosenSet[i].UserData.set.ToString();
                
                itemInstance.SetActive(true);
                newToggleList[i] = itemInstance.GetComponent<Interactable>();

                instantiatedButtons.Add(itemInstance);
            }

            gridObjectCollection.UpdateCollection();

            if(newToggleList.Length > 0)
            {
                toggleCollection.ToggleList = newToggleList;
            }
        }
        catch (System.Exception)
        {
            Debug.LogWarning("Custom Toggle List Populator not able to create a list." +
                "Check, if it is inactive at start"); 
        }

        
    }

    private void ClearList()
    {
        if(instantiatedButtons != null)
        {
            foreach (GameObject item in instantiatedButtons)
            {
                Destroy(item);
            }
        }

        gridObjectCollection.UpdateCollection();
        // toggleCollection.ToggleList
        instantiatedButtons = new List<GameObject>();
        chosenSet = new List<DataManager.Data>();
    }
}
