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
    /// Object to duplicate in <see cref="ScrollView"/>. 
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

    private void Start()
    {
        numItems = DataManager.Instance.UserSettings.Count; 
        if(numItems == 0)
        {
            Debug.LogWarning("CustomScrollableListPopulator::Start no items in DataManager.UserSettings found. Using Default Value of 3.");
            numItems = 3; 
        }
    }


    private void OnEnable()
    {
        // Make sure we find a collection
        if (gridObjectCollection == null)
        {
            gridObjectCollection = parentTransform.GetComponent<GridObjectCollection>();
        }
    }

    public void MakeToggleList()
    {
        if (gridObjectCollection == null)
        {
            // Grid Object Collection to organize Buttons
            gridObjectCollection = parentTransform.AddComponent<GridObjectCollection>();
            gridObjectCollection.CellWidth = 0.1f;
            gridObjectCollection.CellHeight = 0.02f;
            gridObjectCollection.SurfaceType = ObjectOrientationSurfaceType.Plane;
            gridObjectCollection.Layout = LayoutOrder.ColumnThenRow;
            gridObjectCollection.Columns = 1;
            gridObjectCollection.Anchor = LayoutAnchor.UpperLeft;

            InteractableToggleCollection toggleCollection = parentTransform.AddComponent<InteractableToggleCollection>();
            Interactable[] newToggleList = new Interactable[numItems]; 


            //scrollView.AddContent(collectionGameObject);

            // Generate Objects
            for (int i = 0; i < numItems; i++)
            {
                GameObject itemInstance = Instantiate(dynamicItem, parentTransform.transform);
                itemInstance.SetActive(true);
                newToggleList[i] = itemInstance.GetComponent<Interactable>(); 
            }

            //scrollView.gameObject.SetActive(true);
            gridObjectCollection.UpdateCollection();

            toggleCollection.ToggleList = newToggleList; 

        }
    }
}
