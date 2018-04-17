#region Usings

using UnityEngine;
using System;
using WindowsInput;

#endregion

public class KeyboardScript : MonoBehaviour
{

    public static KeyboardScript instance;

    #region Private Properties
    private InputSimulator sim = new InputSimulator();

    #endregion

    #region Events

    public delegate void NewChar(string newChar);
    public event NewChar NewCharAdded;

    public delegate void NewKey(Event ev);
    public event NewKey NewKeyPressed;



    #endregion

    #region Public Properties

    public string lastChar;
    public bool textBoxActive = false;
    private string _text;
    public string text {
        get { return _text; }
        set {
            _text = value;
            KeyboardHandlerScript.searchBox.GetComponent<TextMesh>().text = value;
        }
    }

    #endregion

    #region Methods

    private void Awake()
    {
        instance = this;
    }

    public void AddLetter(WindowsInput.Native.VirtualKeyCode keyCode)
    {
        sim.Keyboard.KeyDown(keyCode);
        sim.Keyboard.KeyUp(keyCode);
    }





    #endregion
}