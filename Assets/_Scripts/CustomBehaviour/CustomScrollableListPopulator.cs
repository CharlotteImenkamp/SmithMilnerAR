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

    private void Start()
    {
        objectCreator = ScriptableObject.CreateInstance<ObjectCreator>();
        objectCreator.PrefabFolderName = "Objects";
        instantatedObjects = new List<GameObject>(); 


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
        clippingBox = scrollView.GetComponentInChildren<ClippingBox>();

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

        if (loader != null)
        {
            loader.SetActive(true);
        }
    }

    public void MakeScrollingList(string listType)
    {
        ClearList(); 

        if (listType == "incompleteSet")
        {
            numItems = DataManager.Instance.IncompleteUserData.Count;
            StartCoroutine(UpdateUserList(loader, DataManager.Instance.IncompleteUserData));

        }
        else if (listType == "completeSet")
        {
            numItems = DataManager.Instance.CompleteUserData.Count;
            StartCoroutine(UpdateUserList(loader, DataManager.Instance.CompleteUserData));
        }
        else if (listType == "newSet")
        {
            numItems = DataManager.Instance.NewUserData.Count;
            StartCoroutine(UpdateUserList(loader, DataManager.Instance.NewUserData));
        }
        else if(listType == "Objects")
        {
            dynamicItems = Resources.LoadAll<GameObject>("Objects/");
            numItems = dynamicItems.Length;
            this.gameObject.SetActive(true); 
            StartCoroutine(UpdateObjectList(loader));
        }
        else
        {
            throw new System.Exception("CustomToggle List Populator, incorrect input");
        } 
    }

    private IEnumerator UpdateObjectList(GameObject loaderViz)
    {
            for (int currItemCount = 0; currItemCount < numItems; currItemCount++)
            {
                // Buttons
                var parent = Instantiate<GameObject>(buttonObject);
                parent.transform.parent = gridObjectCollection.gameObject.transform;
                parent.transform.localRotation = Quaternion.identity;
                parent.SetActive(true);

                var obj = dynamicItems[currItemCount];
                parent.GetComponent<ButtonConfigHelper>().MainLabelText = obj.name;

                // spawn Object
                parent.GetComponent<ButtonConfigHelper>().OnClick.AddListener(() => InstantiateObject(obj, parent));
                instantatedObjects.Add(parent);

                var renderer = parent.GetComponentsInChildren<MeshRenderer>();
                foreach (var r in renderer)
                {
                    clippingBox.AddRenderer(r);
                }
                yield return null;
            }

            // Now that the list is populated, hide the loader and show the list
            loaderViz.SetActive(false);
            scrollView.gameObject.SetActive(true);

            // Finally, manually call UpdateCollection to set up the collection
            gridObjectCollection.UpdateCollection();
            scrollView.UpdateContent();
    }

    private IEnumerator UpdateUserList(GameObject loaderViz, List<DataManager.Data> chosenSet)
    {
         
        for (int i = 0; i < numItems; i++)
        {
            // Buttons
            var itemInstance = Instantiate<GameObject>(buttonObject, gridObjectCollection.transform);
            itemInstance.SetActive(true);

            itemInstance.GetComponent<ButtonConfigHelper>().MainLabelText = "UserID " + chosenSet[i].UserData.UserID.ToString() +
                                                                               " SetType " + chosenSet[i].UserData.set.ToString();
            instantatedObjects.Add(itemInstance);

            // Write Text
            var Set = chosenSet[i]; 
            itemInstance.GetComponent<ButtonConfigHelper>().OnClick.AddListener(() => WriteText(itemInstance,Set));

            var renderer = itemInstance.GetComponentsInChildren<MeshRenderer>();
            foreach (var r in renderer)
            {
                clippingBox.AddRenderer(r);
            }
            yield return null;
        }

        // Now that the list is populated, hide the loader and show the list
        loaderViz.SetActive(false);
        scrollView.gameObject.SetActive(true);

        // Finally, manually call UpdateCollection to set up the collection
        gridObjectCollection.UpdateCollection();
        scrollView.UpdateContent();
    }

    /// <summary>
    /// Instantiate Objects when chosen in new settings menu
    /// </summary>
    /// <param name="obj">Game Object to instantiate</param>
    /// <param name="button"> reference to button object to disable it after use </param>
    private void InstantiateObject(GameObject obj, GameObject button)
    {
        objectCreator.SpawnObject(obj, GameManager.Instance.parentPlayTable, Vector3.zero, Quaternion.identity, ConfigType.MovementEnabled);

        // Disable Button to prevent several objects of the same type in scene  //\ TODO bessere lösung?
        button.SetActive(false);
        
        gridObjectCollection.UpdateCollection();
        GameManager.Instance.parentPlayTable.GetComponent<GridObjectCollection>().UpdateCollection(); 

        var renderer = button.GetComponentsInChildren<MeshRenderer>();
        foreach (var r in renderer)
            clippingBox.RemoveRenderer(r);
    }

    private void WriteText(GameObject button, DataManager.Data chosenSet)
    {
        if (text != null)
        {
            text.text = button.GetComponent<ButtonConfigHelper>().MainLabelText;
            DataManager.Instance.CurrentSettings = chosenSet;
        }
        else
            throw new MissingComponentException("Add text Object to Custom Scrollable List"); 
    }

    /// <summary>
    /// Called in userInputHelper to combine user data and object data
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
    /// </summary>
    public void ScrollByTier(int amount)
    {
        scrollView.MoveByTiers(amount);
    }

    public void ClearList()
    {
        if (instantatedObjects != null)
        {
            foreach (GameObject item in instantatedObjects)
            {
                Destroy(item);
            }
        }
        if (gridObjectCollection != null)
            gridObjectCollection.UpdateCollection();


        // Make sure we find a collection
        if (scrollView == null)
        {
            scrollView = GetComponentInChildren<ScrollingObjectCollection>();
        }
        if(text != null)
            text.SetText(""); 

        scrollView.gameObject.SetActive(true);
        gridObjectCollection.gameObject.SetActive(true);
        loader.SetActive(true); 
        
    }
}
