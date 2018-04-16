using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Firebase.Storage;
using Firebase.Database;

public class firebaseManager : MonoBehaviour {

    public bool uploading;

    public FirebaseStorage storage;
    public StorageReference rivers_ref;

    public FirebaseDatabase db;
    public DatabaseReference debRef;
    private int counter = 0;

    public VideoStreaming streamingScript;

    void Start()
    {
        storage = FirebaseStorage.DefaultInstance;
        StorageReference storage_ref = storage.GetReferenceFromUrl("gs://cloudstories-7de0a.appspot.com/mit_upload");
        rivers_ref = storage_ref.Child(ApplicationStaticData.roomToConnectName);

        db = FirebaseDatabase.DefaultInstance;
        DatabaseReference dbRef = db.GetReferenceFromUrl("https://cloudstories-7de0a.firebaseio.com/mit");
        debRef = dbRef.Child(ApplicationStaticData.roomToConnectName);
    }

    public void upload(byte[] array) {
        uploading = true;

        rivers_ref.Child(counter.ToString() + ".jpg").PutBytesAsync(array).ContinueWith((Task<StorageMetadata> task) => {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log(task.Exception.ToString());
            }
            else
            {
                debRef.Child("LastCorrectFrame").SetValueAsync(counter.ToString() + ".jpg");
                counter = (counter + 1)%100;
                Debug.Log("uploaded succesful");
            }

            uploading = false;
            streamingScript.streaming = false;
        });
    }

    public void download()
    { 
        debRef.Child("LastCorrectFrame").GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                Debug.Log(task.Exception.ToString());
                streamingScript.streaming = false;
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                string filename = (string)snapshot.Value;

                StorageReference fileRef = rivers_ref.Child(filename);

                const long maxAllowedSize = 1 * 1024 * 1024;
                fileRef.GetBytesAsync(maxAllowedSize).ContinueWith((Task<byte[]> task2) => {
                    if (task2.IsFaulted || task2.IsCanceled)
                    {
                        Debug.Log(task2.Exception.ToString());

                    }
                    else
                    {
                        streamingScript.textureBytes = task2.Result;
                        streamingScript.frameReady = true;
                        Debug.Log("Finished downloading file "+ filename + "!");
                    }
                    streamingScript.streaming = false;

                });

            }
        });

    }
    
}
