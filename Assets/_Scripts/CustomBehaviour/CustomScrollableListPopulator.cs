using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Linq;
using TMPro;

public class CustomScrollableListPopulator : MonoBehaviour
{
    #region serialized properties
    [SerializeField]
    [Tooltip("The ScrollingObjectCollection to populate, if left empty. the populator will create on your behalf.")]
    private ScrollingObjectCollection scrollView;

    [NonSerialized]
    [Tooltip("Objects to duplicate in ScrollCollection")]
    private GameObject[] dynamicItems;

    [SerializeField]
    [Tooltip("Parent Object")]
    private GameObject buttonObject;

    [NonSerialized]
    private int numItems;

    [SerializeField]
    private TextMeshPro text; 

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
    private ClippingBox clippingBox;
    private List<GameObject> instantatedObjects; 

    private void OnEnable()
    {
        // Make sure we find a collection
        if (scrollView == null)
        {
            scrollView = GetComponentInChildren<ScrollingObjectCollection>();
        }
    }


    /// <summary>
    /// Called, if GameObject is disabled
    /// Removes Instantiated Objects etc
    /// </summary>
    private void OnDisable()
    {
        
    }

    /// <summary>
    /// Called in Editor 
    /// </summary>
    /// <param name="listType"></param>
    public void MakeScrollingList(string listType)
    {
        ClearList(); 
        if(DataManager.Instance != null)
        {
            if (listType == "incompleteSet")
            {
                numItems = DataManager.Instance.IncompleteUserData.Count;
                StartCoroutine(UpdateList("Sets", loader, DataManager.Instance.IncompleteUserData));

            }
            else if (listType == "completeSet")
            {
                numItems = DataManager.Instance.CompleteUserData.Count;
                StartCoroutine(UpdateList("Sets", loader, DataManager.Instance.CompleteUserData));
            }
            else if (listType == "newSet")
            {
                numItems = DataManager.Instance.NewUserData.Count;
                StartCoroutine(UpdateList("Sets", loader, DataManager.Instance.NewUserData));
            }
            else if (listType == "Objects")
            {
                dynamicItems = Resources.LoadAll<GameObject>("Objects/");
                numItems = dynamicItems.Length;
                this.gameObject.SetActive(true);
                StartCoroutine(UpdateList("Objects", loader));
            }
            else
                throw new System.Exception("CustomToggle List Populator, incorrect input");
        }
        else
        {
            Debug.LogWarning("CustomScrollable List Populator tried to get Datamananger Instance, which was null."); 
        }
        
       
    }

    /// Used in Coroutine to slowly load objects
    private IEnumerator UpdateList(string listType, GameObject loaderViz, List<DataManager.Data> chosenSet = null)
    {
        if (listType == "Sets" && chosenSet == null)
            throw new System.ArgumentException(" When chosing a Set List, the chosen Set cannot be null."); 

        // Show Loader
        loaderViz.SetActive(true);

        // Populate List
        for (int currItemCount = 0; currItemCount < numItems; currItemCount++)
        {
            // Instantiate Buttons
            var button = Instantiate<GameObject>(buttonObject, gridObjectCollection.transform);
            
            button.SetActive(true);

            if (listType == "Objects")
            {
                // Set Label
                var obj = dynamicItems[currItemCount]; 
                button.GetComponent<ButtonConfigHelper>().MainLabelText = obj.name;

                // Add Listener
                button.GetComponent<ButtonConfigHelper>().OnClick.AddListener(() => InstantiateObject(obj, button));
            }
            else if (listType == "Sets")
            {
                // Set Label
                button.GetComponent<ButtonConfigHelper>().MainLabelText = "UserID " + chosenSet[currItemCount].UserData.UserID.ToString() + " SetType " + chosenSet[currItemCount].UserData.set.ToString();
                var Set = chosenSet[currItemCount];

                // Add Listener
                button.GetComponent<ButtonConfigHelper>().OnClick.AddListener(() => SaveSettings(button, Set));
            }
            else
                throw new System.ArgumentException("ListType has to be either \"Objects\" or \"Sets\". "); 

            instantatedObjects.Add(button);

            // Update Renderers for clippingBox
            var renderer = button.GetComponentsInChildren<MeshRenderer>();
            foreach (var r in renderer)
                clippingBox.AddRenderer(r);
                
            yield return null;
        }

        // Now that the list is populated, hide the loader and show the list
        loaderViz.SetActive(false);
        scrollView.gameObject.SetActive(true);

        // Set up collection and Scroll View
        gridObjectCollection.UpdateCollection();
        scrollView.UpdateContent();
    }

