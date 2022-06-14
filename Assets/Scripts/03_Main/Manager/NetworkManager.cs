using System;
using System.Collections;
using System.Collections.Generic;
using GameS;
using Newtonsoft.Json;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Random = UnityEngine.Random;

[System.Serializable]
public class playerinfo
{
    public Player player;
    public int Idx;
    public String OriNickName;
    public String NickName;
    public int State;
    public bool Dead = false;
    public int Life = 100;
    //0 : 없음
    //1 : 연결중
    //2 : 관전중
    //3 : 나감
    public bool IsPickBool = false;
}

    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        public static NetworkManager inst;
        public List<playerinfo> players;
     

        public PhotonView pv;

        private void Awake()
        {
            inst = this;
        }

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

        public void RoundFuncGo(int idx)
        {
            pv.RPC(nameof(NetworkRoundFuncGo),RpcTarget.All,idx);
        }

        [PunRPC]
        void NetworkRoundFuncGo(int idx)
        {
            switch (idx)
            {
                case 1:
                    RoundManager.inst.Round_PVP_EndFunc();
                    break;
                case 2:
                    RoundManager.inst.Round_PVE_EndFunc();
                    break;
                case 3:
                    RoundManager.inst.Round_PICk_EndFunc();
                    break;
                default:
                    Debug.Log("버그");
                    break;
            }
        }

        public void PickAllOpen()
        {
            pv.RPC(nameof(NetworkPickAllOpen),RpcTarget.All);
        }

        public void PickOpen(int idx)
        {
            
            pv.RPC(nameof(NetworkPickOpen),RpcTarget.All,idx);
        }

        [PunRPC]
        void NetworkPickAllOpen()
        {
            PickRoundManager.inst.PickAllOpen();
            if (PlayerInfo.Inst.Dead==false)
            {
            PlayerInfo.Inst.IsPick = true;
            }
        }
        [PunRPC]
        void NetworkPickOpen(int idx)
        {
            PickRoundManager.inst.PickOpen(idx);
            if (PlayerInfo.Inst.PlayerIdx==idx)
            {
                if (PlayerInfo.Inst.Dead==false)
                {
                    PlayerInfo.Inst.IsPick = true;
                }
            }
            
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
                players[i].Life = 100;
                players[i].Idx = i;
                MasterInfo.inst.lifeCheck.Add(new LifeRank(i,100));
                pv.RPC(nameof(PlayerSetting),RpcTarget.All,i,players[i].OriNickName);
                //PhotonNetwork.PlayerList[i].CustomProperties.Add("PlayerIdx",i);

            }
            
            
            StartCoroutine(masterSetting());
        }

        [PunRPC]
        void PlayerSetting(int idx,string name)
        {
            UIManager.inst.PlayerUiSetting(idx,name);
        }

        IEnumerator masterSetting()
        {
            yield return YieldInstructionCache.WaitForSeconds(0.5f);
            pv.RPC(nameof(IdxCheck),RpcTarget.All);
            yield return YieldInstructionCache.WaitForSeconds(0.5f);
            MasterRoundStart(0);
            
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
            if (ob.TryGetComponent(out PlayerMoving player))
            {
                player.NickNameSetting(GameManager.inst.NickName);
            }
        }

        [PunRPC]
        void CameraMovePick()
        {
            PlayerInfo.Inst.camer.transform.position = PositionManager.inst.Camera_PickPos.position;
            PlayerInfo.Inst.camer.transform.rotation = PositionManager.inst.Camera_PickPos.rotation;
            UIManager.inst.PickUiSetting();
        }

        [PunRPC]
        void CameraMoveNormal(int i)
        {
            int idx = PlayerInfo.Inst.PlayerIdx;
            PlayerInfo.Inst.camer.transform.position = PositionManager.inst.Camera_Pos[idx].position;
            PlayerInfo.Inst.camer.transform.rotation = PositionManager.inst.Camera_Pos[idx].rotation;
            if (i==1)
            {
                UIManager.inst.BattleUiSetting();
            }
        }
        void CameraMoveNormal2(int i)
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

        public void MasterRoundStart(int Round)
        {
            pv.RPC(nameof(NetworkRoundStart),RpcTarget.All,Round);
        }

        [PunRPC]
        void NetworkRoundStart(int Round)
        {
            if (Round==0)
            {
                GameSystem_AllInfo.inst.StartFunc();
            }
            RoundManager.inst.RoundStart(Round);
        }



        public void PickSelect()
        {
            pv.RPC(nameof(NetWorkPickSelct),RpcTarget.MasterClient,PlayerInfo.Inst.PlayerIdx);
        }

        [PunRPC]
        void NetWorkPickSelct(int PlayerIdx)
        {
            MasterInfo.inst.player_PickCheck[PlayerIdx] = 0;
            NetworkManager.inst.players[PlayerIdx].IsPickBool = false;

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
            else if (Input.GetKeyDown(KeyCode.Keypad4))
            {
                PlayerInfo.Inst.EnemyIdx = 1;
                PlayerInfo.Inst.BattleMove = true;
                RoundManager.inst.BattleMoveFunc();
            }
            else if (Input.GetKeyDown(KeyCode.Keypad5))
            {
                //PlayerInfo.Inst.EnemyIdx = -1;
               // PlayerInfo.Inst.BattleMove = false;
                RoundManager.inst.BattleMoveFunc2();
            }
            else if (Input.GetKeyDown(KeyCode.Keypad9))
            {
                PlayerInfo.Inst.Gold += 5;

            }
        }

        public void NoSelectPickFunc()
        {
            for (int i = 0; i < NetworkManager.inst.players.Count; i++)
            {
                if (NetworkManager.inst.players[i].State==1)
                {
                    if (MasterInfo.inst.player_PickCheck[i]==1)
                    {
                        int ran = Random.Range(0, MasterInfo.inst.pickCards.Count);
                        int idx = 0;
                        if (MasterInfo.inst.pickCards[ran].TryGetComponent(out Card_Info info))
                        {
                            info.pickIdx = idx;
                        }
                        MasterInfo.inst.pickCards.RemoveAt(ran);
                        pv.RPC(nameof(NetworkNoSelectPickFunc),RpcTarget.All,i,idx);
                        //너 강제로 선택해.
                    }
                }
            }
        }

        [PunRPC]
        void NetworkNoSelectPickFunc(int Playeridx, int Idx)
        {
            if (PlayerInfo.Inst.PlayerIdx==Playeridx&&PlayerInfo.Inst.IsPick)
            {
                GameObject ob = GameSystem_AllInfo.inst.PickCard[Idx];
                if (PlayerInfo.Inst.PlayerOb.TryGetComponent(out PlayerPickSelect pick))
                {
                    pick.PickOb(ob);
                }
            }
        }

        public void MasterInfoOrder()
        {
            
            string c=MasterInfo.inst.LifeOrder2();
            pv.RPC(nameof(RPC_PlayerInfoOrder),RpcTarget.All,c);
        }

        [PunRPC]
        void RPC_PlayerInfoOrder(string s)
        {
            MasterInfo.inst.lifeRank2=JsonConvert.DeserializeObject<List<LifeRank>>(s);
            UIManager.inst.PlayerInfoOrder();
        }
    }

