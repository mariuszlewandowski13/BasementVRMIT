#region Usings

using UnityEngine;

#endregion

public class ToolsButtons : ToolsObject {

    #region Public Properties

    public SceneObject[] sceneObjects;
    public GameObject toolIcon;
    public bool needColorsbar;
    public bool rotation;
    public int textureNumber;

    public int sourceForSceneObjects;

    public int statsNum;

    public static int taskNumber = 0;

    #endregion

    #region Private Properties
    private string searchText;
    private GameObject searchBox;
    private GameObject keyboard;

    public delegate void SceneObjectsLoaded();
    public event SceneObjectsLoaded newSceneObjectsList;

    private bool searchGiphy;
    private bool searchGoogle;

    private object searchGiphyLock = new object();
    private object searchGoogleLock = new object();
    #endregion

    #region Constructors

    public ToolsButtons(SceneObject[] sceneObj, GameObject icon, bool needColorsbar, bool rotation, int statsNumber, int source = 0)
    {
        sceneObjects = sceneObj;
        toolIcon = icon;
        this.needColorsbar = needColorsbar;
        this.rotation = rotation;
        sourceForSceneObjects = source;
        statsNum = statsNumber;
        //LoadSceneObjects();
    }

    public ToolsButtons(GameObject icon, bool needColorsbar)
    {
        sceneObjects = null;
        toolIcon = icon;
        this.needColorsbar = needColorsbar;
        this.rotation = false;
        sourceForSceneObjects = 0;
    }

    #endregion

    #region Methods

    public void LoadSceneObjects(GameObject controller = null)
    {
        if (sourceForSceneObjects > 0 && sourceForSceneObjects < 3)
        {
            keyboard = KeyboardHandlerScript.InitializeKeyboard();
            searchText = "";
            searchBox = KeyboardHandlerScript.InitalizeSearchBox(searchText);
            searchBox.GetComponent<TextMesh>().text = searchText;
          //  keyboard.GetComponent<KeyboardScript>().NewCharAdded += OnCharAdded;
        }

    }


    #endregion

}
