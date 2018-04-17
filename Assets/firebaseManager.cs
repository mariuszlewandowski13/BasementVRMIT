using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Firebase.Storage;

public class firebaseManager : MonoBehaviour {

    public bool uploading;

    public FirebaseStorage storage;
    public StorageReference rivers_ref;

    private int counter = 0;

    public VideoStreaming streamingScript;

    void Start()
    {
        storage = FirebaseStorage.DefaultInstance;
        StorageReference storage_ref = storage.GetReferenceFromUrl("gs://bvr-mit.appspot.com/mit_upload");
        rivers_ref = storage_ref.Child(ApplicationStaticData.roomToConnectName);

    }

    public void upload(byte[] array, string filename) {
        uploading = true;

        rivers_ref.Child(filename).PutBytesAsync(array).ContinueWith((Task<StorageMetadata> task) => {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log(task.Exception.ToString());
            }
            else
            {
                Debug.Log("uploaded succesful");
            }

            uploading = false;
            streamingScript.streaming = false;
        });
    }

    public void download(string filename)
    { 

                StorageReference fileRef = rivers_ref.Child(filename);
        Debug.Log(fileRef.Path);
                const long maxAllowedSize = 1024 * 1024 * 1024;
                fileRef.GetBytesAsync(maxAllowedSize).ContinueWith((Task<byte[]> task2) => {
                    if (task2.IsFaulted || task2.IsCanceled)
                    {
                        Debug.Log(task2.Exception.ToString());

                    }
                    else
                    {
                        streamingScript.textureBytes = task2.Result;
                        streamingScript.frameReady = true;
                        Debug.Log("Finished downloading file 0.jpg!");
                    }
                    streamingScript.streaming = false;

        });

    }
    
}
