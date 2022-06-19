using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameS;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Random = System.Random;
using Newtonsoft.Json;


[System.Serializable]
public class LifeRank
{
    public int PlayerIdx;
    public int Life;

    public LifeRank(int _idx, int _Life)
    {
        PlayerIdx = _idx;
        Life = _Life;
    }

    // public void LifeSet(int life)
    // {
    //     Life = life;
    // }
}
public class MasterInfo : MonoBehaviourPunCallbacks
{
    public PhotonView pv;
    public static MasterInfo inst;
    public List<int> Cnt_Card1;
    public List<int> Cnt_Card2;
    public List<int> Cnt_Card3;
    public List<int> Cnt_Card4;
    public List<int> Cnt_Card5;
    
    public List<int> player_PickCheck;

    public List<GameObject> pickCards;
    public List<LifeRank> lifeCheck;
    public List<LifeRank> lifeRank;
    public List<LifeRank> lifeRank2;
    

    private void Start()
    {
        
        if (PhotonNetwork.IsMasterClient)
        {
            masterFunc();
            
        }
    }

    public string LifeOrder()
    {

        lifeRank.Clear();
        for (int i = 0; i < lifeCheck.Count; i++)
        {
            lifeRank.Add(new LifeRank(lifeCheck[i].PlayerIdx,lifeCheck[i].Life));
        }

        lifeRank = lifeRank.OrderBy(o => o.Life).ToList();
        return JsonConvert.SerializeObject(lifeRank);


    }
    public string LifeOrder2()
    {


        lifeRank2.Clear();
        for (int i = 0; i < lifeCheck.Count; i++)
        {
            lifeRank2.Add(new LifeRank(lifeCheck[i].PlayerIdx,lifeCheck[i].Life));
        }

        lifeRank2 = lifeRank2.OrderByDescending(o => o.Life).ToList();
        return JsonConvert.SerializeObject(lifeRank2);

    }

    private void Awake()
    {
        inst = this;
    }

    void masterFunc()
    {
        foreach (GameObject ob in GameSystem_AllInfo.inst.Card_1)
        {
            int max = CsvManager.inst.CardMax[1];
            int idx=0;
            if (ob.TryGetComponent(out Card_Info info))
            {
                idx = info.Idx;
            }
            for (int i = 0; i < max; i++)
            {
                Cnt_Card1.Add(idx);
            }
        }
        foreach (GameObject ob in GameSystem_AllInfo.inst.Card_2)
        {
            int max = CsvManager.inst.CardMax[2];
            int idx=0;
            if (ob.TryGetComponent(out Card_Info info))
            {
                idx = info.Idx;
            }
            for (int i = 0; i < max; i++)
            {
                Cnt_Card2.Add(idx);
            }
        }
        foreach (GameObject ob in GameSystem_AllInfo.inst.Card_3)
        {
            int max = CsvManager.inst.CardMax[3];
            int idx=0;
            if (ob.TryGetComponent(out Card_Info info))
            {
                idx = info.Idx;
            }
            for (int i = 0; i < max; i++)
            {
                Cnt_Card3.Add(idx);
            }
        }
        foreach (GameObject ob in GameSystem_AllInfo.inst.Card_4)
        {
            int max = CsvManager.inst.CardMax[4];
            int idx=0;
            if (ob.TryGetComponent(out Card_Info info))
            {
                idx = info.Idx;
            }
            for (int i = 0; i < max; i++)
            {
                Cnt_Card4.Add(idx);
            }
        }
        foreach (GameObject ob in GameSystem_AllInfo.inst.Card_5)
        {
            int max = CsvManager.inst.CardMax[5];
            int idx=0;
            if (ob.TryGetComponent(out Card_Info info))
            {
                idx = info.Idx;
            }
            for (int i = 0; i < max; i++)
            {
                Cnt_Card5.Add(idx);
            }
        }
    }
    
    public List<int> CardList(int Lv)
    {
        
        switch (Lv)
        {
            case 1:
                return Cnt_Card1;
                break;
            case 2:
                return Cnt_Card2;
                break;
            case 3:
                return Cnt_Card3;
                break;
            case 4:
                return Cnt_Card4;
                break;
            case 5:
                return Cnt_Card5;
                break;
            default:
                break;
        }

        return Cnt_Card1;
    }

    

