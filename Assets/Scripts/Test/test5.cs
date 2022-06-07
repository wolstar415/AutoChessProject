using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class test5 : MonoBehaviourPunCallbacks
{
    void Awake()
    {
        Screen.SetResolution(960, 540, false);
        PhotonNetwork.ConnectUsingSettings();
    }


    public override void OnConnectedToMaster()
    {
        RoomOptions roomOption = new RoomOptions();
        roomOption.MaxPlayers = 6;
        roomOption.CustomRoomProperties = new Hashtable() { { "키1", "문자열" }, { "키2", 1 } };

        PhotonNetwork.JoinOrCreateRoom("Room", roomOption, null);
    }


    public override void OnJoinedRoom()
    {
        Hashtable CP = PhotonNetwork.CurrentRoom.CustomProperties;

        print(CP["키1"]);

        CP["키1"] = "gd";

        CP.Add("키3", 0.5f);

        print(CP.Count);

        CP.Remove("키3");



        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "IsAdmin", "Admin" } });

            print(PhotonNetwork.LocalPlayer.CustomProperties["키1"]);
            print(PhotonNetwork.LocalPlayer.CustomProperties["키2"]);
            print(PhotonNetwork.LocalPlayer.CustomProperties["IsAdmin"]);
            Hashtable playerCP = PhotonNetwork.LocalPlayer.CustomProperties;

            print(playerCP["IsAdmin"]);

            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                PhotonNetwork.PlayerList[i].SetCustomProperties(new Hashtable { { "IsAdmin", "Admin" } });
            }
            
        }
    }
}
