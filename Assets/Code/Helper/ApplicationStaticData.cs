#region Usings
using System.Collections.Generic;
using System;
using UnityEngine;
#endregion

public enum UserType {
    teacher,
    studentWithPriviliges,
    student
}

public static class ApplicationStaticData {

    #region Public Properties

    static private string _roomToConnectName = "MITclass1";
    static public string roomToConnectName
    {
        get {
            return _roomToConnectName;
        }

        set {

            _roomToConnectName = value;
        }
    }

    static public string userName = "";
    static public string userID = "";
    static public string userRoom = "";
    static public string userScene = "";
    static public string scriptsServerAdress = "http://vrowser.e-kei.pl/BasementVR/MIT/scripts/";

    static public Color objectsHighligthing = Color.red;
    static public Color toolsHighligthing = Color.red;
    static public Color doorsHighligthing = Color.red;
#if UNITY_EDITOR

    static public string imagesPath = "C:/files/";
    static public string shapesPath = "C:/files/Shapes/";
    static public string photoSpheresPath = "C:/files/PhotoSphere/";
    static public string userImagesPath = "C:/files/userImages/";
    static public string screenshotsPath = "C:/screenshots/";

#else

    static public string imagesPath = Application.dataPath + "/media/appData/images/";
    static public string shapesPath = Application.dataPath + "/media/appData/shapes/";
    static public string photoSpheresPath = Application.dataPath + "/media/appData/sfery/";
    static public string userImagesPath = Application.dataPath + "/media/upload/";
    static public string screenshotsPath = Application.dataPath + "/media/screenshots/";
#endif
    static public UserType userType = UserType.student;
    static public string idFilePath = Application.dataPath + "/login.txt";
#endregion

#region Private Properties

    private static List<ImagesInfo> imagesInfo;
    private static List<ImagesInfo> shapesInfo;
    private static List<PhotoSpheresInfo> photoSpheresInfo;
    private static List<ImagesInfo> userImagesInfo;

    private static ImageObject[] imageObjects;
    private static ShapeObject[] shapeObjects;
    private static PhotoSphere[] photoSphereObjects;
    private static ImageObject[] userImageObjects;

    private static Color avatarColor;

#endregion

#region Static Methods
    public static List<ImagesInfo> GetImagesInfo()
    {
        if (imagesInfo == null)
        {
            LoadImagesInfo();
        }

        return imagesInfo;
    }

    public static List<ImagesInfo> GetUserImagesInfo()
    {
        if (userImagesInfo == null)
        {
            LoadUserImagesInfo();
        }

        return userImagesInfo;
    }


    public static List<ImagesInfo> GetShapesInfo()
    {
        if (shapesInfo == null)
        {
            LoadShapesInfo();
        }

        return shapesInfo;
    }

    public static List<PhotoSpheresInfo> GetPhotoSpheresInfo()
    {
        if (photoSpheresInfo == null)
        {
            LoadPhotoSpheresInfo();
        }

        return photoSpheresInfo;
    }

    public static void LoadAllData()
    {
        if (IsSuperUser())
        {
            LoadImagesInfo();
            LoadShapesInfo();
            LoadPhotoSpheresInfo();

            LoadImages();
            LoadShapes();
            LoadPhotoSpheres();
        }
        
    }

    private static void LoadImagesInfo()
    {
        if (imagesInfo == null)
        {
            try
            {
                //ImageFilesInfoLoader loader = new ImageFilesInfoLoader(Application.dataPath + "/media/images/");
                ImageFilesInfoLoader loader = new ImageFilesInfoLoader(imagesPath);
                string[] extensions = { ".jpg", ".bmp", ".jpeg", ".png" };
                imagesInfo = loader.LoadImagesInfo(extensions);
            }
            catch (Exception e)
            {
                Debug.Log("Unable to load images!");
                Debug.Log(e);
            }
        }
        
    }
    private static void LoadUserImagesInfo()
    {
        if (userImagesInfo == null)
        {
            try
            {
                //ImageFilesInfoLoader loader = new ImageFilesInfoLoader(Application.dataPath + "/media/images/");
                ImageFilesInfoLoader loader = new ImageFilesInfoLoader(userImagesPath);
                string[] extensions = { ".jpg", ".bmp", ".jpeg", ".png" };
                userImagesInfo = loader.LoadImagesInfo(extensions, "", 50, 2000000);
            }
            catch (Exception e)
            {
                Debug.Log("Unable to load user images!");
                Debug.Log(e);
            }
        }

    }


