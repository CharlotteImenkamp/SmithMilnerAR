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

    private int numItems;
    /// <summary>
    /// Number of items to generate
    /// </summary>
    public int NumItems
    {
        get { return numItems; }
        set { numItems = value; }
    }

    private GridObjectCollection gridObjectCollection = null;
    private InteractableToggleCollection toggleCollection = null;

    public void Initialize()
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

        if (DataManager.Instance != null)
        {
            // Get Size
            if (DataManager.Instance.UserSettings.Count == 0)
            {
                GameManager.Instance.debugText.text = "CustomScrollableListPopulator::Start no items in DataManager.UserSettings found. Using Default Value of 3.";

                Debug.LogWarning("CustomScrollableListPopulator::Start no items in DataManager.UserSettings found. Using Default Value of 3.");
                numItems = 3;
            }
            else
            {
                numItems = DataManager.Instance.UserSettings.Count;
            }
        }
        else
        {
            GameManager.Instance.debugText.text = "CustomToggleList Populator:: Initialize not able to get DataManager Instance"; 
            Debug.LogError("CustomToggleList Populator:: Initialize not able to get DataManager Instance"); 
        }

    }

    public void MakeToggleList()
    {
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
        Interactable[] newToggleList = new Interactable[numItems]; 


        // Generate Objects
        for (int i = 0; i < numItems; i++)
        {
            GameObject itemInstance = Instantiate(dynamicItem, parentTransform.transform);
            itemInstance.GetComponent<ButtonConfigHelper>().MainLabelText = "UserID " + DataManager.Instance.UserSettings[i].UserID.ToString() +
                                                                            " SetType " + DataManager.Instance.UserSettings[i].set.ToString() ; 
            itemInstance.SetActive(true);
            newToggleList[i] = itemInstance.GetComponent<Interactable>(); 
        }

        gridObjectCollection.UpdateCollection();
        toggleCollection.ToggleList = newToggleList; 
    }
}
