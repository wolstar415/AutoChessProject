using System;
using System.Collections;
using System.Collections.Generic;
using GameS;
using JetBrains.Annotations;
using Photon.Pun;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class PlayerCardCnt
{
    public List<GameObject> Lv1;
    public List<GameObject> Lv2;
    public List<GameObject> Lv3;

    public List<GameObject> Lv(int lv)
    {
        if (lv==2)
        {
            return Lv2;
        }
        else if (lv==3)
        {
            return Lv3;
        }

        return Lv1;
    }
}
public class PlayerInfo : MonoBehaviourPunCallbacks
{
    public PhotonView pv;
    public GameObject PlayerOb;
    public static PlayerInfo Inst;
    public Camera camer;
    [SerializeField]private int gold;
    public int Level;
    [SerializeField] private int life;
    public int victoryCnt;
    public int defeatCnt;
    public bool IsVictory;

    public bool BattleEnd;

    public int Life
    {
        get { return life;}
        set
        {
            life = value;
            if (life <= 0)
            {
                life = 0;
                
                DeadFunc();
            }
            pv.RPC(nameof(MasterGoLife),RpcTarget.MasterClient,PlayerIdx,life);
            
            

        }
    }
    public int PlayerIdx;
    public GameObject PlayerCharacter;
    public bool Dead = false;
    public bool IsLock=false;
    [SerializeField]private int _food=0;
    [SerializeField]private int _foodMax=0;
    public int Xp;
    public int XpMax;
    
    public GameObject FiledTileOb;
    public GameObject PlayerTileOb;
    public GameObject EnemyPlayerTileob;
    [Header("My")]
    public List<GameObject> PlayerTile;
    public List<GameObject> FiledTile;
    public List<GameObject> GoldOb;
    public Transform PlayerMovePos;
    [Header("Enemy")]
    public List<GameObject> EnemyTile;
    public List<GameObject> EnemyFiledTile;
    public List<GameObject> EnemyGoldOb;
    public Transform EnemyPlayerMovePos;
    [Space]
    public int[] PlayerTilestate;
    public int[] FiledTilestate;
    public int[] TraitandJobCnt;
    public Color[] colors;
    
    [SerializeField]private int[] PlayerCardCnt;
    [SerializeField] private int[] PlayerFiledCardCnt;

    public List<PlayerCardCnt> PlayerCardCntLv;
    [Header("라운드관련")] 
    public int Round = 0;
    public int RoundIdx = 0;
    public bool PVP = false;

    public bool PickRound = false;
    [Header("전투관련")] 
    public int EnemyIdx=-1;
    public bool IsBattle=false;
    public int deadCnt;
    public int pVEdeadCnt;
    
    public bool BattleMove = false;
    [Header("픽관련")] 
    public bool IsPick = false;

    [Header("현황")] 
    public List<GameObject> PlayerCard;
    public List<GameObject> PlayerCard_Filed;
    public List<GameObject> PlayerCard_NoFiled;


    [PunRPC]
    void MasterGoLife(int i,int life)
    {
        NetworkManager.inst.players[i].Life = life;
        if (life<=0)
        {
        NetworkManager.inst.players[i].State = 2;
        NetworkManager.inst.players[i].Dead = true;
            
        }

        MasterInfo.inst.lifeCheck[i].LifeSet(life);

    }
    public void LifeDamage(int i)
    {
        Life -= i;
    }
    public void PlayerCardCntAdd(int idx)
    {
        PlayerCardCnt[idx]++;
    }
    public void PlayerCardCntRemove(int idx)
    {
        PlayerCardCnt[idx]--;
    }
    public void PlayerFiledCardCntAdd(int trait1,int trait2,int job1,int job2,int idx)
    {
        if (PlayerFiledCardCnt[idx]==0)
        {
            TraitJobManager.inst.TraitJobAdd(trait1);
            TraitJobManager.inst.TraitJobAdd(trait2);
            TraitJobManager.inst.TraitJobAdd(job1);
            TraitJobManager.inst.TraitJobAdd(job2);
            TraitJobManager.inst.OrderList();

        }

        PlayerFiledCardCnt[idx]++;
    }
    public void PlayerFiledCardCntRemove(int trait1,int trait2,int job1,int job2,int idx)
    {
        if (PlayerFiledCardCnt[idx]==1)
        {
            TraitJobManager.inst.TraitJobRemove(trait1);
            TraitJobManager.inst.TraitJobRemove(trait2);
            TraitJobManager.inst.TraitJobRemove(job1);
            TraitJobManager.inst.TraitJobRemove(job2);
            TraitJobManager.inst.OrderList();
        }
        PlayerFiledCardCnt[idx]--;
    }
    private void Awake()
    {
        
        Inst = this;
        //테스트
        
    }