    private static void LoadShapesInfo()
    {
        if (shapesInfo == null)
        {
            try
            {
                ImageFilesInfoLoader loader = new ImageFilesInfoLoader(shapesPath);
                //ImageFilesInfoLoader loader = new ImageFilesInfoLoader(Application.dataPath + "/media/Shapes/");

                string[] extensions = { ".png" };
                shapesInfo = loader.LoadImagesInfo(extensions);
            }
            catch (Exception e)
            {
                Debug.Log("Unable to load shapes!");
                Debug.Log(e);
            }
        }
        
    }

    private static void LoadPhotoSpheresInfo()
    {
        if (photoSpheresInfo == null)
        {
            try
            {
                PhotoSpheresInfoLoader loader = new PhotoSpheresInfoLoader(photoSpheresPath);
                //PhotoSpheresInfoLoader loader = new PhotoSpheresInfoLoader(Application.dataPath + "/media/photospheres/");

                string[] extensions = { ".mat" };
                photoSpheresInfo = loader.LoadPhotoSpheresInfo(extensions);
            }
            catch (Exception e)
            {
                Debug.Log("Unable to load photospheres!");
                Debug.Log(e);
            }
        }
        
    }

    public static ImageObject[] GetImages()
    {
        LoadImages();
        ImageObject [] z = new ImageObject[imageObjects.Length + userImageObjects.Length];
        imageObjects.CopyTo(z, 0);
        userImageObjects.CopyTo(z, imageObjects.Length);

        return z;
    }


    public static ShapeObject[] GetShapes()
    {
       LoadShapes();
        return shapeObjects;
    }


    public static PhotoSphere[] GetPhotoSpheres()
    {
        LoadPhotoSpheres();
        return photoSphereObjects;
    }

    private static void LoadImages()
    {
        if (imageObjects == null)
        {
            if (imagesInfo != null)
            {
                imageObjects = new ImageObject[imagesInfo.Count];
                int index = 0;
                foreach (ImagesInfo img in imagesInfo)
                {
                    imageObjects[index] = new ImageObject(img.width, img.height, img.name, img.path);
                    index++;
                }
            }
        }
    }

    public static void LoadUserImagesFromServer(List<ImagesInfo> serverList)
    {
        if (userImageObjects == null)
        {
            if (serverList != null)
            {
                userImageObjects = new ImageObject[serverList.Count];
                int index = 0;
                foreach (ImagesInfo img in serverList)
                {
                    userImageObjects[index] = new ImageObject(img.width, img.height, img.path+"/"+ img.name, "", LoadingType.remote, img.path+ "/miniatures/" + img.name, false);
                    index++;
                }
            }
        }
    }


    private static void LoadShapes()
    {
        if (shapeObjects == null)
        {
            if (shapesInfo != null)
            {
                shapeObjects = new ShapeObject[shapesInfo.Count];
                int index = 0;
                Color color = Color.white;
                foreach (ImagesInfo shape in shapesInfo)
                {
                    shapeObjects[index] = new ShapeObject(shape.width, shape.height, shape.name, shape.path, color);
                    index++;
                }
            }
        }   
    }



    private static void LoadPhotoSpheres()
    {
        if (photoSphereObjects == null)
        {
            if (photoSpheresInfo != null)
            {
                photoSphereObjects = new PhotoSphere[photoSpheresInfo.Count];
                int index = 0;
                foreach (PhotoSpheresInfo sphere in photoSpheresInfo)
                {
                    photoSphereObjects[index] = new PhotoSphere(sphere.sphereName);
                    index++;
                }
            }
        }
    }

    public static Color GetAvatarColor()
    {
        if (avatarColor == new Color(0.0f, 0.0f, 0.0f, 0.0f))
        {
            avatarColor = HelperFunctionsScript.GetRandomColor();
        }

        return avatarColor;
    }


    public static bool IsRoomMine()
    {
        if (userRoom == roomToConnectName) return true;
        else return false;
    }


    public static bool CanInteract()
    {
        return userType >= UserType.studentWithPriviliges || IsRoomMine();
    }

    public static bool IsSuperUser()
    {
        return userType == UserType.teacher;
    }

#endregion
}
