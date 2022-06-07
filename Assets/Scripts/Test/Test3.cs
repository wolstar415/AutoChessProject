using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class Test3 : MonoBehaviourPunCallbacks
{

    private void Start()
    {
        
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "1.0";
        PhotonNetwork.ConnectUsingSettings();
         PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "1", 1 } });
         Debug.Log(PhotonNetwork.LocalPlayer.CustomProperties["1"]);
        
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers =8;
        PhotonNetwork.JoinOrCreateRoom("asd", roomOptions,null);
    }
    public override void OnJoinedRoom()
    {
        //PhotonNetwork.NetworkingClient.EventReceived += EventReceive;
        Debug.Log("완료");
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "1", 5 } });
        Debug.Log(PhotonNetwork.LocalPlayer.CustomProperties["1"]);

    }



}
