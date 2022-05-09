using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Firebase.Storage;
using Firebase.Extensions;
using UnityEngine.ResourceManagement.ResourceProviders;
using RobinBird.FirebaseTools.Storage.Addressables;
using Photon.Pun;
using Photon.Realtime;

using System.Threading.Tasks;

public class Test3 : MonoBehaviourPunCallbacks
{

    public AudioSource audios;
    public AudioClip audioss;
    [SerializeField]AssetReference Ref;
    [SerializeField]GameObject zzz;
    // Start is called before the first frame update
    public AssetReference photonObject;

    List<GameObject> gameObjects;


    void Start()
    {
        Addressables.ResourceManager.ResourceProviders.Add(new FirebaseStorageAssetBundleProvider());
        Addressables.ResourceManager.ResourceProviders.Add(new FirebaseStorageJsonAssetProvider());
        Addressables.ResourceManager.ResourceProviders.Add(new FirebaseStorageHashProvider());

        // This requires Addressables >=1.75 and can be commented out for lower versions
        Addressables.InternalIdTransformFunc += FirebaseAddressablesCache.IdTransformFunc;
        PhotonNetwork.GameVersion = "0.1";
        PhotonNetwork.ConnectUsingSettings();


        GameObject Ob = Resources.Load<GameObject>("Capsule3");
        Instantiate(Ob, new Vector3(-3, 0, 0), Quaternion.identity);

    }
 

    public override void OnJoinedLobby()
    {
        //PhotonNetwork.JoinOrCreateRoom("asd",new RoomOptions { MaxPlayers=2},null);
        PhotonNetwork.CreateRoom("zxczxczxc", new RoomOptions { MaxPlayers = 2 });
        Debug.Log("ㅁㄴㅇ");
    }
    public override void OnJoinedRoom()
    {
        Debug.Log(gameObjects[0].name);
        PhotonNetwork.Instantiate("Capsule", new Vector3(0, -4, 0), Quaternion.identity);
    }


    public void asd()
    {
        //StorageReference gsReference =
        //    storage.GetReferenceFromUrl("gs://bucket/images/stars.jpg");
        FirebaseAddressablesManager.IsFirebaseSetupFinished = true;
        Addressables.DownloadDependenciesAsync("GoGo").Completed += (AsyncOperationHandle handle) => {
            if (handle.IsDone)
            {
                GameObject Ob = Resources.Load<GameObject>("Capsule2");
                Instantiate(Ob,new Vector3(-3,0,0), Quaternion.identity);   
            Debug.Log("다운로드2");
                Addressables.InstantiateAsync("ccc", new Vector3(0, 0, 0), Quaternion.identity).Completed += (AsyncOperationHandle<GameObject> obj) => { };
            }
            Debug.Log("다운로드");
        };

        // 
    }
    public void asd2()
    {
        Addressables.InstantiateAsync("ccc", new Vector3(0,-3, 0), Quaternion.identity).Completed += (AsyncOperationHandle<GameObject> obj) => { };

    }
    public void asd3()
    {
        Instantiate(zzz, new Vector3(0, -5, 0), Quaternion.identity);

    }
    public void asd4()
    {
        Addressables.LoadAssetAsync<AudioClip>("music").Completed += (AsyncOperationHandle<AudioClip> obj) =>
        {
            if (obj.Status== AsyncOperationStatus.Succeeded)
            {
                audioss = obj.Result;
        audios.clip = audioss;
        audios.Play();

            } };
        //Addressables.Release(obj);
    }
    public void asd5()
    {
        PhotonNetwork.JoinLobby();
        //
    }


}
