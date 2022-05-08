using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Firebase.Storage;
using Firebase.Extensions;
using UnityEngine.ResourceManagement.ResourceProviders;
using RobinBird.FirebaseTools.Storage.Addressables;

public class Test3 : MonoBehaviour
{

    
    [SerializeField]AssetReference Ref;
    // Start is called before the first frame update
    void Start()
    {
        Addressables.ResourceManager.ResourceProviders.Add(new FirebaseStorageAssetBundleProvider());
        Addressables.ResourceManager.ResourceProviders.Add(new FirebaseStorageJsonAssetProvider());
        Addressables.ResourceManager.ResourceProviders.Add(new FirebaseStorageHashProvider());

        // This requires Addressables >=1.75 and can be commented out for lower versions
        Addressables.InternalIdTransformFunc += FirebaseAddressablesCache.IdTransformFunc;

    }


    public void asd()
    {
        //StorageReference gsReference =
        //    storage.GetReferenceFromUrl("gs://bucket/images/stars.jpg");
        FirebaseAddressablesManager.IsFirebaseSetupFinished = true;
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
