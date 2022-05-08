using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class AddressableTest : MonoBehaviour
{
    [SerializeField] GameObject obj;
    [SerializeField] AssetReference Ref;
    [SerializeField] Image asd;
    AsyncOperationHandle Handle;
    void Start()
    {
        Ref.InstantiateAsync(obj.transform);
       // _Click1();
    }


    public void _Click1()
    {
        Addressables.LoadAssetAsync<Sprite>("logo").Completed += (AsyncOperationHandle<Sprite> Ob) => {
            Handle = Ob;
            asd.sprite = Ob.Result;
        
        };
        //Addressables.DownloadDependencies().
    }
}