    public void CardResetNetworkFunc1()
    {
        int lv = PlayerInfo.Inst.Level;
        pv.RPC(nameof(CardResetNetworkFunc2),RpcTarget.MasterClient,PhotonNetwork.LocalPlayer,lv);
    }

    [PunRPC]
    public void CardResetNetworkFunc2(Player p, int lv)
    {
        int[] idxs = new int[5];
        for (int i = 0; i < 5; i++)
        {
            idxs[i] = RandomCardSet(lv);
        }
        pv.RPC(nameof(CardResetNetworkFunc3),RpcTarget.All,p,idxs);
    }


    public void PickPlayerCheck()
    {
        for (int i = 0; i < NetworkManager.inst.players.Count; i++)
        {
            player_PickCheck[i] = 0;
            if (NetworkManager.inst.players[i].Dead==false&&NetworkManager.inst.players[i].State==1)
            {
                player_PickCheck[i] = 1;
                NetworkManager.inst.players[i].IsPickBool = true;
            }
        }
        
    }
    public void PickCardCreate(int idxCheck)
    {
        
        if (idxCheck==1000)
        {
            PickRoundManager.inst.FirstFunc();
        }
        else
        {
            PickRoundManager.inst.PickFunc();
        }
    }
    
    

    [PunRPC]
    public void CardResetNetworkFunc3(Player p,int[] idxs)
    {
        if (PhotonNetwork.LocalPlayer==p)
        {
            CardManager.inst.CardResetFunc(idxs);
        }
    }
    int RandomCardSet(int lv)
    {
        List<int> ReRoll = CsvManager.inst.ReRoll(lv);
        int selectlv = 0;
        int Per = 0;
        int RanPer = UnityEngine.Random.Range(0, 100);
        for (int i = 0; i < 5; i++)
        {
            Per += ReRoll[i];
            if (RanPer<=Per)
            {
                selectlv = i + 1;
                break;
            }
        }

        List<int> Cardlist = CardList(selectlv);
        int randomint=UnityEngine.Random.Range(0, Cardlist.Count);
        int result = Cardlist[randomint];
        Cardlist.RemoveAt((randomint));
        return result;
    }

    public void CardRemove(int idx)
    {
        pv.RPC(nameof(CardRemoveFunc),RpcTarget.MasterClient,idx);
    }

    public void CardAdd(int idx)
    {
        pv.RPC(nameof(CardAddFunc),RpcTarget.MasterClient,idx,1);
    }
    public void CardAdd_Lv(int idx,int lv)
    {
        int cnt = 1;
        if (lv==2)
        {
            cnt =3;
        }
        else if (lv==3)
        {
            cnt =9;
        }
        pv.RPC(nameof(CardAddFunc),RpcTarget.MasterClient,idx,cnt);
    }
    [PunRPC]
    
     void CardRemoveFunc(int idx)
    {
        int lv = CsvManager.inst.characterInfo[idx].Tier;
        List<int> ranlist = CardList(lv);
        ranlist.Remove(idx);
    }
    [PunRPC]
     void CardAddFunc(int idx,int cnt)
    {
        int lv = CsvManager.inst.characterInfo[idx].Tier;
        List<int> ranlist = CardList(lv);
        for (int i = 0; i < cnt; i++)
        {
        ranlist.Add(idx);
            
        }
    }

     public void BattelEndFunc1()
     {
         pv.RPC(nameof(RPC_BattelEndFunc1),RpcTarget.All,PlayerInfo.Inst.PlayerIdx);
     }

     [PunRPC]
     void RPC_BattelEndFunc1(int pidx)
     {
         if (PhotonNetwork.IsMasterClient)
         {
         NetworkManager.inst.players[pidx].BattleEnd = true;
             
         }
         UIManager.inst.PlayerInfoClear(pidx);
     }

