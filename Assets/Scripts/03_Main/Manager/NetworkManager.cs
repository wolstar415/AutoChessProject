using System;
using System.Collections;
using System.Collections.Generic;
using GameS;
using Newtonsoft.Json;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

[Serializable]
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
    public bool BattleEnd  = false;
    //public bool BattleMove = false;
}

    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        public static NetworkManager inst;
        public List<playerinfo> players;

        private Coroutine coTime;

        public PhotonView pv;
        public Color[] Dagamecolors;
        public Image[] Playericons;
        public GameObject[] Fireob;
        public TextMeshProUGUI[] PlayerTexts;
        public List<Card_Info> dummyInfo=new List<Card_Info>();
        public int Checkint = 0;

        private void Awake()
        {
            inst = this;
        }

        private void Start()
        {
            PhotonNetwork.IsMessageQueueRunning = true;
            pv.RPC(nameof(check),RpcTarget.MasterClient,PhotonNetwork.LocalPlayer.NickName);
            
        }

        private void StartFunc()
        {
            
            
            
            
            



            //pv.RPC(nameof(IdxCheck), RpcTarget.All);

        }

        [PunRPC]
        public void check(string name)
        {
            Checkint++;
            Debug.Log($"{name} 로딩 완료! {Checkint} / {PhotonNetwork.PlayerList.Length}");
            if (Checkint==PhotonNetwork.PlayerList.Length)
            {
                Debug.Log("로딩이 완료되었습니다 게임을 실행합니다.");
                MasterInfo.inst.VictoryInt = PhotonNetwork.PlayerList.Length;
                StartFunc();
                Master();
                pv.RPC(nameof(start2),RpcTarget.All);
            }
        }

        [PunRPC]
        public void start2()
        {
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {

                if (PhotonNetwork.PlayerList[i]==PhotonNetwork.LocalPlayer)
                {
                    PlayerInfo.Inst.PlayerIdx = i;
                    pv.RPC(nameof(RPC_iconSet), RpcTarget.All, i, GameManager.Inst.CharIdx);
                    PlayerTexts[i].color=Color.yellow;
                }

            }
        }

        public void RoundFuncGo(int idx)
        {
            pv.RPC(nameof(NetworkRoundFuncGo),RpcTarget.All,idx);
        }

        [PunRPC]
        void RPC_iconSet(int idx,int icon)
        {
            Playericons[idx].sprite = GameManager.Inst.charIcons[icon];
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
                pv.RPC(nameof(PlayerSetting),RpcTarget.All,i,players[i].NickName);
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
            MasterRoundFirst();
            
        }
        [PunRPC]
        void IdxCheck()
        {
            

            int idx = PlayerInfo.Inst.PlayerIdx;
            Vector3 pos =PositionManager.inst.PickPos[idx].position;
            GameObject ob = PhotonNetwork.Instantiate("Player"+GameManager.Inst.CharIdx, pos, Quaternion.identity);
            PlayerInfo.Inst.PlayerOb = ob;
            PlayerInfo.Inst.PickRound = true;
            PosSetting();
            if (ob.TryGetComponent(out PlayerMoving player))
            {
                player.NickNameSetting(GameManager.Inst.NickName);
            }
        }

        [PunRPC]
        void CameraMovePick()
        {
            PlayerInfo.Inst.camer.transform.position = PositionManager.inst.Camera_PickPos[PlayerInfo.Inst.PlayerIdx].localPosition;
            PlayerInfo.Inst.camer.transform.rotation = PositionManager.inst.Camera_PickPos[PlayerInfo.Inst.PlayerIdx].localRotation;
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
        public void  CameraMovePlayer(int playeridx,bool b)
        {

            if (b)
            {
                PlayerInfo.Inst.camer.transform.position = PositionManager.inst.Camera_AttackPos[playeridx].position;
                PlayerInfo.Inst.camer.transform.rotation = PositionManager.inst.Camera_AttackPos[playeridx].rotation;

            }
            else
            {
                PlayerInfo.Inst.camer.transform.position = PositionManager.inst.Camera_Pos[playeridx].position;
                PlayerInfo.Inst.camer.transform.rotation = PositionManager.inst.Camera_Pos[playeridx].rotation;

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

        public void MasterRoundFirst()
        {
            pv.RPC(nameof(NetworkRoundStart),RpcTarget.All,0);
        }

        public void MasterRoundNextStart()
        {
            int cnt = 0;
            int finalplayer = 0;
            for (int i = 0; i < players.Count; i++)
            {
                if (!players[i].Dead&&players[i].State==1&&players[i].Life>=1)
                {
                    cnt++;
                    finalplayer = i;
                }
            }
            if (cnt==1&&PhotonNetwork.PlayerList.Length>1)
            {
                //게임끝남
                Debug.Log($"승리! 남은플레이어 번호 :{finalplayer}");
                
                MasterInfo.inst.VictoryGo(1);
                return;
            }

            if (cnt==0)
            {
                Debug.Log("아무도없음 버그처리!");
            }

            int a = PlayerInfo.Inst.Round + 1;
            pv.RPC(nameof(NetworkRoundStart),RpcTarget.All,a);
        }

        [PunRPC]
        void NetworkRoundStart(int Round)
        {
            PlayerInfo.Inst.Round = Round;
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

        

        public void NoSelectPickFunc()
        {
            List<GameObject> check1=new List<GameObject>();
            for (int i = 0; i < MasterInfo.inst.pickCards.Count; i++)
            {
                if (MasterInfo.inst.pickCards[i]!=null)
                {
                    check1.Add(MasterInfo.inst.pickCards[i]);
                }
            }
            for (int i = 0; i < NetworkManager.inst.players.Count; i++)
            {
                if (NetworkManager.inst.players[i].State==1)
                {
                    if (MasterInfo.inst.player_PickCheck[i]==1)
                    {
                        int ran = Random.Range(0, check1.Count);
                        int idx = 0;
                        if (check1[ran].TryGetComponent(out Card_Info info))
                        {
                            idx = info.pickIdx;
                        }
                        check1.RemoveAt(ran);
                        pv.RPC(nameof(NetworkNoSelectPickFunc),RpcTarget.All,i,idx);
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

        public void TextUi(string text, Vector3 pos,float Size=1f,int coloridx=0, float gap = 0.1f)
        {
            pv.RPC(nameof(RPC_TextUi),RpcTarget.All,text,Size,coloridx,pos,gap);
        }

        [PunRPC]
        void RPC_TextUi(string text,float Size, int coloridx, Vector3 pos, float gap)
        {

            GameObject UI = ObjectPooler.SpawnFromPool("DamageText", pos, Quaternion.identity);
            if (UI.TryGetComponent(out UIHUDText hud))
            {
                hud.Play2(text,Dagamecolors[coloridx],pos,Size,gap);
            }
        }

        public void BattleFoodCheck()
        {
            pv.RPC(nameof(RPC_BattleFoodCheck),RpcTarget.All);
        }

        [PunRPC]
        public void RPC_BattleFoodCheck()
        {
            
            if (ClickManager.inst.ClickCard!=null&&ClickManager.inst.clickstate==PlayerClickState.Card)
            {
                ClickManager.inst.ClickCard.GetComponent<Card_Info>().MoveReset();
                ClickManager.inst.resetfunc();
            }
            
            
            
            int c = PlayerInfo.Inst.foodMax - PlayerInfo.Inst.food;
            for (int i = 0; i < c; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (PlayerInfo.Inst.PlayerTile[j].TryGetComponent(out TileInfo ti))
                    {
                        if (ti.tileGameob!=null)
                        {
                            GameObject ob = ti.tileGameob;
                            ti.RemoveTile();
                            //int CreatePosidx=-1;
                            GameObject tile = null;
                            for (int k = 0; k < PlayerInfo.Inst.FiledTile.Count; k++)
                            {
                                if (PlayerInfo.Inst.FiledTile[k].TryGetComponent(out TileInfo check))
                                {
                                    if (check.tileGameob == null)
                                    {
                                        tile = PlayerInfo.Inst.FiledTile[k];
                                        break;
                                    }
                                }
                            }

                            ClickManager.inst.CharacterTileMoveFunc(ob, tile);
                            PlayerInfo.Inst.food++;
                            ob.GetComponent<Card_Info>().FiledIn();
                            PlayerInfo.Inst.PlayerCard_NoFiled.Remove(ob);
                            break;
                        }
                    }
                }

            }
        }

        public void BattleReady()
        {
            pv.RPC(nameof(RPC_BattleReady),RpcTarget.All);

        }

        [PunRPC]
        public void RPC_BattleReady()
        {
            
            
            
            UIManager.inst.BattleStartUi();
            PlayerInfo.Inst.IsBattle = true;
            
            
            
            
            UIManager.inst.DmgUINoShow();
            PlayerInfo.Inst.deadCnt = PlayerInfo.Inst.PlayerCard_Filed.Count;
            if (PlayerInfo.Inst.IsCopy)
            {
                PlayerInfo.Inst.copydeadCnt = PlayerInfo.Inst.deadCnt;
            }
            for (int i = 0; i < PlayerInfo.Inst.PlayerCard_Filed.Count; i++)
            {
                if (PlayerInfo.Inst.PlayerCard_Filed[i].TryGetComponent(out Card_Info info))
                {
                    
                    info.stat.DmgIdx = i;
                    UIManager.inst.DmgUIShow(info,i);
                }
                
            }
            

            for (int i = 0; i < PlayerInfo.Inst.PlayerCard_Filed.Count; i++)
            {
                if (PlayerInfo.Inst.PlayerCard_Filed[i].TryGetComponent(out Card_Info info))
                {
                    info.BattleReady();
                }
            }
            for (int i = 0; i < PVEManager.inst.Enemis.Count; i++)
            {
                if (PVEManager.inst.Enemis[i].TryGetComponent(out Card_Info info))
                {
                    info.BattleReady();
                }
            }
            for (int i = 0; i < PVPManager.inst.copyob.Count; i++)
            {
                if (PVPManager.inst.copyob[i].TryGetComponent(out Card_Info info))
                {
                    info.BattleReady();
                }
            }
                UIManager.inst.TimeFunc(2);
            UIManager.inst.PlayerInfoBattleStart();
            
        }
        public void BattleStart()
        {
            pv.RPC(nameof(RPC_BattleStart),RpcTarget.All);
            
        }

        [PunRPC]
        void RPC_BattleStart()
        {
            
            for (int i = 0; i < PlayerInfo.Inst.PlayerCard_Filed.Count; i++)
            {
                if (PlayerInfo.Inst.PlayerCard_Filed[i].TryGetComponent(out Card_Info info))
                {
                    //info.BattleReady();
                    info.BattleStart();
                    
                }
            }

            
                for (int i = 0; i < PVEManager.inst.Enemis.Count; i++)
                {
                    if (PVEManager.inst.Enemis[i].TryGetComponent(out Card_Info info))
                    {
                        info.BattleStart();
                    }
                }
                for (int i = 0; i < PVPManager.inst.copyob.Count; i++)
                {
                    if (PVPManager.inst.copyob[i].TryGetComponent(out Card_Info info))
                    {
                        info.BattleStart();
                    }
                }


            
            UIManager.inst.TimeFunc(60);

            if (PlayerInfo.Inst.deadCnt==0)
            {
                PlayerInfo.Inst.deadCnt = 1;
                PlayerInfo.Inst.DeadCheck();
                if (PlayerInfo.Inst.IsCopy)
                {
                    PlayerInfo.Inst.DeadCheck(true);
                }
            }
            
            
            
            
            // 특성 계열 추가 트리거

            if (PlayerInfo.Inst.TraitandJobCnt[3]>=3)
            {
                if (PlayerInfo.Inst.TraitandJobCnt[3]>=6)
                {
                    for (int i = 0; i < PlayerInfo.Inst.PlayerCard_Filed.Count; i++)
                    {
                        var cainfo = PlayerInfo.Inst.PlayerCard_Filed[i].GetComponent<Card_Info>();

                        cainfo.stat.Job3Func();
                    }

                    if (PlayerInfo.Inst.IsCopy)
                    {
                        for (int i = 0; i < PVPManager.inst.copyob.Count; i++)
                        {
                            var cainfo = PVPManager.inst.copyob[i].GetComponent<Card_Info>();

                            cainfo.stat.Job3Func();
                        }
                    }
                    
                }
                else
                {
                    int ran = Random.Range(0, PlayerInfo.Inst.PlayerCard_Filed.Count);
                    var cainfo = PlayerInfo.Inst.PlayerCard_Filed[ran].GetComponent<Card_Info>();
                    cainfo.stat.Job3Func();
  
                        if (PlayerInfo.Inst.IsCopy)
                        {
                            ran = Random.Range(0, PVPManager.inst.copyob.Count);
                            cainfo = PVPManager.inst.copyob[ran].GetComponent<Card_Info>();
                            cainfo.stat.Job3Func();
                        }
                }
                
                
                
                
                
                
            }

            
            if (PlayerInfo.Inst.TraitandJobCnt[4]>=2)
            {
                if (PlayerInfo.Inst.TraitandJobCnt[4]>=4)
                {
                    dummyInfo.Clear();
                    for (int i = 0; i < PlayerInfo.Inst.PlayerCard_Filed.Count; i++)
                    {
                        var check = PlayerInfo.Inst.PlayerCard_Filed[i].GetComponent<Card_Info>();
                        if (check.IsHaveJob(4))
                        {
                            dummyInfo.Add(check);
                        }
                    }
                    for (int i = 0; i < dummyInfo.Count; i++)
                    {
                        var cainfo = dummyInfo[i].GetComponent<Card_Info>();

                        cainfo.stat.Job4Func();
                    }

                    if (PlayerInfo.Inst.IsCopy)
                    {
                        dummyInfo.Clear();
                        for (int i = 0; i < PVPManager.inst.copyob.Count; i++)
                        {
                            var check = PVPManager.inst.copyob[i].GetComponent<Card_Info>();
                            if (check.IsHaveJob(4))
                            {
                                dummyInfo.Add(check);
                            }
                        }
                        for (int i = 0; i < dummyInfo.Count; i++)
                        {
                            var cainfo = dummyInfo[i].GetComponent<Card_Info>();

                            cainfo.stat.Job4Func();
                        }
                    }
                    
                }
                else
                {
                    dummyInfo.Clear();
                    for (int i = 0; i < PlayerInfo.Inst.PlayerCard_Filed.Count; i++)
                    {
                        var check = PlayerInfo.Inst.PlayerCard_Filed[i].GetComponent<Card_Info>();
                        if (check.IsHaveJob(4))
                        {
                            dummyInfo.Add(check);
                        }
                    }
                    int ran = Random.Range(0, dummyInfo.Count);
                    var cainfo = dummyInfo[ran].GetComponent<Card_Info>();
                    cainfo.stat.Job4Func();
  
                    if (PlayerInfo.Inst.IsCopy)
                    {
                        dummyInfo.Clear();
                        for (int i = 0; i < PVPManager.inst.copyob.Count; i++)
                        {
                            var check = PVPManager.inst.copyob[i].GetComponent<Card_Info>();
                            if (check.IsHaveJob(4))
                            {
                                dummyInfo.Add(check);
                            }
                        }
                        ran = Random.Range(0, dummyInfo.Count);
                        cainfo = dummyInfo[ran].GetComponent<Card_Info>();
                        cainfo.stat.Job4Func();
                    }
                }
                
                
                
                
                
                
            }
            
            if (PlayerInfo.Inst.TraitandJobCnt[29]>=2)
            {
                if (PlayerInfo.Inst.TraitandJobCnt[29]>=4)
                {
                    dummyInfo.Clear();
                    for (int i = 0; i < PlayerInfo.Inst.PlayerCard_Filed.Count; i++)
                    {
                        var check = PlayerInfo.Inst.PlayerCard_Filed[i].GetComponent<Card_Info>();
                        if (check.IsHaveJob(29))
                        {
                            dummyInfo.Add(check);
                        }
                    }
                    for (int i = 0; i < dummyInfo.Count; i++)
                    {
                        var cainfo = dummyInfo[i].GetComponent<Card_Info>();

                        cainfo.stat.Job29Func();
                    }

                    if (PlayerInfo.Inst.IsCopy)
                    {
                        dummyInfo.Clear();
                        for (int i = 0; i < PVPManager.inst.copyob.Count; i++)
                        {
                            var check = PVPManager.inst.copyob[i].GetComponent<Card_Info>();
                            if (check.IsHaveJob(29))
                            {
                                dummyInfo.Add(check);
                            }
                        }
                        for (int i = 0; i < dummyInfo.Count; i++)
                        {
                            var cainfo = dummyInfo[i].GetComponent<Card_Info>();

                            cainfo.stat.Job29Func();
                        }
                    }
                    
                }
                else
                {
                    dummyInfo.Clear();
                    for (int i = 0; i < PlayerInfo.Inst.PlayerCard_Filed.Count; i++)
                    {
                        var check = PlayerInfo.Inst.PlayerCard_Filed[i].GetComponent<Card_Info>();
                        if (check.IsHaveJob(29))
                        {
                            dummyInfo.Add(check);
                        }
                    }
                    int ran = Random.Range(0, dummyInfo.Count);
                    var cainfo = dummyInfo[ran].GetComponent<Card_Info>();
                    cainfo.stat.Job29Func();
  
                    if (PlayerInfo.Inst.IsCopy)
                    {
                        dummyInfo.Clear();
                        for (int i = 0; i < PVPManager.inst.copyob.Count; i++)
                        {
                            var check = PVPManager.inst.copyob[i].GetComponent<Card_Info>();
                            if (check.IsHaveJob(29))
                            {
                                dummyInfo.Add(check);
                            }
                        }
                        ran = Random.Range(0, dummyInfo.Count);
                        cainfo = dummyInfo[ran].GetComponent<Card_Info>();
                        cainfo.stat.Job29Func();
                    }
                }
                
                
                
                
                
                
            }
            if (PlayerInfo.Inst.TraitandJobCnt[30]>=3)
            {
                dummyInfo.Clear();
                for (int i = 0; i < PlayerInfo.Inst.PlayerCard_Filed.Count; i++)
                {
                    var check = PlayerInfo.Inst.PlayerCard_Filed[i].GetComponent<Card_Info>();
                    if (check.IsHaveJob(30))
                    {
                        dummyInfo.Add(check);
                    }
                }
                
                int ran = Random.Range(0, dummyInfo.Count);
                    var cainfo = dummyInfo[ran].GetComponent<Card_Info>();
                    //소환
                    UnitCreate(false,"Unit_Job30",cainfo.transform.position,Quaternion.identity,3000,100,1,2.6f,350,cainfo.TeamIdx,cainfo.EnemyTeamIdx,cainfo.stat.DmgIdx);

                    if (PlayerInfo.Inst.IsCopy)
                    {
                        dummyInfo.Clear();
                        for (int i = 0; i < PVPManager.inst.copyob.Count; i++)
                        {
                            var check = PVPManager.inst.copyob[i].GetComponent<Card_Info>();
                            if (check.IsHaveJob(30))
                            {
                                dummyInfo.Add(check);
                            }
                        }
                
                         ran = Random.Range(0, dummyInfo.Count);
                         cainfo = dummyInfo[ran].GetComponent<Card_Info>();
                        //소환
                        UnitCreate(true,"Unit_Job30",cainfo.transform.position,Quaternion.identity,3000,100,1,2.6f,350,cainfo.TeamIdx,cainfo.EnemyTeamIdx,cainfo.stat.DmgIdx);

                    }
                    
                    
                    
            }
            //

            coTime = StartCoroutine(BattleTimeFunc());




        }

        IEnumerator BattleTimeFunc()
        {
            int second = 0;
            while (true)
            {
                if (!PlayerInfo.Inst.IsBattle||PlayerInfo.Inst.BattleEnd)
                {
                    coTime = null;
                    break;
                }



                if (second % 4 == 0)
                {

                    if (PlayerInfo.Inst.TraitandJobCnt[2]>=2)
                    {
                        float v = 7;
                        if (PlayerInfo.Inst.TraitandJobCnt[2]>=4)
                        {
                            v = 10;
                        }
                        
                        for (int i = 0; i < PlayerInfo.Inst.PlayerCard_Filed.Count; i++)
                        {
                            var cainfo = PlayerInfo.Inst.PlayerCard_Filed[i].GetComponent<Card_Info>();
                            if (!cainfo.stat.IsDead&&cainfo.IsHaveJob(2))
                            {
                                cainfo.stat.AtkPlus(0,0,v,true);
                                EffectManager.inst.EffectCreate("Job2_Effect",PlayerInfo.Inst.PlayerCard_Filed[i].transform.position,Quaternion.Euler(-90,0,0),1.5f);

                            }
                        }
                    }
                }


                if (second%5==0)
                {
                    EventManager.inst.Sub_Item40Func.OnNext(20);
                }
                
                
                
                
                
                
                
                
                
                
                yield return YieldInstructionCache.WaitForSeconds(1);
                second++;
            }
        }

        public void BattleEnd()
        {
            pv.RPC(nameof(RPC_BattleEnd),RpcTarget.All);
        }

        [PunRPC]
        void RPC_BattleEnd()
        {
           
            
            
            PlayerInfo.Inst.roundDamgeMax = 0;
            //죽은애 다시 살리기
            if (PhotonNetwork.IsMasterClient)
            {
                MasterInfoOrder();
            }
            
            // 에너미 완전삭제
            for (int i = 0; i < PVEManager.inst.Enemis.Count; i++)
            {
                PhotonNetwork.Destroy(PVEManager.inst.Enemis[i]);
            }
            for (int i = 0; i < PVPManager.inst.copyob.Count; i++)
            {
                PhotonNetwork.Destroy(PVPManager.inst.copyob[i]);
            }
            PVEManager.inst.Enemis.Clear();
            PVPManager.inst.copyob.Clear();
            if (PlayerInfo.Inst.Dead == false)
            {
                
            
            
                for (int i = 0; i < PlayerInfo.Inst.PlayerCard_Filed.Count; i++)
                {
                    if (PlayerInfo.Inst.PlayerCard_Filed[i].TryGetComponent(out Card_Info info))
                    {
                        
                        info.BattleEnd();
                        info.stat.DmgIdx = -1;
                    }
                }
                RoundManager.inst.BattleMoveFunc2();
            }
            if(coTime!=null) StopCoroutine(coTime);
            coTime = null;

        }

        public void UnitCreate(bool copy,string name,Vector3 pos,Quaternion qu,float hp, float damage, float Cool, float range, float speed, int teami, int enemyi,int uiidx)
        {
            if (!PlayerInfo.Inst.IsBattle) return;

            GameObject ob= PhotonNetwork.Instantiate(name, pos, qu);
            if (copy)
            {
                PlayerInfo.Inst.copydeadCnt++;
            }
            else
            {
                
            PlayerInfo.Inst.deadCnt++;
            }
            if (ob.TryGetComponent(out Card_Info cardinfo))
            {
                cardinfo.UnitStart(hp,damage,Cool,range,speed,teami,enemyi,uiidx);
            }
            PVEManager.inst.Enemis.Add(ob);

            if (PlayerInfo.Inst.TraitandJobCnt[31]>=4)
            {
                ob.GetComponent<CardState>().Job31 = true;
            }

        }
        
        public void GoldSet(int eidx,int gold)
        {

            pv.RPC(nameof(RPC_GoldSet),RpcTarget.All,eidx,gold);
        }

        [PunRPC]
        void RPC_GoldSet(int eidx,int gold)
        {
            int Enemyidx = eidx;
            for (int i = 0; i <5; i++)
            {
                if (gold <10*(i+1))
                {
                    GameObject ob = PositionManager.inst.playerPositioninfo[Enemyidx].EnemyGoldOb[i];
                    if (ob.TryGetComponent(out MeshRenderer mesh))
                    {
                        mesh.material.color = PlayerInfo.Inst.colors[0];
                    }
                
                }
                else
                {
                    GameObject ob = PositionManager.inst.playerPositioninfo[Enemyidx].EnemyGoldOb[i];
                    if (ob.TryGetComponent(out MeshRenderer mesh))
                    {
                        mesh.material.color = PlayerInfo.Inst.colors[1];
                    }
                }
            
            
            }
        }
        
        
        public void BattleInfoReset()
        {
            pv.RPC(nameof(RPC_BattleInfoReset),RpcTarget.All);

        }

        [PunRPC]
        void RPC_BattleInfoReset()
        {
            for (int i = 0; i < GameSystem_AllInfo.inst.battleinfos.Count; i++)
            {
                
                GameSystem_AllInfo.inst.battleinfos[i].playeridx = i;
                GameSystem_AllInfo.inst.battleinfos[i].enemyidx = -1;
                GameSystem_AllInfo.inst.battleinfos[i].IsBattleMove = false;
                GameSystem_AllInfo.inst.battleinfos[i].IsCopy = false;
                GameSystem_AllInfo.inst.battleinfos[i].copyidx = -1;
            }
        }

        public void FireActive(int idx, bool b)
        {
            pv.RPC(nameof(RPC_FireActive),RpcTarget.All,idx,b);
        }

        [PunRPC]
        void RPC_FireActive(int idx, bool b)
        {
            Fireob[idx].SetActive(b);
        }

    }