    /// <summary>
    /// Instantiate Objects when chosen in new settings menu
    /// </summary>
    /// <param name="obj"> Game Object to instantiate</param>
    /// <param name="button"> reference to button object to disable it after use </param>
    private void InstantiateObject(GameObject obj, GameObject button)
    {
        // Spawn Object
        objectCreator.SpawnObject(obj, GameManager.Instance.parentPlayTable, Vector3.zero, Quaternion.identity, ConfigType.MovementEnabled);

        // Disable Button to prevent several objects of the same type in scene  //\ TODO bessere lösung?
        button.SetActive(false);
        
        // Update Button and Object Collection
        gridObjectCollection.UpdateCollection();
        GameManager.Instance.parentPlayTable.GetComponent<GridObjectCollection>().UpdateCollection(); 

        // Update Renderers in Clipping Box
        var renderer = button.GetComponentsInChildren<MeshRenderer>();
        foreach (var r in renderer)
            clippingBox.RemoveRenderer(r);
    }

    /// <summary>
    /// Write Chosen Text, When List ist Set List
    /// Saves chosen Set to DataManater
    /// </summary>
    /// <param name="button"></param>
    /// <param name="chosenSet"></param>
    private void SaveSettings(GameObject button, DataManager.Data chosenSet)
    {
        if (text != null)
        {
            // Update Text
            text.text = button.GetComponent<ButtonConfigHelper>().MainLabelText;

            // Save Settings
            DataManager.Instance.CurrentSet = chosenSet;
        }
        else
            throw new MissingComponentException("Add text Object to Custom Scrollable List"); 
    }

    /// <summary>
    /// Called in UserInputHelper to combine user data and object data
    /// </summary>
    /// <returns></returns>
    public ObjectData GetInstantiatedObjects()
    {
        ObjectData newData = new ObjectData(objectCreator.InstantiatedObjects, Time.time);
        objectCreator.RemoveAllObjects();
        return newData; 
    }

    /// <summary>
    /// Smoothly moves the scroll container a relative number of tiers of cells.
    /// Attached to Buttons next to Scroll View
    /// </summary>
    public void ScrollByTier(int amount)
    {
        scrollView.MoveByTiers(amount);
    }

    /// <summary>
    /// Clears List Properties to Start with a new List each time, the List is activated again
    /// </summary>
    public void ClearList()
    {

        // Object Creator
        if (objectCreator == null)
        {
            objectCreator = ScriptableObject.CreateInstance<ObjectCreator>();
            objectCreator.PrefabFolderName = "Objects";
        }
        else
        {
            objectCreator.Reset();
            objectCreator = null;
            objectCreator = ScriptableObject.CreateInstance<ObjectCreator>(); 

        }

        // Instantiated Button Objects
        if (instantatedObjects == null)
        {
            instantatedObjects = new List<GameObject>();
        }
        else
        {
            foreach (GameObject item in instantatedObjects)
            {
                Destroy(item);
            }
        }

        // Update GridObjectCollection
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
        gridObjectCollection.UpdateCollection();
        gridObjectCollection.gameObject.SetActive(true);


        // Find ScrollView
        if (scrollView == null)
        {
            scrollView = GetComponentInChildren<ScrollingObjectCollection>();

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

        }
        
        // Reset Text
        if(text != null)
            text.SetText("");

        // Activate Scroll View
        if (scrollView != null)
            scrollView.gameObject.SetActive(true);
        else
            throw new ArgumentNullException("Assign a Scrolling ObjectCollection as Child of CustomScrollableListPopulator.");


        // Clipping Box
        if(clippingBox == null)
            clippingBox = scrollView.GetComponentInChildren<ClippingBox>();

        // Parameters
        numItems = 0; 
    }





}
