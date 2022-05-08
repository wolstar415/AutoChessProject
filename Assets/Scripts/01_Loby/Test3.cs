using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Firebase.Storage;

public class Test3 : MonoBehaviour
{

    [SerializeField]AssetReference Ref;
    // Start is called before the first frame update
    void Start()
    {

    }
public void asd()
    {
        //StorageReference gsReference =
        //    storage.GetReferenceFromUrl("gs://bucket/images/stars.jpg");
        Addressables.DownloadDependenciesAsync("logo").Completed += (AsyncOperationHandle handle) => {
            if (handle.IsDone)
            {
            Debug.Log("다운로드2");

            }
            Debug.Log("다운로드");
            Addressables.Release(handle);
            Addressables.InstantiateAsync("ccc", new Vector3(0, 0, 0), Quaternion.identity);
        };

        // 
    }
}
