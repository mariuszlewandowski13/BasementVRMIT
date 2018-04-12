using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class LoginScript : MonoBehaviour {

    public GameObject waitingPlaceholder;
    public Button loginActionButton;

    public Text errorText;

    public InputField loginInput;
    public InputField passInput;

    private int minimalLength = 6;

    private void Start()
    {
        loginActionButton.onClick.AddListener(LoginAction);
    }

    private void LoginAction()
    {
        string error = "";

        string login = loginInput.text;
        string pass = passInput.text;

        login = PrepareInput(login);
        pass = PrepareInput(pass);

        if (CheckInputCorectness(login, out error, "login"))
        {
            if (CheckInputCorectness(pass, out error, "password"))
            {
                TryLogin(login, pass);
            }
        }

        SetErrorText(error);

    }

    private string PrepareInput(string input)
    {
        return input.Replace(" ", string.Empty);
    }

    private bool CheckInputCorectness( string input, out string error, string inputInfo)
    {
        if (input.Length < minimalLength)
        {
            error = "ERROR: "+ inputInfo + " should have at least " + minimalLength.ToString() + " characters (spaces are removed).";
            return false;
        }

        if (!isAlphaNumeric(input))
        {
            error = "ERROR: You can use only alphanumeric characters for your "+ inputInfo + ".";
            return false;
        }

        error = "";
        return true;
    }

    private bool isAlphaNumeric(string strToCheck)
    {
        Regex rg = new Regex(@"^[a-zA-Z0-9]*$");
        return rg.IsMatch(strToCheck);
    }

    private void SetErrorText(string error)
    {
        errorText.text = error;
    }

    private void Waiting(bool state)
    {
        waitingPlaceholder.SetActive(state);
        loginActionButton.gameObject.SetActive(!state);
    }


    private void TryLogin(string login, string password)
    {
        Waiting(true);

        WWWForm form = new WWWForm();
        form.AddField("username", login);
        form.AddField("password", password);
        form.AddField("userType", (int)ApplicationStaticData.userType);
        WWW w = new WWW(ApplicationStaticData.scriptsServerAdress + "Login.php", form);
        StartCoroutine(loginRequest(w,login));
    }

    IEnumerator loginRequest(WWW w, string login)
    {
        string message = "";
        yield return w;
        if (w.error == null)
        {
            message = w.text;
        }
        else
        {
            message = "ERROR: " + w.error + "\n";
        }

        if (message == "OK")
        {
            LoadNextLoginLevel( login);
        }

        SetErrorText(message);
        Waiting(false);
    }

    private void LoadNextLoginLevel(string login)
    {
        ApplicationStaticData.userName = login;

    }
}
