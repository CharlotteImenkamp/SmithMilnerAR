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

    /// <summary>
    /// TODO Struktur ändern, sodass auch GameObjects unter der Parent Transform erstellt werden. Wie in Custom Scrollable List Populator
    /// </summary>
    private void Start()
    {
        //  Check components
        if (gridObjectCollection == null)
        {
            gridObjectCollection = parentTransform.GetComponentInChildren<GridObjectCollection>();
        }

        if (toggleCollection == null)
        {
            toggleCollection = parentTransform.GetComponentInChildren<InteractableToggleCollection>();
        }

        // Grid Object Collection to organize Buttons
        if (gridObjectCollection == null)
        {
            gridObjectCollection = parentTransform.AddComponent<GridObjectCollection>();
            gridObjectCollection.CellWidth = 0.15f;
            gridObjectCollection.CellHeight = 0.035f;
            gridObjectCollection.SurfaceType = ObjectOrientationSurfaceType.Plane;
            gridObjectCollection.Layout = LayoutOrder.ColumnThenRow;
            gridObjectCollection.Columns = 1;
            gridObjectCollection.Anchor = LayoutAnchor.UpperLeft;
        }

        instantiatedButtons = new List<GameObject>();
        chosenSet = new List<DataManager.Data>();
    }

    public void OnEnable()
    {
        ClearList(); 
    }

    /// <summary>
    /// Called on Buttons in Scene 
    /// </summary>
    /// <param name="userSet"></param>
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

            // List for Toggle Collection 
            Interactable[] newToggleList = new Interactable[chosenSet.Count];

            // Generate Objects
            for (int i = 0; i < chosenSet.Count; i++)
            {
                GameObject itemInstance = Instantiate(dynamicItem, gridObjectCollection.transform);

                itemInstance.GetComponent<ButtonConfigHelper>().MainLabelText = "UserID " + chosenSet[i].UserData.UserID.ToString() +
                                                                                " SetType " + chosenSet[i].UserData.set.ToString();
                
                itemInstance.SetActive(true);
                newToggleList[i] = itemInstance.GetComponent<Interactable>();

                instantiatedButtons.Add(itemInstance);
            }

            // Update and Assign Changes
            gridObjectCollection.UpdateCollection();

            if(newToggleList.Length > 0)
            {
                toggleCollection.ToggleList = newToggleList;
            }
        }
        catch (System.Exception)
        {
            Debug.LogWarning("Custom Toggle List Populator not able to create a list."); 
        }

        
    }

    /// <summary>
    /// If the menu is called a second time, it is necessary to clear the list before
    /// </summary>
    private void ClearList()
    {
        if(instantiatedButtons != null)
        {
            foreach (GameObject item in instantiatedButtons)
            {
                Destroy(item);
            }
        }
        if(gridObjectCollection != null)
            gridObjectCollection.UpdateCollection();

        instantiatedButtons = new List<GameObject>();
        chosenSet = new List<DataManager.Data>();
    }
}
