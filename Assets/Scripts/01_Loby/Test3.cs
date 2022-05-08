using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Firebase.Storage;
using Firebase.Extensions;

public class Test3 : MonoBehaviour
{

    
    [SerializeField]AssetReference Ref;
    // Start is called before the first frame update
    void Start()
    {
        FirebaseStorage storage;
        storage = FirebaseStorage.DefaultInstance;
        StorageReference storageRef = storage.GetReferenceFromUrl("gs://unity-autochess.appspot.com");
        Debug.Log(storageRef);
    }
    public void asd2()
    {
        

    }
public void asd()
    {
        //StorageReference gsReference =
        //    storage.GetReferenceFromUrl("gs://bucket/images/stars.jpg");
        Addressables.DownloadDependenciesAsync("GoGo").Completed += (AsyncOperationHandle handle) => {
            if (handle.IsDone)
            {
            Debug.Log("다운로드2");
                Addressables.InstantiateAsync("ccc", new Vector3(0, 0, 0), Quaternion.identity).Completed += (AsyncOperationHandle<GameObject> obj) => { };
            }
            Debug.Log("다운로드");
        };

        // 
    }
}
