using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;

public class CustomScrollableListPopulator : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Object to duplicate in ScrollCollection")]
    private GameObject dynamicItem;

    /// <summary>
    /// Object to duplicate in <see cref="ScrollView"/>. 
    /// </summary>
    public GameObject DynamicItem
    {
        get { return dynamicItem; }
        set { dynamicItem = value; }
    }


    [SerializeField]
    [Tooltip("Number of items to generate")]
    private int numItems;
    /// <summary>
    /// Number of items to generate
    /// </summary>
    public int NumItems
    {
        get { return numItems; }
        set { numItems = value; }
    }

    [SerializeField]
    [Tooltip("The ScrollingObjectCollection to populate, if left empty. the populator will create on your behalf.")]
    private ScrollingObjectCollection scrollView;

    /// <summary>
    /// The ScrollingObjectCollection to populate, if left empty. the populator will create on your behalf.
    /// </summary>
    public ScrollingObjectCollection ScrollView
    {
        get { return scrollView; }
        set { scrollView = value; }
    }


    private GridObjectCollection gridObjectCollection;
    private Transform scrollPositionRef = null;

    // collection sizes
    private float cellWidth = 0.1f;
    private float cellHeight = 0.032f;
    private float cellDepth = 0.032f;
    private int cellsPerTier = 1;
    private int tiersPerPage = 5;

    private void OnEnable()
    {
        // Make sure we find a collection
        if (scrollView == null)
        {
            scrollView = GetComponentInChildren<ScrollingObjectCollection>();
        }
    }

    public void MakeScrollingList()
    {
        if (scrollView == null)
        {
            GameObject newScroll = new GameObject("Scrolling Object Collection");
            newScroll.transform.parent = scrollPositionRef ? scrollPositionRef : transform;
            newScroll.transform.localPosition = Vector3.zero;
            newScroll.transform.localRotation = Quaternion.identity;
            newScroll.SetActive(false);
            scrollView = newScroll.AddComponent<ScrollingObjectCollection>();

            // Prevent the scrolling collection from running until we're done dynamically populating it.
            scrollView.CellWidth = cellWidth;
            scrollView.CellHeight = cellHeight;
            scrollView.CellDepth = cellDepth;
            scrollView.CellsPerTier = cellsPerTier;
            scrollView.TiersPerPage = tiersPerPage;
        }

        gridObjectCollection = scrollView.GetComponentInChildren<GridObjectCollection>();

        if (gridObjectCollection == null)
        {
            GameObject collectionGameObject = new GameObject("Grid Object Collection");
            collectionGameObject.transform.position = scrollView.transform.position;
            collectionGameObject.transform.rotation = scrollView.transform.rotation;

            gridObjectCollection = collectionGameObject.AddComponent<GridObjectCollection>();

            gridObjectCollection.CellWidth = cellWidth;
            gridObjectCollection.CellHeight = cellHeight;
            gridObjectCollection.SurfaceType = ObjectOrientationSurfaceType.Plane;
            gridObjectCollection.Layout = LayoutOrder.ColumnThenRow;
            gridObjectCollection.Columns = cellsPerTier;
            gridObjectCollection.Anchor = LayoutAnchor.UpperLeft;

            scrollView.AddContent(collectionGameObject);
        }


        // Generate Objects
         for (int i = 0; i < numItems; i++)
         {
             MakeItem(dynamicItem);
         }
         scrollView.gameObject.SetActive(true);
         gridObjectCollection.UpdateCollection();

    }


    public void MakeToggleList()
    {
        gridObjectCollection = scrollView.GetComponentInChildren<GridObjectCollection>();

        if (gridObjectCollection == null)
        {
            GameObject collectionGameObject = new GameObject("Grid Object Collection");
            collectionGameObject.transform.position = scrollView.transform.position;
            collectionGameObject.transform.rotation = scrollView.transform.rotation;

            gridObjectCollection = collectionGameObject.AddComponent<GridObjectCollection>();

            gridObjectCollection.CellWidth = cellWidth;
            gridObjectCollection.CellHeight = cellHeight;
            gridObjectCollection.SurfaceType = ObjectOrientationSurfaceType.Plane;
            gridObjectCollection.Layout = LayoutOrder.ColumnThenRow;
            gridObjectCollection.Columns = cellsPerTier;
            gridObjectCollection.Anchor = LayoutAnchor.UpperLeft;

            InteractableToggleCollection toggleCollection = collectionGameObject.AddComponent<InteractableToggleCollection>();
            Interactable[] newToggleList = new Interactable[numItems]; 


            scrollView.AddContent(collectionGameObject);

            // Generate Objects
            for (int i = 0; i < numItems; i++)
            {
                GameObject itemInstance = Instantiate(dynamicItem, gridObjectCollection.transform);
                itemInstance.SetActive(true);
                newToggleList[i] = itemInstance.GetComponent<Interactable>(); 
            }

            scrollView.gameObject.SetActive(true);
            gridObjectCollection.UpdateCollection();

            toggleCollection.ToggleList = newToggleList; 

        }
    }
    private void MakeItem(GameObject item)
    {

    }
}
