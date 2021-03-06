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
        if (lv == 2)
        {
            return Lv2;
        }
        else if (lv == 3)
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
    [SerializeField] private int gold;
    public int Level;
    [SerializeField] private int life = 100;
    public int victoryCnt;
    public int defeatCnt;
    public bool IsVictory;

    public float roundDamgeMax = 0;
    public bool BattleEnd;

    public int Life
    {
        get { return life; }
        set
        {
            life = value;
            if (life <= 0)
            {
                life = 0;

                pv.RPC(nameof(RPC_DeadFunc), RpcTarget.MasterClient, PlayerIdx);
                pv.RPC(nameof(RPC_DeadCheck), RpcTarget.All, PlayerIdx);
                PlayerDead();
                return;
            }

            pv.RPC(nameof(MasterGoLife), RpcTarget.MasterClient, PlayerIdx, life);
            if (PlayerOb.TryGetComponent(out PlayerMoving pl))
            {
                pl.HpSetting(life);
            }
        }
    }

    public int PlayerIdx;
    public GameObject PlayerCharacter;
    public bool Dead = false;
    public bool IsLock = false;
    [SerializeField] private int _food = 0;
    [SerializeField] private int _foodMax = 0;
    public int Xp;
    public int XpMax;

    public GameObject FiledTileOb;
    public GameObject PlayerTileOb;
    public GameObject EnemyPlayerTileob;
    [Header("My")] public List<GameObject> PlayerTile;
    public List<GameObject> FiledTile;
    public List<GameObject> GoldOb;
    public Transform PlayerMovePos;
    [Header("Enemy")] public List<GameObject> EnemyTile;
    public List<GameObject> EnemyFiledTile;
    public List<GameObject> EnemyGoldOb;
    public Transform EnemyPlayerMovePos;
    [Space] public int[] PlayerTilestate;
    public int[] FiledTilestate;
    public int[] TraitandJobCnt;
    public Color[] colors;

    [SerializeField] private int[] PlayerCardCnt;
    [SerializeField] private int[] PlayerFiledCardCnt;

    public List<PlayerCardCnt> PlayerCardCntLv;
    [Header("???????????????")] public int Round = 0;
    public int RoundIdx = 0;
    public bool PVP = false;

    public bool PickRound = false;
    [Header("????????????")] public int EnemyIdx = -1;
    public int copyEnemyIdx = -1;
    public bool IsBattle = false;
    public bool IsCopy = false;
    public int deadCnt;
    public int copydeadCnt;
    public int pVEdeadCnt;

    public bool BattleMove = false;
    [Header("?????????")] public bool IsPick = false;

    [Header("??????")] public List<GameObject> PlayerCard;
    public List<GameObject> PlayerCard_Filed;
    public List<GameObject> PlayerCard_NoFiled;


    public void LifeDamage(int i)
    {
        Life -= i;
        NetworkManager.inst.TextUi(i.ToString(), PlayerInfo.Inst.PlayerOb.transform.position, 5);
    }

    public void PlayerCardCntAdd(int idx)
    {
        PlayerCardCnt[idx]++;
    }

    public void PlayerCardCntRemove(int idx)
    {
        PlayerCardCnt[idx]--;
    }

    public void PlayerFiledCardCntAdd(Card_Info info)
    {
        bool b = false;

        if (PlayerFiledCardCnt[info.Idx] == 0)
        {
            TraitJobManager.inst.TraitJobAdd(info.Character_trait1);
            TraitJobManager.inst.TraitJobAdd(info.Character_trait2);
            TraitJobManager.inst.TraitJobAdd(info.Character_Job1);
            TraitJobManager.inst.TraitJobAdd(info.Character_Job2);
            b = true;
        }


        if (info.IsHaveJob(3, false) == false && info.IsItemHave(17) > 0)
        {
            TraitJobManager.inst.TraitJobAdd(3);
            b = true;
        }

        if (info.IsHaveJob(26, false) == false && info.IsItemHave(25) > 0)
        {
            TraitJobManager.inst.TraitJobAdd(26);
            b = true;
        }

        if (info.IsHaveJob(27, false) == false && info.IsItemHave(32) > 0)
        {
            TraitJobManager.inst.TraitJobAdd(27);
            b = true;
        }

        if (info.IsHaveJob(8, false) == false && info.IsItemHave(38) > 0)
        {
            TraitJobManager.inst.TraitJobAdd(8);
            b = true;
        }

        if (info.IsHaveJob(28, false) == false && info.IsItemHave(43) > 0)
        {
            TraitJobManager.inst.TraitJobAdd(28);
            b = true;
        }

        if (info.IsHaveJob(24, false) == false && info.IsItemHave(47) > 0)
        {
            TraitJobManager.inst.TraitJobAdd(24);
            b = true;
        }

        if (info.IsHaveJob(23, false) == false && info.IsItemHave(50) > 0)
        {
            TraitJobManager.inst.TraitJobAdd(23);
            b = true;
        }

        if (info.IsHaveJob(25, false) == false && info.IsItemHave(52) > 0)
        {
            TraitJobManager.inst.TraitJobAdd(25);
            b = true;
        }

        if (b) TraitJobManager.inst.OrderList();
        PlayerFiledCardCnt[info.Idx]++;
    }

    public void PlayerFiledCardCntRemove(Card_Info info)
    {
        bool b = false;
        if (PlayerFiledCardCnt[info.Idx] == 1)
        {
            TraitJobManager.inst.TraitJobRemove(info.Character_trait1);
            TraitJobManager.inst.TraitJobRemove(info.Character_trait2);
            TraitJobManager.inst.TraitJobRemove(info.Character_Job1);
            TraitJobManager.inst.TraitJobRemove(info.Character_Job2);
            b = true;
        }


        if (info.IsHaveJob(3, false) == false && info.IsItemHave(17) > 0)
        {
            TraitJobManager.inst.TraitJobRemove(3);
            b = true;
        }

        if (info.IsHaveJob(26, false) == false && info.IsItemHave(25) > 0)
        {
            TraitJobManager.inst.TraitJobRemove(26);
            b = true;
        }

        if (info.IsHaveJob(27, false) == false && info.IsItemHave(32) > 0)
        {
            TraitJobManager.inst.TraitJobRemove(27);
            b = true;
        }

        if (info.IsHaveJob(8, false) == false && info.IsItemHave(38) > 0)
        {
            TraitJobManager.inst.TraitJobRemove(8);
            b = true;
        }

        if (info.IsHaveJob(28, false) == false && info.IsItemHave(43) > 0)
        {
            TraitJobManager.inst.TraitJobRemove(28);
            b = true;
        }

        if (info.IsHaveJob(24, false) == false && info.IsItemHave(47) > 0)
        {
            TraitJobManager.inst.TraitJobRemove(24);
            b = true;
        }

        if (info.IsHaveJob(23, false) == false && info.IsItemHave(50) > 0)
        {
            TraitJobManager.inst.TraitJobRemove(23);
            b = true;
        }

        if (info.IsHaveJob(25, false) == false && info.IsItemHave(52) > 0)
        {
            TraitJobManager.inst.TraitJobRemove(25);
            b = true;
        }

        if (b) TraitJobManager.inst.OrderList();
        PlayerFiledCardCnt[info.Idx]--;
    }

    private void Awake()
    {
        Inst = this;
        //?????????
    }

    private void Start()
    {
        XpMax = 2;
        Level = 1;
        foodMax = Level;
    }


    public int Gold
    {
        get { return gold; }
        set
        {
            if (value < 0)
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
        for (int i = 0; i < 5; i++)
        {
            if (Gold < 10 * (i + 1))
            {
                pv.RPC(nameof(GoldColorSet), RpcTarget.All, PlayerIdx, i, false, EnemyIdx);
            }
            else
            {
                pv.RPC(nameof(GoldColorSet), RpcTarget.All, PlayerIdx, i, true, EnemyIdx);
            }
        }
    }

    [PunRPC]
    void GoldColorSet(int Pidx, int idx, bool b, int EnemyInt)
    {
        GameObject ob = PositionManager.inst.playerPositioninfo[Pidx].GoldOb[idx];

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


        if (EnemyInt != -1 && EnemyInt != 10)
        {
            ob = PositionManager.inst.playerPositioninfo[EnemyInt].EnemyGoldOb[idx];

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
        get { return _food; }
        set
        {
            _food = value;
            UIManager.inst.FoodSet();
        }
    }


    //public ReactiveProperty<int> foodMax = new ReactiveProperty<int>();

    public int foodMax
    {
        get { return _foodMax; }
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
        if (Level <= 8)
        {
            Level++;
            UIManager.inst.ReRollSet(Level);
            Xp = Xp - XpMax;
            XpMax = CsvManager.inst.Player_Xp[Level - 1];
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
        if (pVEdeadCnt <= 0)
        {
            //??? ??? ?????? ???????????????!.
            PlayerInfo.Inst.BattleEnd = true;
            MasterInfo.inst.BattelEndFunc1();
        }
    }

    public void DeadCheck(bool isCopy = false)
    {
        if (isCopy)
        {
            CopyDead();
            return;
        }

        deadCnt--;
        if (deadCnt <= 0)
        {
            if (!PlayerInfo.Inst.PVP)
            {
                //???????????? ????????? ??????.
                int dmg = CsvManager.inst.RoundDamage[Round];

                for (int i = 0; i < PVEManager.inst.Enemis.Count; i++)
                {
                    if (PVEManager.inst.Enemis[i].TryGetComponent(out CardState stat))
                    {
                        if (!stat.IsDead)
                        {
                            dmg += 2;
                        }
                    }
                }

                PlayerInfo.Inst.BattleEnd = true;
                MasterInfo.inst.BattelEndFunc1();
                pv.RPC(nameof(RPC_RoundDamage), RpcTarget.All, dmg, PlayerIdx);
                //1?????????
                if (IsVictory)
                {
                    IsVictory = false;
                    if (victoryCnt >= 5)
                    {
                        NetworkManager.inst.FireActive(PlayerIdx, false);
                    }

                    victoryCnt = 0;
                    defeatCnt = 1;
                }
                else
                {
                    victoryCnt = 0;
                    defeatCnt++;
                }

                UIManager.inst.VictorySet();
            }
            else
            {
                //????????????????????? ????????????
                //1?????????
                pv.RPC(nameof(Victory), RpcTarget.All, EnemyIdx, PlayerIdx, false);
                PlayerInfo.Inst.BattleEnd = true;
                MasterInfo.inst.BattelEndFunc1();

                if (IsVictory)
                {
                    IsVictory = false;
                    if (victoryCnt >= 5)
                    {
                        NetworkManager.inst.FireActive(PlayerIdx, false);
                    }

                    victoryCnt = 0;
                    defeatCnt = 1;
                }
                else
                {
                    victoryCnt = 0;
                    defeatCnt++;
                }

                UIManager.inst.VictorySet();
            }
        }
    }

    public void CopyDead()
    {
        copydeadCnt--;
        if (copydeadCnt <= 0)
        {
            //???????????????
            pv.RPC(nameof(Victory), RpcTarget.All, copyEnemyIdx, PlayerIdx, true);
        }
    }

    [PunRPC]
    void Victory(int pidx, int pidx2, bool isCopy)
    {
        if (PlayerIdx != pidx) return;
        if (IsCopy && copyEnemyIdx == pidx2) // ????????????????????? ???????????? ??????????????????
        {
            int dmg2 = CsvManager.inst.RoundDamage[Round];

            for (int i = 0; i < PVPManager.inst.copyob.Count; i++)
            {
                if (PVPManager.inst.copyob[i].TryGetComponent(out CardState stat))
                {
                    if (!stat.IsDead)
                    {
                        dmg2 += 2;
                    }
                }
            }

            //??????????????? ????????? ?????????
            pv.RPC(nameof(RPC_RoundDamage), RpcTarget.All, dmg2, pidx2);
            return;
        }


        Gold++; //?????????

        //1?????????
        if (IsVictory)
        {
            victoryCnt++;
            if (victoryCnt == 5)
            {
                NetworkManager.inst.FireActive(PlayerIdx, true);
            }
        }
        else
        {
            IsVictory = true;
            victoryCnt = 1;
            defeatCnt = 0;
        }

        UIManager.inst.VictorySet();

        PlayerInfo.Inst.BattleEnd = true;
        MasterInfo.inst.BattelEndFunc1();

        if (isCopy) return; //?????? ????????? ???????????? ?????????

        int dmg = CsvManager.inst.RoundDamage[Round];

        for (int i = 0; i < PlayerCard_Filed.Count; i++)
        {
            if (PlayerCard_Filed[i].TryGetComponent(out CardState stat))
            {
                if (!stat.IsDead)
                {
                    dmg += 2;
                }
            }
        }

        //??????????????? ????????? ?????????
        pv.RPC(nameof(RPC_RoundDamage), RpcTarget.All, dmg, pidx2);
    }

    public void RoundDamage(int dmg, int pidx)
    {
        pv.RPC(nameof(RPC_RoundDamage), RpcTarget.All, dmg, pidx);
    }

    [PunRPC]
    void RPC_RoundDamage(int dmg, int pidx)
    {
        if (PlayerInfo.Inst.Dead) return;
        if (PlayerInfo.Inst.PlayerIdx == pidx)
        {
            //?????????

            EffectManager.inst.EffectCreate("DeadEffect", PlayerOb.transform.position, Quaternion.identity, 1);
            LifeDamage(dmg);
        }
    }


    [PunRPC]
    void RPC_DeadCheck(int pidx)
    {
        GameSystem_AllInfo.inst.playerdead[pidx] = true;
    }

    [PunRPC]
    void RPC_DeadFunc(int pidx)
    {
        NetworkManager.inst.players[pidx].Dead = true;
        NetworkManager.inst.players[pidx].State = 2;
        NetworkManager.inst.players[pidx].Life = 0;

        var lifeRank = MasterInfo.inst.lifeCheck[pidx];
        lifeRank.Life = 0;
    }

    void PlayerDead()
    {
        Dead = true;
        PhotonNetwork.Destroy(PlayerOb);

        for (int i = 0; i < PlayerCard.Count; i++)
        {
            if (PlayerCard[i].TryGetComponent(out Card_Info info))
            {
                info.remove();
            }
        }

        for (int i = 0; i < PVEManager.inst.Enemis.Count; i++)
        {
            PhotonNetwork.Destroy(PVEManager.inst.Enemis[i]);
        }

        PVEManager.inst.Enemis.Clear();


        UIManager.inst.DeadUi();
        MasterInfo.inst.VictoryCheck();
    }


    [PunRPC]
    void MasterGoLife(int i, int life)
    {
        NetworkManager.inst.players[i].Life = life;
        if (life <= 0)
        {
            NetworkManager.inst.players[i].State = 2;
            NetworkManager.inst.players[i].Dead = true;
        }


        var lifeRank = MasterInfo.inst.lifeCheck[i];
        lifeRank.Life = life;
    }

    public void TraitandJobFunc(bool Plus, int idx)
    {
        int pidx = PlayerIdx;
        if (Plus)
        {
            TraitandJobCnt[idx]++;
        }
        else
        {
            TraitandJobCnt[idx]--;
        }

        pv.RPC(nameof(RPC_TraitandJobFunc), RpcTarget.All, Plus, idx, pidx);
    }

    [PunRPC]
    public void RPC_TraitandJobFunc(bool Plus, int idx, int pidx)
    {
        if (Plus)
        {
            GameSystem_AllInfo.inst.playerJobcnt[pidx].JobAndTrait[idx]++;
        }
        else
        {
            GameSystem_AllInfo.inst.playerJobcnt[pidx].JobAndTrait[idx]--;
        }
    }
}