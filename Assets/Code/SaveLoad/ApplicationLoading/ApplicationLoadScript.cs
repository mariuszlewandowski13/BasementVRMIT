using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;
using System.Linq;


public class ApplicationLoadScript : MonoBehaviour {

    #region Private Properties

    private string message;
    private bool loading = false;
    public static bool loadingLogin = false;
    private bool loadFirstLevel = false;
    private bool canLoad = false;
    private bool loadingError = false;
    private bool compared = false;


    private bool userImagesFromServerLoaded = false;
    private bool userImagesLocalLoaded = false;

    private bool canCheckImagesForlder = false;
    private object emailCheckLock = new object();

    private float time = 0.0f;

    private List<ImagesInfo> userImagesOnTheServer;
    private List<ImagesInfo> userImagesLocaly;

    private int sendedCounter = 0;
    private object sendedCounterLock = new object();
    private static System.Random random = new System.Random();

    private int idLength = 25;

    #endregion

    #region Public Properties

    //public Material loginMaterial;
    //public Material loadingMaterial;

    #endregion



    void Awake () {
        Application.runInBackground = true;
        ApplicationStaticData.LoadAllData();
    }

	void Update () {
        //if (!loading) { 
        //    loading = true;
        //    ApplicationStaticData.userID = CheckAndCreateNewID();
        //    ApplicationStaticData.userName = "someone";
        //    LoadUserData();
        //}


        //if (loadFirstLevel && canLoad)
        //{
        //    Debug.Log("Loading main room");
        //    //RenderSettings.skybox = loadingMaterial;
        //    loadFirstLevel = false;
        //    if (PhotonNetwork.inRoom)
        //    {
        //        PhotonNetwork.Disconnect();
        //    }

        //    //if (TutorialScript.tutorialActive)
        //    //{
        //        ApplicationStaticData.roomToConnectName = ApplicationStaticData.userRoom;
        //        SceneManager.LoadSceneAsync(ApplicationStaticData.userScene);
        //   // }
        //    //else {
        //    //    ApplicationStaticData.roomToConnectName = ApplicationStaticData.worldRoomName;
        //    //    SceneManager.LoadSceneAsync(ApplicationStaticData.worldSceneName);
        //    //}
        //}

        //if (loadFirstLevel && canCheckImagesForlder)
        //{
        //    LoadUserImagesInfoFromServer();
        //    LoadLocalUserImagesFolder();
        //    canCheckImagesForlder = false;
        //}

        //if (userImagesFromServerLoaded && userImagesLocalLoaded && !canLoad && !compared)
        //{
        //    CompareImagesFolders();
        //    compared = true;
        //}

        //if (loadingError)
        //{
        //    Application.Quit();
        //}
    }

    private string CheckAndCreateNewID()
    {
        string id = "";
        if (File.Exists(ApplicationStaticData.idFilePath))
        {
            id = GetIdFromfile();
        }
        else {
            id = CreateNewID();
        }

        return id;
    }

    private string CreateNewID()
    {
        string id = GetRandomString(idLength);
        File.WriteAllLines(ApplicationStaticData.idFilePath, new string[] { id });
        return id;
    }

    private string GetRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    private string GetIdFromfile()
    {
        string id = "";
        string [] idFile = File.ReadAllLines(ApplicationStaticData.idFilePath);
        id = idFile[0];
        return id;
    }

    private void LoadUserData()
    {
        WWWForm form = new WWWForm();
        form.AddField("userID", ApplicationStaticData.userID);
        form.AddField("userName", ApplicationStaticData.userName);
        WWW w = new WWW(ApplicationStaticData.scriptsServerAdress + "GetUserData.php", form);
        StartCoroutine(loadUserDataFromDb(w));
    }


    IEnumerator loadUserDataFromDb(WWW w)
    {
        yield return w;
        if (w.error == null)
        {
            message = w.text;
        }
        else {
            message = "ERROR: " + w.error + "\n";
        }
        Debug.Log(message);
        if (w.error == null)
        {
            string[] row = null;
            row = message.Split(new string[] { "#####" }, StringSplitOptions.None);

            ApplicationStaticData.userRoom = row[2];
            ApplicationStaticData.userScene = row[5];
            
               loadFirstLevel = true;
               canCheckImagesForlder = true;
        }
        else {
            loadingError = true;
        }
    }

    IEnumerator checkUserEmail(WWW w)
    {
        yield return w;
        if (w.error == null)
        {
            message = w.text;
        }
        else {
            message = "ERROR: " + w.error + "\n";
        }
        // Debug.Log(message);
        if (w.error == null)
        {
            lock(emailCheckLock)
            {
                string row = null;
                row = message;
                if (row != "" && loadingLogin == true && !canLoad)
                {
                    loadingLogin = false;
                    loadFirstLevel = true;
                    canCheckImagesForlder = true;
                }

                Debug.Log(row);
            }
        }
        else {
            loadingError = true;
        }
    }

