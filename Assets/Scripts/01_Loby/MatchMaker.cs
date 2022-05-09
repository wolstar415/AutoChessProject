using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;

public class MatchMaker : MonoBehaviourPunCallbacks
{

    public AssetReference photonObject;

    public List<GameObject> gameObjects;

    async void Start()
    {
        gameObjects = new List<GameObject>();

        DefaultPool defaultPool = PhotonNetwork.PrefabPool as DefaultPool;

        Debug.Log("a1");
        Task<GameObject> task = photonObject.LoadAssetAsync<GameObject>().Task;
        Debug.Log("a2");
        await task;


        Debug.Log("a3");
        gameObjects.Add(task.Result);

        defaultPool.ResourceCache.Add(gameObjects[0].name, gameObjects[0]);
        Debug.Log("오브젝트를 캐시에 추가.");

        Debug.Log("start");
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = "0.1";
    }

    private void OnDestroy()
    {
        photonObject.ReleaseAsset();
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

        PhotonNetwork.Instantiate(
            gameObjects[0].name,
            new Vector3(randomX, 0f, 0f),
            Quaternion.identity,
            0
        );


    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

        // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
        PhotonNetwork.CreateRoom(null);

        //PhotonNetwork.CreateRoom(null, new RoomOptions{MaxPlayers = 4});
    }


}
