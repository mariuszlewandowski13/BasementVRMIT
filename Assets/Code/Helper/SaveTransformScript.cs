#region Usings

using UnityEngine;

#endregion

public class SaveTransformScript : MonoBehaviour {


    #region Public Properties
    public GameObject parentObject;
    #endregion

    #region Private Properties

    private bool active = true;
    private Transform colorpicker;
    private Transform pointer;
    private Transform colorpicker2;
    private Transform pointer2;
    private Transform bottom;
    private Transform bottom1;
    private Transform bottom2;

    private bool foundBottoms;
    private SelectingObjectsScript selectingObjectScript;

    #endregion

    #region Methods
    void Start()
    {
        colorpicker = transform.Find("ColorPicker");
        pointer = transform.Find("Pointer");
        colorpicker2 = transform.Find("BrightnessPicker");
        pointer2 = transform.Find("Pointer2");
    }

    void Update () {
        if (parentObject == null) DestroyImmediate(gameObject);
        else if (active)
        {
            if (!foundBottoms)
            {
                FindBottoms();
                foundBottoms = true;
            }

            Vector3 newPos = new Vector3();
            Quaternion newRotation = new Quaternion();


            if (bottom != null)
            {
                newPos = bottom.position;
                newRotation = bottom.rotation;
            }
            else if (bottom1 != null && bottom2 != null)
            {
                newPos = Vector3.Lerp(bottom1.position, bottom2.position, 0.5f);
                newRotation = bottom1.rotation;
            }

            transform.position = newPos;
            transform.rotation = newRotation;

        }
        FadeColorpicker();

    }

    public void SetParentObject(GameObject parent)
    {
        parentObject = parent;
        selectingObjectScript = parent.GetComponent<SelectingObjectsScript>();
    }


    private void FadeColorpicker()
    {
        if (colorpicker != null)
        {
            if (!active && selectingObjectScript.selected)
            {
                colorpicker.gameObject.SetActive(true);
                pointer.gameObject.SetActive(true);
                colorpicker2.gameObject.SetActive(true);
                pointer2.gameObject.SetActive(true);
                active = true;
            }
            else if (active && !selectingObjectScript.selected)
            {
                colorpicker.gameObject.SetActive(false);
                pointer.gameObject.SetActive(false);
                colorpicker2.gameObject.SetActive(false);
                pointer2.gameObject.SetActive(false);
                active = false;
            }
        }
    }


    private void FindBottoms()
    {
        bottom = parentObject.transform.Find("ScaleHandler").Find("Bottom");
        bottom1 = parentObject.transform.Find("ScaleHandler").Find("Bottom1");
        bottom2 = parentObject.transform.Find("ScaleHandler").Find("Bottom2");
    }

    #endregion
}
