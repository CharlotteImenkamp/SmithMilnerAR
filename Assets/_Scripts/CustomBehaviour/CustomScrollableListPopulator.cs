using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;

public class CustomScrollableListPopulator : MonoBehaviour
{
    #region serialized properties
    [SerializeField]
    [Tooltip("The ScrollingObjectCollection to populate, if left empty. the populator will create on your behalf.")]
    private ScrollingObjectCollection scrollView;

    [SerializeField]
    [Tooltip("Objects to duplicate in ScrollCollection")]
    private GameObject[] dynamicItems;

    [NonSerialized]
    private int numItems;

    [SerializeField]
    [Tooltip("Demonstrate lazy loading")]
    private bool lazyLoad;

    /// <summary>
    /// Demonstrate lazy loading 
    /// </summary>
    public bool LazyLoad
    {
        get { return lazyLoad; }
        set { lazyLoad = value; }
    }

    [SerializeField]
    [Tooltip("Indeterminate loader to hide / show for LazyLoad")]
    private GameObject loader;

    [SerializeField]
    private float cellWidth = 0.04f;

    [SerializeField]
    private float cellHeight = 0.4f;

    [SerializeField]
    private float cellDepth = 0.04f;

    [SerializeField]
    private int cellsPerTier = 3;

    [SerializeField]
    private int tiersPerPage = 5;

    [SerializeField]
    private Transform scrollPositionRef = null;

    #endregion serialized properties 

    private ObjectCreator objectCreator; 
    private GridObjectCollection gridObjectCollection;

    /// <summary>
    /// Indeterminate loader to hide / show for <see cref="LazyLoad"/> 
    /// </summary>
    public GameObject Loader
    {
        get { return loader; }
        set { loader = value; }
    }

    public ScrollingObjectCollection ScrollView { get => scrollView; set => scrollView = value; }

    private void OnEnable()
    {
        // Make sure we find a collection
        if (scrollView == null)
        {
            scrollView = GetComponentInChildren<ScrollingObjectCollection>();
        }
    }

    private void Start()
    {
        objectCreator = ScriptableObject.CreateInstance<ObjectCreator>();
        objectCreator.PrefabFolderName = "Objects";
        objectCreator.BoundingBoxFolderName = "BoundingBox";

        dynamicItems = Resources.LoadAll<GameObject>("Objects/");
        numItems = dynamicItems.Length;
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

        if (!lazyLoad)
        {
            
            objectCreator.SpawnObjects(dynamicItems, gridObjectCollection.gameObject, gridObjectCollection.transform.position, gridObjectCollection.transform.rotation, ConfigType.scrollBox);
            scrollView.gameObject.SetActive(true);
            gridObjectCollection.UpdateCollection();
        }
        else
        {
            if (loader != null)
            {
                loader.SetActive(true);
            }

            StartCoroutine(UpdateListOverTime(loader));
        }
    }

    private IEnumerator UpdateListOverTime(GameObject loaderViz)
    {
        for (int currItemCount = 0; currItemCount < numItems; currItemCount++)
        {
            objectCreator.SpawnObject(dynamicItems[currItemCount], gridObjectCollection.gameObject, gridObjectCollection.transform.position, gridObjectCollection.transform.rotation, ConfigType.scrollBox);
            // objectCreator.ResizeObject(dynamicItems[currItemCount], 0.5f);
            yield return null;
        }

        // Now that the list is populated, hide the loader and show the list
        loaderViz.SetActive(false);
        scrollView.gameObject.SetActive(true);

        // Finally, manually call UpdateCollection to set up the collection
        gridObjectCollection.UpdateCollection();
    }

}
