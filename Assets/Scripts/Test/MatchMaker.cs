using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;
using DG.Tweening.Plugins.Core.PathCore;

public class MatchMaker : MonoBehaviourPunCallbacks, IPunPrefabPool
{

    public AssetReference[] obs;
    public List<GameObject> gameObjects;


    async void Start()
    {
        //PhotonNetwork.PrefabPool = this;
        
        gameObjects = new List<GameObject>();

        DefaultPool defaultPool = PhotonNetwork.PrefabPool as DefaultPool;

        foreach (var ob in obs)
        {

            Task<GameObject> task = ob.LoadAssetAsync<GameObject>().Task;

            await task;
            gameObjects.Add(task.Result);
            defaultPool.ResourceCache.Add(task.Result.name, task.Result);
            ob?.ReleaseAsset();
        }

        //defaultPool.ResourceCache.Add(Checkob.name, Checkob);
        Debug.Log("오브젝트를 캐시에 추가.");

        Debug.Log("start");
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = "0.1";
    }

    

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster() was called by PUN");

        PhotonNetwork.JoinRandomRoom();
    }


    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room.");

        float randomX = Random.Range(-6f, 6f);

        // PhotonNetwork.Instantiate(gameObjects[0].name, new Vector3(randomX, 0f, 0f), Quaternion.identity);
        // PhotonNetwork.Instantiate(gameObjects[1].name, new Vector3(randomX, 0f, 0f), Quaternion.identity);
        // PhotonNetwork.Instantiate(gameObjects[2].name, new Vector3(randomX, 0f, 0f), Quaternion.identity);


        

    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

        // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
        PhotonNetwork.CreateRoom(null);

        //PhotonNetwork.CreateRoom(null, new RoomOptions{MaxPlayers = 4});
    }


    public GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation)
    {
        Debug.Log("asas");
        //throw new System.NotImplementedException();
        return null;
    }

    public void Destroy(GameObject gameObject)
    {
        Debug.Log("파괴");
    }
}