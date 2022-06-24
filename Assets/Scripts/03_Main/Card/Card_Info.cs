using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Photon.Pun;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace GameS
{
    public enum Fight_FSM
    {
        None = 0,
        Idle,
        Moving,
        Attacking
    }


    public class Card_Info : MonoBehaviourPunCallbacks
    {
        [Header("네트워크")] public PhotonView pv;
        [Header("전투상태")] public bool IsFiled;
        public Card_FSM_Fight fsm;
        public bool IsFighting = false;
        public bool IsAttacker = false;
        public int TeamIdx = 0;
        public int EnemyTeamIdx = 0;
        public int Idx;
        public int MoveIdx;
        public GameObject TileOb;
        public bool IsDead = false;
        public int GoldCost; //가격

        public bool IsAni;
        public CharacterInfo info;
        public int Name; //이름
        public int Icon; //이름
        [Header("정보")] public int Level = 0; //레벨
        public int Tier = 0; //티어
        public int Food; //인구수
        public bool IsRangeAttack; //공격타입(원거리,근거리)
        public int[] Item;

        public CardState stat;
        [Header("특성 계열")] public int Character_Job1; //계열1
        public int Character_Job2; //계열2
        public int Character_trait1; //특성1
        public int Character_trait2; //특성2

        [Header("UI")] public GameObject[] StarUI;
        public GameObject[] ItemUI;

        private IDisposable Event_MoveReset;
        private IDisposable Event_BattleMove;
        private IDisposable Event_CheckLevelUp=null;
        private IDisposable Event_TJ1=null;
        private IDisposable Event_TJ2=null;
        private IDisposable Event_TJ3=null;
        private IDisposable Event_TJ4=null;
        
        private IDisposable Event_item40=null;


        [Header("Pick")] public int pickIdx = -1;

        public Boolean IsPick;

        [Header("특성직업 표시")] 
        [SerializeField] private GameObject show_fileTileob;
        [SerializeField] private GameObject show_Tileob;

        private List<Collider> dummylist = new List<Collider>();
        
        private static readonly int Run = Animator.StringToHash("Run");
        // Start is called before the first frame update


        public void PickStart(int Idx, int ItemIdx)
        {
            pv.RPC(nameof(NetworkdPickStart), RpcTarget.All, Idx,ItemIdx);
            stat.ani.SetBool(Run,true);
        }

        public void PickSelect()
        {
            pv.RPC(nameof(NetworkdPickSelect), RpcTarget.All, PlayerInfo.Inst.PlayerIdx);
            
            MasterInfo.inst.CardRemove(Idx);
            stat.ani.SetBool(Run,false);
        }



        [PunRPC]
        void NetworkdPickStart(int Idx,int itemidx)
        {
            transform.parent = GameSystem_AllInfo.inst.PickPos;


            pickIdx = Idx;
            IsPick = true;
            GameSystem_AllInfo.inst.PickCard[Idx] = gameObject;
            
            int itemicon = CsvManager.inst.itemInfo[itemidx].Icon;
            Item[0] = itemidx;
            if (ItemUI[0].TryGetComponent(out Image image))
            {
                image.sprite = IconManager.inst.icon[itemicon];
            }
            ItemUI[0].SetActive(true);

            


        }

        [PunRPC]
        void NetworkdPickSelect(int Playeridx)
        {

            transform.parent = null;


            GameSystem_AllInfo.inst.PickCard[pickIdx] = null;
            pickIdx = -1;
            IsPick = false;
            if (PhotonNetwork.IsMasterClient)
            {
                MasterInfo.inst.pickCards.Remove(gameObject);
            }

        }

        public void startSetting()
        {
            
            PlayerInfo.Inst.PlayerCard_NoFiled.Add(gameObject);
            show_fileTileob.GetComponent<MeshRenderer>().material.color = show_fileTileob.GetComponent<TileInfo>().colors[2];
            show_Tileob.GetComponent<MeshRenderer>().material.color = show_Tileob.GetComponent<TileInfo>().colors[2];
            pv.RPC(nameof(NetworkSetting), RpcTarget.All, PlayerInfo.Inst.PlayerIdx);

            PlayerInfo.Inst.PlayerCardCntAdd(Idx);
            PlayerInfo.Inst.PlayerCardCntLv[Idx].Lv(1).Add(gameObject);

            if (pv.Owner != PhotonNetwork.LocalPlayer)
            {
                pv.TransferOwnership(PhotonNetwork.LocalPlayer);
            }


            for (int i = 0; i < CardManager.inst.CardUi.Count; i++)
            {
                if (CardManager.inst.CardUi[i].TryGetComponent(out CardUI_Info check))
                {
                    check.CheckEffect();
                }
            }



            stat.RangeSet();
            PlayerInfo.Inst.PlayerCard.Add(gameObject);
            //이벤트부분 등록

            Event_MoveReset = EventManager.inst.Sub_CardMove.Subscribe(_ => { MoveReset(); }
            );
            Event_BattleMove = EventManager.inst.Sub_CardBattleMove.Subscribe(BattleMove
            );
            if (PlayerInfo.Inst.PlayerCardCntLv[Idx].Lv1.Count>=3)
            {
                Event_CheckLevelUp = EventManager.inst.Sub_LevelUpCheck.Subscribe(_=>LevelUpCheck()); 
            }
            
            if (Character_trait1>=1) Event_TJ1=TraitJobManager.inst.Obs[Character_trait1].GetComponent<TraitJobInfo>().Sub_CardJobAndTraitShow.Subscribe(TileShow);
            if (Character_trait2>=1) Event_TJ2=TraitJobManager.inst.Obs[Character_trait2].GetComponent<TraitJobInfo>().Sub_CardJobAndTraitShow.Subscribe(TileShow);
            if (Character_Job1>=1) Event_TJ3=TraitJobManager.inst.Obs[Character_Job1].GetComponent<TraitJobInfo>().Sub_CardJobAndTraitShow.Subscribe(TileShow);
            if (Character_Job2>=1) Event_TJ4=TraitJobManager.inst.Obs[Character_Job2].GetComponent<TraitJobInfo>().Sub_CardJobAndTraitShow.Subscribe(TileShow);


            if (Item[0]>=0)
            {
                pv.RPC(nameof(itemStatadd), RpcTarget.All, 0, Item[0]);
            }

        }

        [PunRPC]
        void NetworkSetting(int PlayerIdx)
        {
            stat.HpLine.SetActive(true);
            info = CsvManager.inst.characterInfo[Idx];
            Level = 1;
            Setting(Level);
            Tier = info.Tier;
            Food = info.Food;
            IsRangeAttack = info.IsRange;
            Name = info.Name;
            Icon = info.Icon;
            Character_Job1 = info.Job1;
            Character_Job2 = info.Job2;
            Character_trait1 = info.Trait1;
            Character_trait2 = info.Trait2;
            stat.Char_Range = info.Range;
            stat.Char_Defence = info.Defense;
            stat.Char_Defence_Magic = info.Defense;
            stat.Char_Speed = info.Speed;
            stat.Char_Mana = info.Mana;
            stat.Char_ManaMax = info.Mana_Max;
            stat.Char_CriPer = info.CriPer;
            stat.Char_CriDmg = info.CriDmg;
            stat.Char_Magic_Damage = 100;
            TeamIdx = PlayerIdx;
            gameObject.layer = 6 + PlayerIdx;
            stat.nav.speed = stat.Speed() * 0.01f;
            stat.currentHp = stat.HpMax();
            stat.currentMana = stat.ManaMax();
            stat.HpAndMpSet();
            if (pv.IsMine)
            {
                stat.RangeSet();
            }
            else
            {
                stat.ColorChange();
            }
        }

        public void EnemyStart(float hp,float damage,float Cool,int Checkidx,int item=-1)
        {
            IsFiled = true;
            stat.Char_Speed = 350f;
            stat.nav.speed = 200 * 0.01f;
            stat.Char_Range = 1.8f;
            stat.RangeSet();
            stat.Checkidx = Checkidx;
            pv.RPC(nameof(RPC_EnemyStart),RpcTarget.All,hp,damage,Cool,PlayerInfo.Inst.PlayerIdx);
            if (item>=0)
            {
                pv.RPC(nameof(itemStatadd), RpcTarget.All, 0, item);
            }
        }

        

        [PunRPC]
        void RPC_EnemyStart(float hp,float damage,float Cool,int playeridx)
        {



            stat.monster = true;
            stat.Char_Hp = hp;
            stat.Char_Atk_Cool = Cool;
            stat.Char_Atk_Damage = damage;
            
            stat.ColorChange();
            TeamIdx = 10;
            EnemyTeamIdx = playeridx;
            gameObject.layer = 16;
            stat.currentHp = stat.HpMax();
            stat.currentMana = stat.ManaMax();
            stat.HpAndMpSet();
        }

        public void UnitStart(float hp, float damage, float Cool, float range, float speed, int teami, int enemyi,int uiidx)
        {
            IsFiled = true;
            stat.Char_Speed = speed;
            stat.nav.speed = speed * 0.01f;
            stat.Char_Range = range;
            stat.RangeSet();
            stat.DmgIdx = uiidx;

            pv.RPC(nameof(RPC_UnitStart), RpcTarget.All, hp, damage, Cool, teami, enemyi);


            stat.collider.enabled = true;
            stat.nav.enabled = true;
            fsm.BattleStart();
        }

        [PunRPC]
        void RPC_UnitStart(float hp, float damage, float Cool,int teami,int enemyi)
        {
            stat.IsUnit = true;
            stat.Char_Hp = hp;
            stat.Char_Atk_Cool = Cool;
            stat.Char_Atk_Damage = damage;
            
            
            TeamIdx = teami;
            EnemyTeamIdx = enemyi;
            gameObject.layer = 6 + teami;
            stat.currentHp = stat.HpMax();
            stat.currentMana = stat.ManaMax();
            if (!pv.IsMine)stat.ColorChange();
            stat.HpAndMpSet();
            
        }
        
        
        public void CopyStart(int lv,int[] items)
        {
            stat.IsCopy = true;
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i]>=0)
                {
                    Itemadd(items[i]);
                }
            }
            
            
            pv.RPC(nameof(RPC_CopyStart),RpcTarget.All,lv,PlayerInfo.Inst.PlayerIdx);
        }

        [PunRPC]
        void RPC_CopyStart(int lv,int PlayerIdx)
        {
            stat.HpLine.SetActive(true);
            stat.IsCopy = true;
            IsFiled = true;
            info = CsvManager.inst.characterInfo[Idx];
            Setting(lv);
            Tier = info.Tier;
            Food = info.Food;
            IsRangeAttack = info.IsRange;
            Name = info.Name;
            Icon = info.Icon;
            Character_Job1 = info.Job1;
            Character_Job2 = info.Job2;
            Character_trait1 = info.Trait1;
            Character_trait2 = info.Trait2;
            stat.Char_Range = info.Range;
            stat.Char_Defence = info.Defense;
            stat.Char_Defence_Magic = info.Defense;
            stat.Char_Speed = info.Speed;
            stat.Char_Mana = info.Mana;
            stat.Char_ManaMax = info.Mana_Max;
            stat.Char_CriPer = info.CriPer;
            stat.Char_CriDmg = info.CriDmg;
            stat.Char_Magic_Damage = 100;
            TeamIdx = PlayerIdx;
            gameObject.layer = 6 + PlayerIdx;
            stat.nav.speed = stat.Speed() * 0.01f;
            stat.currentHp = stat.HpMax();
            stat.currentMana = stat.ManaMax();
            stat.HpAndMpSet();
            if (pv.IsMine)
            {
                stat.RangeSet();
            }
            else
            {
                stat.ColorChange();
            }
        }
        public void LevelUp()
        {

            int lv = Level+1;
            pv.RPC(nameof(RPC_LevelUp),RpcTarget.All,lv);

        }

        [PunRPC]
        void RPC_LevelUp(int lv)
        {
            Level = lv;
            Setting(Level);
            if (pv.IsMine)
            {
                stat.currentHp = stat.HpMax();
                stat.HpSize();
                PlayerInfo.Inst.PlayerCardCntLv[Idx].Lv(Level-1).Remove(gameObject);
                PlayerInfo.Inst.PlayerCardCntLv[Idx].Lv(Level).Add(gameObject);
                if (Level == 2)
                {
                    CreateManager.inst.CheckLevelUp(Idx, Level);
                    if (PlayerInfo.Inst.IsBattle)
                    {
                        if (PlayerInfo.Inst.PlayerCardCntLv[Idx].Lv2.Count>=3)
                        {
                            Event_CheckLevelUp?.Dispose();
                            Event_CheckLevelUp = EventManager.inst.Sub_LevelUpCheck.Subscribe(_=>LevelUpCheck()); 
                        }
                    }
                }

                for (int i = 0; i < CardManager.inst.CardUi.Count; i++)
                {
                    if (CardManager.inst.CardUi[i].TryGetComponent(out CardUI_Info check))
                    {
                        check.CheckEffect();
                    }
                }

                
            }


            StarUI[Level - 1].SetActive(true);
     

        }

        public void remove()
        {

            PlayerInfo.Inst.PlayerCard_NoFiled.Remove(gameObject);
            PlayerInfo.Inst.PlayerCard_Filed.Remove(gameObject);
            PlayerInfo.Inst.PlayerCard.Remove(gameObject);
            //이벤트삭제
            Event_MoveReset?.Dispose();
            Event_BattleMove?.Dispose();
            Event_CheckLevelUp?.Dispose();
            Event_TJ1?.Dispose();
            Event_TJ2?.Dispose();
            Event_TJ3?.Dispose();
            Event_TJ4?.Dispose();
            Event_item40?.Dispose();

            if (TileOb.TryGetComponent(out TileInfo info))
            {
                info.RemoveTile();
            }

            PlayerInfo.Inst.PlayerCardCntRemove(Idx);
            PlayerInfo.Inst.PlayerCardCntLv[Idx].Lv(Level).Remove(gameObject);
            MasterInfo.inst.CardAdd_Lv(Idx, Level);

            if (IsFiled)
            {
                PlayerInfo.Inst.food--;
                FiledOut(true);
            }



            for (int i = 0; i < CardManager.inst.CardUi.Count; i++)
            {
                if (CardManager.inst.CardUi[i].TryGetComponent(out CardUI_Info check))
                {
                    check.CheckEffect();
                }
            }

            IsFiled = false;
            for (int i = 0; i < 3; i++)
            {
                ItemNo(i, Item[i]);
            }
            PhotonNetwork.Destroy(gameObject);
        }


        public void Setting(int Lv)
        {
            int lv = Lv - 1;
            stat.Char_Hp = info.Hp[lv];

            stat.Char_Atk_Cool = info.AtSpeed[lv];
            stat.Char_Atk_Damage = info.At[lv];
        }


        void NetCheck()
        {
            if (Level == 1)
            {

            }
            else if (Level == 2)
            {

            }
            else if (Level == 3)
            {

            }


        }

        public int costCheck()
        {
            int cost = CsvManager.inst.GoldCost(Tier, Level);
            return cost;
        }

        // public void OnPointerEnter(PointerEventData eventData)
        // {
        //     Debug.Log("ㅋ");
        //     if (ClickManager.inst.clickstate == PlayerClickState.item)
        //     {
        //         if (pv.Owner != PhotonNetwork.LocalPlayer)
        //         {
        //             return;
        //         }
        //
        //         ClickManager.inst.ItemDropCard = gameObject;
        //
        //         if (eventData.pointerDrag == ClickManager.inst.ClickItem)
        //         {
        //             if (eventData.pointerDrag.TryGetComponent(out ItemDraggable info))
        //             {
        //                 Debug.Log("들어옴");
        //                 info.SetUiCard(gameObject);
        //             }
        //         }
        //
        //     }
        // }
        //
        // public void OnPointerExit(PointerEventData eventData)
        // {
        //
        //
        //     if (ClickManager.inst.clickstate == PlayerClickState.item)
        //     {
        //         if (pv.Owner != PhotonNetwork.LocalPlayer)
        //         {
        //             return;
        //         }
        //
        //         ClickManager.inst.ItemDropCard = null;
        //         if (eventData.pointerDrag == ClickManager.inst.ClickItem)
        //         {
        //             if (eventData.pointerDrag.TryGetComponent(out ItemDraggable info))
        //             {
        //                 Debug.Log("나감");
        //                 info.outUicard();
        //             }
        //         }
        //     }
        // }

        public void ItemMove(GameObject ob1)
        {
            for (int i = 0; i < 3; i++)
            {
                if (Item[i] != -1)
                {
                    if (ItemManager.inst.ItemCheck(ob1, Item[i]))
                    {
                        if (ob1.TryGetComponent(out Card_Info info))
                        {
                            info.Itemadd(Item[i]);
                        }
                    }
                    else
                    {
                        ItemNo(i, Item[i]);
                    }
                }
            }
        }

        public void Itemadd(int idx)
        {
            int itemseat = -1;
            int itemidx = idx;
            if (idx <= 8)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (Item[i] == -1)
                    {
                        itemseat = i;

                        break;
                    }
                    else if (Item[i] >= 0 && Item[i] <= 8)
                    {
                        itemseat = i;
                        itemidx = ItemManager.inst.ItemMixIdx(Item[i], idx);
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    if (Item[i] == -1)
                    {
                        itemseat = i;

                        break;
                    }
                }
            }



            if (itemidx == -1 || itemidx >= 54)
            {

                return;
            }

            if (itemseat == -1)
            {
                ItemManager.inst.ItemAdd(itemidx);
                return;
            }

            if (Item[itemseat] >= 0 && Item[itemseat] <= 8)
            {
                //itemStatRemove(itemseat);
                //itemStatadd(itemseat, itemidx);
                pv.RPC(nameof(itemStateUp), RpcTarget.All, itemseat, itemidx);

            }
            else
            {
                pv.RPC(nameof(itemStatadd), RpcTarget.All, itemseat, itemidx);
                //itemStatadd(itemseat, itemidx);

            }


        }

        [PunRPC]
        void itemStateUp(int seat, int idx)
        {
            //스탯내림
            ItemStatMinus(Item[seat],false);
            
            int itemicon = CsvManager.inst.itemInfo[idx].Icon;
            Item[seat] = idx;
            if (ItemUI[seat].TryGetComponent(out Image image))
            {
                image.sprite = IconManager.inst.icon[itemicon];
            }

            ItemUI[seat].SetActive(true);
            ItemStatAdd(Item[seat],true);
        }

        [PunRPC]
        void itemStatRemove(int seat)
        {
            ItemStatMinus(Item[seat],true);
            ItemUI[seat].SetActive(false);
            Item[seat] = -1;
            //스탯내림 
        }

        [PunRPC]
        void itemStatadd(int seat, int idx)
        {

            int itemicon = CsvManager.inst.itemInfo[idx].Icon;
            Item[seat] = idx;
            if (ItemUI[seat].TryGetComponent(out Image image))
            {
                image.sprite = IconManager.inst.icon[itemicon];
            }

            ItemUI[seat].SetActive(true);
            ItemStatAdd(idx, true);
            //스탯올림 상호작용.. 중립몹은 ㄴ 카피도 설정안하는거 인구수증가라던가등등.
        }
        

        void ItemStatAdd(int idx,bool b)
        {
            if (idx is < 0 or > 53) return;
            var checkIteminfo = CsvManager.inst.itemInfo[idx];

            if (checkIteminfo.Hp > 0)stat.HpPlus(0,checkIteminfo.Hp,0);
            if (checkIteminfo.Attack > 0)stat.Item_Atk_Damage += checkIteminfo.Attack;
            if (checkIteminfo.AtkSpeed > 0)stat.Item_Atk_Cool += checkIteminfo.AtkSpeed;
            if (checkIteminfo.Defense > 0)stat.Item_Defence += checkIteminfo.Defense;
            if (checkIteminfo.MagicDefense > 0) stat.Item_Defence_Magic += checkIteminfo.MagicDefense;
            if (checkIteminfo.MagicAtk > 0) stat.Item_Magic_Damage += checkIteminfo.MagicAtk;
            if (checkIteminfo.Mana > 0) stat.ManaPlus(0, checkIteminfo.Mana, 0);
            if (checkIteminfo.CriPer > 0) stat.Item_CriPer += checkIteminfo.CriPer;
            if (checkIteminfo.CriDmg > 0) stat.Item_CriDmg += checkIteminfo.CriDmg;
            if (checkIteminfo.MissPer > 0) stat.Item_NoAttack += checkIteminfo.MissPer;
            if (b) stat.RPC_ReSetFunc1();
            ItemAddFunc(idx);

        }
        void ItemStatMinus(int idx,bool b)
        {
            if (idx is < 0 or > 53) return;
            
            var checkIteminfo = CsvManager.inst.itemInfo[idx];
            if (checkIteminfo.Hp > 0)stat.HpPlus(0,-checkIteminfo.Hp,0);
            if (checkIteminfo.Attack > 0)stat.Item_Atk_Damage -= checkIteminfo.Attack;
            if (checkIteminfo.AtkSpeed > 0)stat.Item_Atk_Cool -= checkIteminfo.AtkSpeed;
            if (checkIteminfo.Defense > 0)stat.Item_Defence -= checkIteminfo.Defense;
            if (checkIteminfo.MagicDefense > 0) stat.Item_Defence_Magic -= checkIteminfo.MagicDefense;
            if (checkIteminfo.MagicAtk > 0) stat.Item_Magic_Damage -= checkIteminfo.MagicAtk;
            if (checkIteminfo.Mana > 0) stat.ManaPlus(0, -checkIteminfo.Mana, 0);
            if (checkIteminfo.CriPer > 0) stat.Item_CriPer -= checkIteminfo.CriPer;
            if (checkIteminfo.CriDmg > 0) stat.Item_CriDmg -= checkIteminfo.CriDmg;
            if (checkIteminfo.MissPer > 0) stat.Item_NoAttack -= checkIteminfo.MissPer;

            if (b) stat.RPC_ReSetFunc1();
            ItemRemoveFunc(idx);
        }

        void ItemAddFunc(int idx)
        {
            if (!pv.IsMine) return;
            if (!stat.IsCard || stat.IsCopy) return;

            switch (idx)
            {
                case 17:
                    //파수꾼의 상징
                    if (IsFiled&&IsHaveJob(3,false)==false)
                    {
                        TraitJobManager.inst.TraitJobAdd(3);
                        TraitJobManager.inst.OrderList();
                    }
                    break;
                case 25:
                    //전사의 상징
                    if (IsFiled&&IsHaveJob(26,false)==false)
                    {
                        TraitJobManager.inst.TraitJobAdd(26);
                        TraitJobManager.inst.OrderList();
                    }
                    break;
                case 29:
                    
                    stat.ItemFunc29Scan.gameObject.SetActive(true);
                    break;
                case 32:
                    //기사의 상징
                    if (IsFiled&&IsHaveJob(27,false)==false)
                    {
                        TraitJobManager.inst.TraitJobAdd(27);
                        TraitJobManager.inst.OrderList();
                    }
                    break;
                case 38:
                    //보호자의 상징
                    if (IsFiled&&IsHaveJob(8,false)==false)
                    {
                        TraitJobManager.inst.TraitJobAdd(8);
                        TraitJobManager.inst.OrderList();
                    }
                    break;
                case 40:
                    
                    Event_item40?.Dispose();

                    break;
                case 43:
                    //마법사의 상징
                    if (IsFiled&&IsHaveJob(28,false)==false)
                    {
                        TraitJobManager.inst.TraitJobAdd(28);
                        TraitJobManager.inst.OrderList();
                    }
                    break;
                case 47:
                    //왕국의 상징
                    if (IsFiled&&IsHaveJob(24,false)==false)
                    {
                        TraitJobManager.inst.TraitJobAdd(24);
                        TraitJobManager.inst.OrderList();
                    }
                    break;
                case 50:
                    //유령의상징
                    if (IsFiled&&IsHaveJob(23,false)==false)
                    {
                        TraitJobManager.inst.TraitJobAdd(23);
                        TraitJobManager.inst.OrderList();
                    }
                    break;
                case 52:
                    //암살자의 상징
                    if (IsFiled&&IsHaveJob(25,false)==false)
                    {
                        TraitJobManager.inst.TraitJobAdd(25);
                        TraitJobManager.inst.OrderList();
                    }
                    break;


                case 53:
                    
                PlayerInfo.Inst.foodMax++;
                    break;
                default:
                    break;
            }


        }

        void ItemRemoveFunc(int idx)
        {
            if (!pv.IsMine) return;
            if (!stat.IsCard || stat.IsCopy) return;
            
            switch (idx)
            {
                case 17:
                    //파수꾼의 상징
                    if (IsFiled&&IsHaveJob(3,false)==false)
                    {
                        TraitJobManager.inst.TraitJobRemove(3);
                        TraitJobManager.inst.OrderList();
                    }
                    break;
                case 25:
                    //전사의 상징
                    if (IsFiled&&IsHaveJob(26,false)==false)
                    {
                        TraitJobManager.inst.TraitJobRemove(26);
                        TraitJobManager.inst.OrderList();
                    }
                    break;
                case 29:
                    if (IsItemHave(29)<=0)
                    {
                        
                    stat.ItemFunc29Scan.gameObject.SetActive(false);
                    }
                    break;
                case 32:
                    //기사의 상징
                    if (IsFiled&&IsHaveJob(27,false)==false)
                    {
                        TraitJobManager.inst.TraitJobRemove(27);
                        TraitJobManager.inst.OrderList();
                    }
                    break;
                case 38:
                    //보호자의 상징
                    if (IsFiled&&IsHaveJob(8,false)==false)
                    {
                        TraitJobManager.inst.TraitJobRemove(8);
                        TraitJobManager.inst.OrderList();
                    }
                    break;
                case 40:
                    
                    Event_item40=EventManager.inst.Sub_Item40Func.Subscribe(_x=>stat.MagicPlus(0,0,_x,true));

                    break;
                case 43:
                    //마법사의 상징
                    if (IsFiled&&IsHaveJob(28,false)==false)
                    {
                        TraitJobManager.inst.TraitJobRemove(28);
                        TraitJobManager.inst.OrderList();
                    }
                    break;
                case 47:
                    //왕국의 상징
                    if (IsFiled&&IsHaveJob(24,false)==false)
                    {
                        TraitJobManager.inst.TraitJobRemove(24);
                        TraitJobManager.inst.OrderList();
                    }
                    break;
                case 50:
                    //유령의상징
                    if (IsFiled&&IsHaveJob(23,false)==false)
                    {
                        TraitJobManager.inst.TraitJobRemove(23);
                        TraitJobManager.inst.OrderList();
                    }
                    break;
                case 52:
                    //암살자의 상징
                    if (IsFiled&&IsHaveJob(25,false)==false)
                    {
                        TraitJobManager.inst.TraitJobRemove(25);
                        TraitJobManager.inst.OrderList();
                    }
                    break;
                case 53:
                    
                    PlayerInfo.Inst.foodMax--;
                    break;
                default:
                    break;
            }
        }

        public void ItemNo(int seat, int idx)
        {
            if (idx == -1)
            {
                return;
            }

            itemStatRemove(seat);
            ItemManager.inst.ItemAdd(idx);
        }
        // Update is called once per frame



        public void MoveReset()
        {
            Vector3 pos = TileOb.transform.position;
            pos.y = 1.6f;
            transform.position = pos;
        }

        public void BattleMove(int x)
        {
            var tilecom = TileOb.GetComponent<TileInfo>();
            int tileidx = tilecom.Idx;
            int enemyidx = PlayerInfo.Inst.EnemyIdx;
            int Pidx = PlayerInfo.Inst.PlayerIdx;

            if (x == 1)
            {
                if (IsFiled)
                {
                    TileOb = PositionManager.inst.playerPositioninfo[enemyidx].EnemyFiledTile[tileidx];

                }
                else
                {
                    TileOb = PositionManager.inst.playerPositioninfo[enemyidx].EnemyPlayerTile[tileidx];
                    tilecom.tileGameob = gameObject;
                }

                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                if (IsFiled)
                {
                    TileOb = PlayerInfo.Inst.FiledTile[tileidx];

                }
                else
                {

                    TileOb = PlayerInfo.Inst.PlayerTile[tileidx];
                    tilecom.tileGameob = gameObject;
                }

                transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            MoveReset();
        }

        public void FiledIn(bool b=false)
        {
            if (!b)
            {
                PlayerInfo.Inst.PlayerCard_Filed.Add(gameObject);
                
            }
            
            PlayerInfo.Inst.PlayerCard_NoFiled.Remove(gameObject);
            PlayerInfo.Inst.PlayerFiledCardCntAdd(this);

        }

        public void FiledOut(bool b=false)
        {
            if (!b)
            {
            PlayerInfo.Inst.PlayerCard_NoFiled.Add(gameObject);
                
            }
            PlayerInfo.Inst.PlayerCard_Filed.Remove(gameObject);
            PlayerInfo.Inst.PlayerFiledCardCntRemove(this);

        }

        public int IsItemHave(int item)
        {
            if (!stat.IsCard) return 0;
            int check = 0;
            for (int i = 0; i < 3; i++)
            {
                if (Item[i]==item)
                {
                    check++;
                }
            }


            return check;
        }


        public void BattleReady()
        {
            if (!IsFiled) return;
            fsm.Enemies.Clear();
            if (stat.IsCard)
            {

                if (stat.IsCopy)
                {
                    EnemyTeamIdx = PlayerInfo.Inst.copyEnemyIdx;
                }
                else
                {
                    int Eidx = PlayerInfo.Inst.EnemyIdx;
                    EnemyTeamIdx = Eidx;
                }
            
            }
            else
            {
                EnemyTeamIdx = PlayerInfo.Inst.PlayerIdx;
            }

            
            stat.currentHp = stat.HpMax();
            stat.currentMana = stat.Mana();
            stat.ReSetFunc1();
            
            
            
            //배틀시작전
            if (stat.IsCard&&IsFiled&&PlayerInfo.Inst.TraitandJobCnt[2]>=2)
            {
                if (IsHaveJob(2))
                {
                    if (PlayerInfo.Inst.TraitandJobCnt[2] >= 4) stat.shiled += 400;
                    else if (PlayerInfo.Inst.TraitandJobCnt[2] >= 2) stat.shiled += 150;

                }
            }
            if (stat.IsCard&&IsFiled&&PlayerInfo.Inst.TraitandJobCnt[7]>=2&&IsHaveJob(7))
            {
                stat.Job7Func();
            }
            if (stat.IsCard&&IsFiled&&PlayerInfo.Inst.TraitandJobCnt[9]>=2&&IsHaveJob(9))
            {
                stat.Job9Func();
            }
            if (stat.IsCard&&IsFiled&&PlayerInfo.Inst.TraitandJobCnt[10]>=2&&IsHaveJob(10))
            {
                stat.Job10Func();
            }
            if (stat.IsCard&&IsFiled&&PlayerInfo.Inst.TraitandJobCnt[22]>=2&&IsHaveJob(22))
            {
                stat.Job22Func();
            }
            if (stat.IsCard&&IsFiled&&PlayerInfo.Inst.TraitandJobCnt[27]>=2)
            {
                stat.Job27Func();
            }
            if (stat.IsCard&&IsFiled&&PlayerInfo.Inst.TraitandJobCnt[31]>=2&&PlayerInfo.Inst.TraitandJobCnt[31]<4&&IsHaveJob(31))
            {
                stat.Job31 = true;
            }
            if (stat.IsCard&&IsFiled&&PlayerInfo.Inst.TraitandJobCnt[31]>=4)
            {
                stat.Job31 = true;
            }

            if (stat.IsCard && IsFiled && IsHaveJob(25))
            {
                float f = 18;
                    if (MoveIdx <= 7) f = 10.5f;
                    else if (MoveIdx<=14) f = 13f;
                    else if (MoveIdx<=21) f = 15.5f;
                Vector3 pos;
                if (PlayerInfo.Inst.BattleMove)
                {
                pos = transform.position + new Vector3(0, 0, -1*f);
                    
                }
                else
                {
                pos = transform.position + new Vector3(0, 0, f);
                    
                }

                transform.DOJump(pos, 3, 1, 0.5f);
            }
            
        }
        public void BattleStart()
        {
            if (!IsFiled) return;
            stat.collider.enabled = true;
            stat.nav.enabled = true;
            fsm.BattleStart();

            



            ItemFuncCheck();//아이템 액션
            
            
            
            
        }

        void ItemFuncCheck()
        {
            GameObject leftob = null;
            GameObject rightob = null;
            GameObject upob = null;
            GameObject downob = null;
            int have15 = IsItemHave(15);
            int have28 = IsItemHave(28);
            int have35 = IsItemHave(35);
            int have36 = IsItemHave(36);
            int have37 = IsItemHave(37);
            int have46 = IsItemHave(46);
            int have49 = IsItemHave(49);
            int have51 = IsItemHave(51);
            //지크의전령있을때 좌우로 공속+15%
            if (have15>0)
            {
                stat.CoolPlus(0,0,15*have15,true);
                if(leftob==null) leftob=BattleTileLeft();
                if(rightob==null) rightob=BattleTileRight();
                
                if (leftob!=null)
                {
                    if (leftob.TryGetComponent(out CardState cstat))
                    {
                        
                            cstat.CoolPlus(0,0,15*have15,true);
                        
                    }
                }
                if (rightob!=null)
                {
                    if (rightob.TryGetComponent(out CardState cstat2))
                    {
                        
                            cstat2.CoolPlus(0,0,15*have15,true);
                        
                    }
                }
                
            }
            if (have28>0)
            {
                if(leftob==null) leftob=BattleTileLeft();
                if(rightob==null) rightob=BattleTileRight();
                float sv = 300;
                if (Level == 2) sv = 400;
                else if (Level == 3) sv = 500;
                stat.shiled += sv;
                if (leftob!=null)
                {
                    if (leftob.TryGetComponent(out CardState cstat))
                    {
                        

                            cstat.shiled += sv;
                        
                    }
                }
                if (rightob!=null)
                {
                    if (rightob.TryGetComponent(out CardState cstat2))
                    {
                        
                            cstat2.shiled += sv;
                        
                    }
                }
                
            }
            if (have35>0)
            {
                if(leftob==null) leftob=BattleTileLeft();
                if(rightob==null) rightob=BattleTileRight();
                stat.MagicPlus(0,0,50*have35,true);
                if (leftob!=null)
                {
                    if (leftob.TryGetComponent(out CardState cstat))
                    {
                        

                            cstat.MagicPlus(0,0,50*have35,true);
                        
                    }
                }
                if (rightob!=null)
                {
                    if (rightob.TryGetComponent(out CardState cstat2))
                    {
                        
                            cstat2.MagicPlus(0,0,50*have35,true);
                        
                    }
                }
                
            }

            if (have37 > 0) stat.Isitem37Check(have37, true);

            if (have46>0)
            {
                for (int i = 0; i < have46; i++)
                {
                    if (Random.Range(0,100)<=50)
                    {
                        stat.MagicPlus(0, 0, 40, true);
                        stat.AtkPlus(0, 0, 40, true);
                    }
                    else
                    {
                        stat.IsItemFunc46++;
                    }
                }
            }

            if (have49 > 0) stat.IsItemFunc49 = true;

            if (have36 > 0)
            {
                Vector3 pos = PlayerInfo.Inst.EnemyFiledTile[MoveIdx].transform.position;
                //EffectManager.inst.EffectCreate("DeadEffect",pos,Quaternion.identity);
                Collider[] c = Physics.OverlapSphere(pos, 8f, GameSystem_AllInfo.inst.masks[EnemyTeamIdx]);

                dummylist.Clear();
                
                if (c.Length>0) dummylist= c.Where(ob => ob.GetComponent<Card_Info>().IsFiled&&ob.GetComponent<CardState>().IsItemFunc36==false).OrderBy(ob => Vector3.Distance(pos, ob.transform.position)).ToList();


                if (dummylist.Count>0)
                {
                    if (dummylist[0].TryGetComponent(out CardState ca))
                    {
                        if (ca.info.IsItemHave(37)>0)
                        {
                            ca.Isitem37Check(1, false);
                        }
                        else
                        {
                            
                        ca.IsItemFunc36 = true;
                        Vector3 ad = ca.transform.position;
                        ad.y = 5;
                        ca.NetStopFunc(false,5,false);
                        ca.InvinSet(5);
                        ca.Jump(ca.transform.position,10,1,5);
                        }
                    }
                }

            }

            if (have51 > 0)
            {
                int g = 2*Level;
                PlayerInfo.Inst.Gold += (g * have51);
            }

            


        }
  

        public void BattleEnd()
        {
            if (!IsFiled) return;
            
            fsm.fsm.SetState(eCardFight_STATE.None);
            //MoveReset();
            //stat.currentHp = stat.HpMax();
            //stat.currentMana = stat.Mana();
            stat.shiled = 0;
            stat.nav.enabled = false;
            stat.collider.enabled = false;
            
            // gameObject.SetActive(true);
            // stat.IsDead = false;
            stat.DeadCheck(false);
            stat.IsInvin = 0;
            IsFighting = false;
            

            stat.BattleEndReset();


            stat.PhyDmg = 0;
            stat.MagicDmg = 0;
            stat.TrueDmg = 0;
            
        }

        public void TileShow(bool b)
        {
    
                if (IsFiled)
                {
                    show_fileTileob.SetActive(b);
                }
                else
                {
                    show_Tileob.SetActive(b);
                }
            
        }

        public void LevelUpCheck()
        {
            CreateManager.inst.CheckLevelUp(Idx, Level);
            Event_CheckLevelUp.Dispose();
        }

        public void TileCheck(bool b)
        {
            pv.RPC(nameof(RPC_TileCheck),RpcTarget.All,b);
        }

        [PunRPC]
        void RPC_TileCheck(bool b)
        {
            IsFiled = b;
        }

        #region 타일오브젝트확인

        


        public GameObject BattleTileLeft()
        {
            if (MoveIdx == 0 || MoveIdx == 7 || MoveIdx == 14 || MoveIdx == 21) return null;

            GameObject tile = PositionManager.inst.playerPositioninfo[PlayerInfo.Inst.PlayerIdx].FiledTile[MoveIdx - 1];
            if (tile.TryGetComponent(out TileInfo info))
            {
                GameObject ob = info.tileGameob;
                if (ob==null)
                {
                    return null;
                }

                if (PlayerInfo.Inst.PlayerCard_Filed.Contains(ob)==false)
                {
                    return null;
                }

                return ob;
            }

            return null;
        }
        
        public GameObject BattleTileRight()
        {
            if (MoveIdx == 6 || MoveIdx == 13 || MoveIdx == 20 || MoveIdx == 27) return null;

            GameObject tile = PositionManager.inst.playerPositioninfo[PlayerInfo.Inst.PlayerIdx].FiledTile[MoveIdx + 1];
            if (tile.TryGetComponent(out TileInfo info))
            {
                GameObject ob = info.tileGameob;
                if (ob==null)
                {
                    return null;
                }

                if (PlayerInfo.Inst.PlayerCard_Filed.Contains(ob)==false)
                {
                    return null;
                }

                return ob;
            }

            return null;
        }
        public GameObject BattleTileUp()
        {
            if (MoveIdx <= 6) return null;

            GameObject tile = PositionManager.inst.playerPositioninfo[PlayerInfo.Inst.PlayerIdx].FiledTile[MoveIdx -7];
            if (tile.TryGetComponent(out TileInfo info))
            {
                GameObject ob = info.tileGameob;
                if (ob==null)
                {
                    return null;
                }

                if (PlayerInfo.Inst.PlayerCard_Filed.Contains(ob)==false)
                {
                    return null;
                }

                return ob;
            }

            return null;
        }
        public GameObject BattleTileDown()
        {
            if (MoveIdx >=21) return null;

            GameObject tile = PositionManager.inst.playerPositioninfo[PlayerInfo.Inst.PlayerIdx].FiledTile[MoveIdx +7];
            if (tile.TryGetComponent(out TileInfo info))
            {
                GameObject ob = info.tileGameob;
                if (ob==null)
                {
                    return null;
                }

                if (PlayerInfo.Inst.PlayerCard_Filed.Contains(ob)==false)
                {
                    return null;
                }

                return ob;
            }

            return null;
        }

        public bool IsHaveJob(int idx,bool ItemCheck=true)
        {
            if (!stat.IsCard) return false;
            if (Character_Job1==idx||Character_Job2==idx||Character_trait1==idx||Character_trait2==idx)
            {
                return true;
            }

            if (ItemCheck)
            {
                if (idx == 3 && IsItemHave(17) > 0)
                {
                    return true;
                }
                if (idx == 26 && IsItemHave(25) > 0)
                {
                    return true;
                }
                if (idx == 27 && IsItemHave(32) > 0)
                {
                    return true;
                }
                if (idx == 8 && IsItemHave(38) > 0)
                {
                    return true;
                }
                if (idx == 28 && IsItemHave(43) > 0)
                {
                    return true;
                }
                if (idx == 24 && IsItemHave(47) > 0)
                {
                    return true;
                }
                if (idx == 23 && IsItemHave(50) > 0)
                {
                    return true;
                }
                if (idx == 25 && IsItemHave(52) > 0)
                {
                    return true;
                }
                
                
            }

            return false;
        }

        #endregion


    }
}
