#region Usings

using UnityEngine;
using WindowsInput;

#endregion

public class LetterScript : MonoBehaviour, IClickable {

    #region Public Properties
    public WindowsInput.Native.VirtualKeyCode  keyCode;
    #endregion

    #region Private Properties
    private Color lightColor;
    private Color darkColor;

    private Renderer _renderer;
    private Renderer renderer2
    {
        get {
            if (_renderer == null)
            {
                _renderer = GetComponent<Renderer>();
                _renderer.material.EnableKeyword("_EMISSION");
            }
            return _renderer;
        }
    }

    private float clickDuration = 0.1f;
    private bool clicking;
    private float lastClickTime;
    #endregion

    #region Methods
    void Start()
    {
        lightColor = new Color(0.6597f, 0.357f, 0.0f);
        darkColor = Color.black;
    }

    void OnEnable()
    {
        EndAnimate();
    }

    public void ClickDown(GameObject controller)
    {
        StartAnimate();
        transform.parent.GetComponent<KeyboardScript>().AddLetter(keyCode);
    }

    public void ClickUp(GameObject controller)
    { }

    private void Update()
    {
        if (clicking && (lastClickTime + clickDuration) < Time.time)
        {
            EndAnimate();
        }
    }

    private void StartAnimate()
    {
        clicking = true;
        lastClickTime = Time.time;
        renderer2.material.SetColor("_EmissionColor", lightColor);
    }

    private void EndAnimate()
    {
        clicking = false;
        renderer2.material.SetColor("_EmissionColor", darkColor);
    }

    #endregion
}