    private void Start()
    {
        XpMax = 2;
        Level = 1;
        foodMax = Level;
    }


    public int Gold
    {
        get { return gold;}
        set
        {
            if (value<0)
            {
                value = 0;
            }

            gold = value;
            UIManager.inst.GoldSet();
            interSet();
        }
    }

    void interSet()
    {
        for (int i = 0; i <5; i++)
        {
            if (Gold <10*(i+1))
            {
                pv.RPC(nameof(GoldColorSet),RpcTarget.All,PlayerIdx,i,false,EnemyIdx);
                
            }
            else
            {
                pv.RPC(nameof(GoldColorSet),RpcTarget.All,PlayerIdx,i,true,EnemyIdx);
            }
            
            
        }
    }

    [PunRPC]
    void GoldColorSet(int Pidx, int idx, bool b,int EnemyInt)
    {

        
        GameObject ob=PositionManager.inst.playerPositioninfo[Pidx].GoldOb[idx];
        
        if (ob.TryGetComponent(out MeshRenderer mesh))
        {
            if (b)
            {
            mesh.material.color = colors[1];
                
            }
            else
            {
            mesh.material.color = colors[0];
                
            }
        }
        
        
        if (EnemyInt!=-1)
        {
            ob=PositionManager.inst.playerPositioninfo[EnemyInt].EnemyGoldOb[idx];

            if (ob.TryGetComponent(out MeshRenderer mesh2))
            {
                if (b)
                {
                    mesh2.material.color = colors[1];
                
                }
                else
                {
                    mesh2.material.color = colors[0];
                
                }
            }
        }
    }
    

    public int food
    {
        get { return _food;}
        set
        {
            _food = value;
            UIManager.inst.FoodSet();
        }
    }

    public int foodMax
    {
        get { return _foodMax;}
        set 
        {
            _foodMax = value;
            UIManager.inst.FoodSet();
        }
    }

    public void XpPlus(int xp)
    {
        Xp += xp;
        
        if (Xp >= XpMax)
        {
            LevelUp();
        }
        else
        {
            UIManager.inst.XpSliderSet();
        }
    }
    public void LevelUp()
    {
        if (Level<=8)
        {
            
        Level++;
        UIManager.inst.ReRollSet(Level);
        Xp = Xp - XpMax;
        XpMax = CsvManager.inst.Player_Xp[Level];
        UIManager.inst.XpSliderSet();
        foodMax = Level;
        if (PlayerOb.TryGetComponent(out PlayerMoving p))
        {
            p.LevelSet(Level);
        }
        }
    }

    public void PveDeadCheck()
    {
        pVEdeadCnt--;
        if (pVEdeadCnt<=0)
        {
            //몹 다 잡음 게임끝났다!.
            PlayerInfo.Inst.BattleEnd = true;
            MasterInfo.inst.MaserGoPveEnd();
        }
    }

    public void DeadCheck()
    {
        deadCnt--;
        if (deadCnt<=0)
        {
            if (!PlayerInfo.Inst.PVP)
            {
                //몹들한테 데미지 받기.
            }
            else
            {
                //현재 복사랑싸우고있는지 체크하기
            }
        }
    }

    public void Victory()
    {
        
    }

    public void SelfPveDamage()
    {
        
    }
    public void SelfPvPDamage()
    {
        
    }

    public void GoDamage()
    {
        
    }

    void DeadFunc()
    {
        Dead = true;
    }


}
