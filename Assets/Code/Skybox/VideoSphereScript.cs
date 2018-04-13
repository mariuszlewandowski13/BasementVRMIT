#region Usings

using UnityEngine;

#endregion

public class VideoSphereScript : MonoBehaviour {

    #region Private Properties

    private VideoSphere videoSphere = new VideoSphere(100, 100, 100);

    #endregion


    #region Methods
    void Start () {
        SetScale();

    }

    private void SetScale()
    {
        float scaleX = videoSphere.scaleX;
        scaleX -= transform.localScale.x;

        float scaleY = videoSphere.scaleY;
        scaleY -= transform.localScale.y;

        float scaleZ = videoSphere.scaleZ;
        scaleZ -= transform.localScale.z;

        transform.localScale += new Vector3(scaleX, scaleY, scaleZ);
    }

    #endregion
}