    IEnumerator GetUserImagesInfoFromServer(WWW w)
    {
        yield return w;
        if (w.error == null)
        {
            message = w.text;

            string[] rawFilesInfo = message.Split(new string[] { "@@@@@" }, StringSplitOptions.None);

            userImagesOnTheServer = new List<ImagesInfo>();
          //  Debug.Log(message);

            foreach (string rawImgInfo in rawFilesInfo)
            {
                ImagesInfo newImgInfo = new ImagesInfo();
                string [] img = rawImgInfo.Split(new string[] { "#####" }, StringSplitOptions.None);

                if (img.Length > 1)
                {
                    newImgInfo.name = img[0];
                    newImgInfo.width = Int32.Parse(img[1]);
                    newImgInfo.height = Int32.Parse(img[2]);
                    newImgInfo.path = img[3];
                    userImagesOnTheServer.Add(newImgInfo);
                }
            }
        }
        else {
            message = "ERROR: " + w.error + "\n";
        }
        // Debug.Log(message);
        userImagesFromServerLoaded = true;
    }

    private void LoadLocalUserImagesFolder()
    {
        userImagesLocaly = ApplicationStaticData.GetUserImagesInfo();
        userImagesLocalLoaded = true;
    }

    private void LoadUserImagesInfoFromServer()
    {
        WWWForm form = new WWWForm();
        form.AddField("userID", ApplicationStaticData.userID);
        WWW w = new WWW(ApplicationStaticData.scriptsServerAdress + "GetUserImagesInfo.php", form);
        StartCoroutine(GetUserImagesInfoFromServer(w));
    }

    private void CompareImagesFolders()
    {
        List<ImagesInfo> removeFromImagesOnServer = new List<ImagesInfo>();
        List<ImagesInfo> sendImagesOnServer = new List<ImagesInfo>();

        try
        {
            foreach (ImagesInfo imgLocal in userImagesLocaly)
            {
                bool isOnServer = false;
                foreach (ImagesInfo imgOnServer in userImagesOnTheServer)
                {
                    if (imgLocal == imgOnServer)
                    {
                        isOnServer = true;
                        break;
                    }
                }
                if (!isOnServer)
                {
                    //Debug.Log("NotOnServer");
                    sendImagesOnServer.Add(imgLocal);
                }
            }

            foreach (ImagesInfo imgOnServer in userImagesOnTheServer)
            {
                bool isLocally = false;
                foreach (ImagesInfo imgLocal in userImagesLocaly)
                {
                    if (imgLocal == imgOnServer)
                    {
                        isLocally = true;
                        break;
                    }
                }

                if (!isLocally)
                {
                    removeFromImagesOnServer.Add(imgOnServer);
                }
            }
            RemoveImagesFromServer(removeFromImagesOnServer);
            SendImagesToTheServer(sendImagesOnServer);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
       
        

    }

    private void SendImagesToTheServer(List<ImagesInfo> sendImagesOnServer)
    {
        sendedCounter = sendImagesOnServer.Count;
        if (sendImagesOnServer.Count > 0)
        {
            foreach (ImagesInfo img in sendImagesOnServer)
            {
                SendImage(img);
            }
        }
        else {
            ApplicationStaticData.LoadUserImagesFromServer(userImagesOnTheServer);
            canLoad = true;
        }
        
    }

    private void RemoveImagesFromServer(List<ImagesInfo> removeFromImagesOnServer)
    {
        foreach (ImagesInfo img in removeFromImagesOnServer)
        {
            RemoveImage(img);
            userImagesOnTheServer.Remove(img);
        }
    }

    private void SendImage(ImagesInfo img)
    {
        Debug.Log("File: " + img.name + " sended!");
        WWWForm form = new WWWForm();
        form.AddField("userID", ApplicationStaticData.userID);
        byte [] bytes = File.ReadAllBytes(ApplicationStaticData.userImagesPath + img.name);
        form.AddBinaryData("file", bytes, img.name, "image/" + img.extension);

        WWW w = new WWW(ApplicationStaticData.scriptsServerAdress + "SaveImageFile.php", form);
        StartCoroutine(sendRequest(w));
    }

    private void RemoveImage(ImagesInfo img)
    {
        Debug.Log("File: " + img.name + " removed!");
        WWWForm form = new WWWForm();
        form.AddField("userID", ApplicationStaticData.userID);
        form.AddField("filename", img.name);
        WWW w = new WWW(ApplicationStaticData.scriptsServerAdress + "RemoveImageFile.php", form);
        StartCoroutine(removeRequest(w));
    }

    IEnumerator removeRequest(WWW w)
    {
        yield return w;
        string message;
        if (w.error == null)
        {
            message = w.text;
        }
        else {
            message = "ERROR: " + w.error + "\n";
        } 
    }

    IEnumerator sendRequest(WWW w)
    {
        yield return w;
        string message;
        if (w.error == null)
        {
            message = w.text;
            Debug.Log("New image from uploaded: " + message);

            string[] img = message.Split(new string[] { "#####" }, StringSplitOptions.None);
            
            ImagesInfo newImgInfo = new ImagesInfo();
                if (img.Length > 1)
                {
                    newImgInfo.name = img[0];
                    newImgInfo.width = Int32.Parse(img[1]);
                    newImgInfo.height = Int32.Parse(img[2]);
                    newImgInfo.path = img[3];
                    userImagesOnTheServer.Add(newImgInfo);
                }
        }
        else {
            message = "ERROR: " + w.error + "\n";
        }  
        lock(sendedCounterLock)
        {
            sendedCounter--;
            if (sendedCounter <= 0)
            {
                ApplicationStaticData.LoadUserImagesFromServer(userImagesOnTheServer);
                canLoad = true; 
            }
        }


    }


}
