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
    public String OriNickName;
    public String NickName;
    public int State;
    public bool Dead = false;
    //0 : 없음
    //1 : 연결중
    //2 : 관전중
    //3 : 나감

}

    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        public List<playerinfo> players;
     

        public PhotonView pv;
        private void Start()
        {
            
            
            PhotonNetwork.IsMessageQueueRunning = true;
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {

                if (PhotonNetwork.PlayerList[i]==PhotonNetwork.LocalPlayer)
                {
                    PlayerInfo.Inst.PlayerIdx = i;
                }

            }
            
            if (PhotonNetwork.IsMasterClient)
            {
                Master();
            }


            //pv.RPC(nameof(IdxCheck), RpcTarget.All);

        }

        void Master()
        {
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                players.Add(new playerinfo());
                players[i].player = PhotonNetwork.PlayerList[i];
                players[i].OriNickName = PhotonNetwork.PlayerList[i].NickName;
                string[] s = PhotonNetwork.PlayerList[i].NickName.Split("@");
                players[i].NickName = s[0];
                players[i].State = 1;
                //PhotonNetwork.PlayerList[i].CustomProperties.Add("PlayerIdx",i);

            }
            
            
            StartCoroutine(masterSetting());
        }

        IEnumerator masterSetting()
        {
            yield return YieldInstructionCache.WaitForSeconds(0.5f);
            pv.RPC(nameof(IdxCheck),RpcTarget.All);
            yield return YieldInstructionCache.WaitForSeconds(0.5f);
            pv.RPC(nameof(CameraMovePick),RpcTarget.All);
            yield return YieldInstructionCache.WaitForSeconds(1);
            pv.RPC(nameof(CameraMoveNormal),RpcTarget.All,1);
            
        }
        [PunRPC]
        void IdxCheck()
        {
            

            int idx = PlayerInfo.Inst.PlayerIdx;
            Vector3 pos =PositionManager.inst.PickPos[idx].position;
            GameObject ob = PhotonNetwork.Instantiate("Player", pos, Quaternion.identity);
            PlayerInfo.Inst.PlayerOb = ob;
            PlayerInfo.Inst.PickRound = true;
            PosSetting();
        }

        [PunRPC]
        void CameraMovePick()
        {
            PlayerInfo.Inst.camer.transform.position = PositionManager.inst.Camera_PickPos.position;
            PlayerInfo.Inst.camer.transform.rotation = PositionManager.inst.Camera_PickPos.rotation;
            UIManager.inst.PickUiSetting();
        }

        [PunRPC]
        void CameraMoveNormal(int i=0)
        {
            int idx = PlayerInfo.Inst.PlayerIdx;
            PlayerInfo.Inst.camer.transform.position = PositionManager.inst.Camera_Pos[idx].position;
            PlayerInfo.Inst.camer.transform.rotation = PositionManager.inst.Camera_Pos[idx].rotation;
            if (i==1)
            {
                UIManager.inst.BattleUiSetting();
            }
        }
        void CameraMoveNormal2(int i=0)
        {
            int idx = PlayerInfo.Inst.PlayerIdx;
            PlayerInfo.Inst.camer.transform.position = PositionManager.inst.Camera_AttackPos[idx].position;
            PlayerInfo.Inst.camer.transform.rotation = PositionManager.inst.Camera_AttackPos[idx].rotation;
            if (i==1)
            {
                UIManager.inst.BattleUiSetting();
            }
        }

        void PosSetting()
        {
            int idx = PlayerInfo.Inst.PlayerIdx;
            PlayerInfo.Inst.PlayerTile = PositionManager.inst.playerPositioninfo[idx].PlayerTile;
            PlayerInfo.Inst.FiledTile = PositionManager.inst.playerPositioninfo[idx].FiledTile;
            PlayerInfo.Inst.GoldOb = PositionManager.inst.playerPositioninfo[idx].GoldOb;
            PlayerInfo.Inst.PlayerMovePos = PositionManager.inst.playerPositioninfo[idx].PlayerMovePos;
            
            PlayerInfo.Inst.EnemyTile = PositionManager.inst.playerPositioninfo[idx].EnemyPlayerTile;
            PlayerInfo.Inst.EnemyFiledTile = PositionManager.inst.playerPositioninfo[idx].EnemyFiledTile;
            PlayerInfo.Inst.EnemyGoldOb = PositionManager.inst.playerPositioninfo[idx].EnemyGoldOb;
            PlayerInfo.Inst.EnemyPlayerMovePos = PositionManager.inst.playerPositioninfo[idx].EnemyPlayerMovePos;
            
            PlayerInfo.Inst.FiledTileOb = PositionManager.inst.playerPositioninfo[idx].FiledTileOb;
            PlayerInfo.Inst.PlayerTileOb = PositionManager.inst.playerPositioninfo[idx].PlayerTileob;
            PlayerInfo.Inst.EnemyPlayerTileob = PositionManager.inst.playerPositioninfo[idx].EnemyPlayerTileob;



        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                CameraMovePick();
            }
            else if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                CameraMoveNormal(1);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                CameraMoveNormal2(1);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad9))
            {
                PlayerInfo.Inst.Gold += 5;

            }
        }
    }

