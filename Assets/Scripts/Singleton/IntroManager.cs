using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;


public class IntroManager : MonoBehaviourPunCallbacks
{
     public static  IntroManager inst;

    private void Awake()
    {
        inst = this;
    }
    // Start is called before the first frame update


    public void GoSAtart()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "1.0";
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        //test();
        
        SceneManager.LoadScene("01_Loby");
    }

    void test()
    {
        string name = "Hey" + UnityEngine.Random.Range(0, 1000000);
        PhotonNetwork.LocalPlayer.NickName = name;
        GameManager.Inst.NickName = name;
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers =8;
        PhotonNetwork.JoinOrCreateRoom("asd", roomOptions,null);
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PhotonNetwork.IsMessageQueueRunning = false;
            SceneManager.LoadScene("03_Main");
        }
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("실행가능");

    }
}

