using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using Hashtable = ExitGames.Client.Photon.Hashtable;

[System.Serializable]
public class playerinfo
{
    public Player player;
    public String NickName;
    public String userId;
    public int Num;
    public GameObject playerob;
    public int ConnectState;

}

    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        public List<playerinfo> players;
     

        public PhotonView pv;
        private void Start()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Master();
            }
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                GameObject ob = PhotonNetwork.Instantiate("PLayer", new Vector3(0, 0, 0), Quaternion.identity);
                players.Add(new playerinfo());
                players[i].player = PhotonNetwork.PlayerList[i];
                players[i].NickName = PhotonNetwork.PlayerList[i].NickName;
                players[i].userId = PhotonNetwork.PlayerList[i].UserId;
                players[i].Num = PhotonNetwork.PlayerList[i].ActorNumber;
                players[i].playerob = ob;
                
                ob.SetActive(false);
                if (PhotonNetwork.PlayerList[i]==PhotonNetwork.LocalPlayer)
                {
                    PlayerInfo.Inst.PlayerIdx = i;
                Debug.Log(i);
                }
            }

            //pv.RPC(nameof(IdxCheck), RpcTarget.All);

        }

        void Master()
        {
            
        }
        [PunRPC]
        void IdxCheck()
        {
            PlayerInfo.Inst.PlayerIdx = (int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerIdx"];
        }
    }