     public void MasterPveStart(float t)
     {
         StartCoroutine(IPVEStart(t));
     }
     IEnumerator IPVEStart(float t)
     {
         yield return YieldInstructionCache.WaitForSeconds(t);
         //전부 실행
         NetworkManager.inst.BattleFoodCheck();
         NetworkManager.inst.BattleReady();
         yield return YieldInstructionCache.WaitForSeconds(2);
         NetworkManager.inst.BattleStart();
         //배틀시작

         while (true)
         {
             bool b = true;
             for (int i = 0; i < NetworkManager.inst.players.Count; i++)
             {
                 if (NetworkManager.inst.players[i].State==1&&NetworkManager.inst.players[i].BattleEnd==false)
                 {
                     b = false;
                     break;
                 }
             }

             if (b)
             {
                 break;
             }
             yield return YieldInstructionCache.WaitForSeconds(1);
         }
         
         yield return YieldInstructionCache.WaitForSeconds(1);
         NetworkManager.inst.BattleEnd();
         yield return YieldInstructionCache.WaitForSeconds(1);
         NetworkManager.inst.RoundFuncGo(2);
                
         


     }
     
     public void MasterBattleRoundReady()
     {
         if (!PhotonNetwork.IsMasterClient) return;

         for (int i = 0; i < NetworkManager.inst.players.Count; i++)
         {
             if (NetworkManager.inst.players[i].State==1&&NetworkManager.inst.players[i].Dead==false)
             {
                 NetworkManager.inst.players[i].BattleEnd = false;
             }
         }
     }

     public void MasterPvPStart(float t) // 마스터가 배틀정보보냄
     {
         
         StartCoroutine(IPVPStart(t));
     }

     public void BtSent()
     {
         string s= JsonConvert.SerializeObject(GameSystem_AllInfo.inst.battleinfos);
         pv.RPC(nameof(RPC_BtInfoSend),RpcTarget.All,s);
     }

     [PunRPC]
     void RPC_BtInfoSend(string s)
     {
         GameSystem_AllInfo.inst.battleinfos=JsonConvert.DeserializeObject<List<BattleInfo>>(s);

         var btinfo = GameSystem_AllInfo.inst.battleinfos[PlayerInfo.Inst.PlayerIdx];
         PlayerInfo.Inst.EnemyIdx = btinfo.enemyidx;
         PlayerInfo.Inst.copyEnemyIdx = btinfo.copyidx;
         PlayerInfo.Inst.BattleMove = btinfo.IsBattleMove;
         PlayerInfo.Inst.IsCopy = btinfo.IsCopy;
         
         RoundManager.inst.BattleMoveFunc(); //팀 모두 이동

         if (PlayerInfo.Inst.IsCopy)
         {
             //쫙생성하기
             for (int i = 0; i < PlayerInfo.Inst.PlayerCard_Filed.Count; i++)
             {
                 var oriOb = PlayerInfo.Inst.PlayerCard_Filed[i];
                 var cinfo = oriOb.GetComponent<Card_Info>();
                 string name = GameSystem_AllInfo.inst.Cards[cinfo.Idx];
                 var tile = PositionManager.inst.playerPositioninfo[PlayerInfo.Inst.copyEnemyIdx]
                     .EnemyFiledTile[cinfo.MoveIdx];
                  GameObject ob=PhotonNetwork.Instantiate(name, tile.transform.position, Quaternion.Euler(0, -180, 0));
                 ob.GetComponent<Card_Info>().CopyStart(cinfo.Level,cinfo.Item);
             }
         }
         

         


         NetworkManager.inst.RPC_BattleReady();
     }
     
     IEnumerator IPVPStart(float t)
     {
         yield return YieldInstructionCache.WaitForSeconds(t);
         //전부 실행
         NetworkManager.inst.BattleFoodCheck();
         PVPManager.inst.MasterBattleReady(); // 다 이동시킴
         yield return YieldInstructionCache.WaitForSeconds(3);
         
         
         NetworkManager.inst.BattleStart();
         //배틀시작

         while (true)
         {
             bool b = true;
             for (int i = 0; i < NetworkManager.inst.players.Count; i++)
             {
                 if (NetworkManager.inst.players[i].State==1&&NetworkManager.inst.players[i].BattleEnd==false)
                 {
                     b = false;
                     break;
                 }
             }

             if (b)
             {
                 break;
             }
             yield return YieldInstructionCache.WaitForSeconds(1);
         }
         
         yield return YieldInstructionCache.WaitForSeconds(1);
         NetworkManager.inst.BattleEnd();
         yield return YieldInstructionCache.WaitForSeconds(1);
         NetworkManager.inst.RoundFuncGo(1);


     }
     
}
