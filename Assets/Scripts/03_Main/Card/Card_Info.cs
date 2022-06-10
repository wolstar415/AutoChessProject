using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameS;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UniRx;

public enum Fight_FSM { 
None=0,
Idle,
Moving,
Attacking
}


public class Card_Info : MonoBehaviourPunCallbacks,IPointerEnterHandler,IPointerExitHandler
{
    [Header("네트워크")] public PhotonView pv;
    [Header("전투상태")] 
    public bool IsFiled;
    public bool IsFighting = false;
    public bool IsAttacker=false;
    public int TeamIdx = 0;
    public int EnemyTeamIdx = 0;
    public int Idx;
    public int MoveIdx;
    public GameObject TileOb;
    public bool IsDead = false;
    public int GoldCost;//가격

    public CharacterInfo info;
    public int Name;//이름
    public int Icon;//이름
    [Header("스탯")] 
    public int Level = 0;//레벨
    public int Tier = 0;//티어
    public int Food;//인구수
    public bool IsRangeAttack;//공격타입(원거리,근거리)
    public float Hp = 0; //체력
    public float HpMax = 0; //최대체력
    public float Range = 0f;//사정거리
    public float Atk_Cool;//공속
    public float Atk_Damage;//데미지
    public float Magic_Damage;//데미지
    public float Defence;//방어
    public float Defence_Magic;//마법방어
    public float Speed;//이동속도
    public float Mana;//마나
    public float ManaMax;//최대마나
    public float CriPer;//크리
    public float CriDmg;//크리데미지
    public int[] Item;

    [Header("캐릭터스텟")] 
    public float Char_Hp; //체력
    public float Char_Range ;//사정거리
    public float Char_Atk_Cool;//공속
    public float Char_Atk_Damage;//데미지
    public float Char_Magic_Damage;//데미지
    public float Char_Defence;//방어
    public float Char_Defence_Magic;//마법방어
    public float Char_Speed;//이동속도
    public float Char_Mana;//마나
    public float Char_ManaMax;//최대마나

    [Header("아이템스탯")] 
    public float Item_Hp; //체력
    public float Item_Range ;//사정거리
    public float Item_Atk_Cool;//공속
    public float Item_Atk_Damage;//데미지
    public float Item_Defence;//방어
    public float Item_Defence_Magic;//마법방어
    public float Item_Speed;//이동속도
    public float Item_Mana;//마나
    public float Item_ManaMax;//최대마나
    [Header("버프스탯")] 
    public float Buff_Hp; //체력
    public float Buff_Range ;//사정거리
    public float Buff_Atk_Cool;//공속
    public float Buff_Atk_Damage;//데미지
    public float Buff_Defence;//방어
    public float Buff_Defence_Magic;//마법방어
    public float Buff_Speed;//이동속도
    public float Buff_Mana;//마나
    public float Buff_ManaMax;//최대마나
    [Header("특성 계열")]
    public int Character_Job1; //계열1
    public int Character_Job2; //계열2
    public int  Character_trait1;//특성1
    public int  Character_trait2;//특성2

    [Header("UI")] 
    public GameObject[] StarUI; 
    public GameObject[] ItemUI;

    private IDisposable Event_MoveReset;
    private IDisposable Event_BattleMove;
    // Start is called before the first frame update
    void Start()
    {
        if (pv.IsMine)
        {
            
        startSetting();
        Setting(Level);
        }
        
    }
    public void startSetting()
    {
        info = CsvManager.inst.characterInfo[Idx];
        Level = 1;
        Tier = info.Tier;
        Food = info.Food;
        IsRangeAttack = info.IsRange;
        Name = info.Name;
        Icon = info.Icon;
        Character_Job1 = info.Job1;
        Character_Job2 = info.Job2;
        Character_trait1 = info.Trait1;
        Character_trait2 = info.Trait2;
        Char_Range = info.Range;
        Char_Defence = info.Defense;
        Char_Defence_Magic = info.Defense;
        Char_Speed = info.Speed;
        Char_Mana = Mana;
        Char_ManaMax = ManaMax;
        Char_Mana = info.Mana;
        Char_ManaMax = info.Mana_Max;
        PlayerInfo.Inst.PlayerCardCntAdd(Idx);
        PlayerInfo.Inst.PlayerCardCntLv[Idx].Lv(1).Add(gameObject);

        for (int i = 0; i < CardManager.inst.CardUi.Count; i++)
        {
            if (CardManager.inst.CardUi[i].TryGetComponent(out CardUI_Info check))
            {
                check.CheckEffect();
            }
        }

        //이벤트부분 등록

        Event_MoveReset=EventManager.inst.Sub_CardMove.Subscribe(_ =>
            {
                MoveReset();
            }
        );
        Event_BattleMove=EventManager.inst.Sub_CardBattleMove.Subscribe(x =>
            {
                BattleMove(x);
            }
        );
        

    }

    public void LevelUp()
    {
        PlayerInfo.Inst.PlayerCardCntLv[Idx].Lv(Level).Remove(gameObject);
        Level++;
        
        PlayerInfo.Inst.PlayerCardCntLv[Idx].Lv(Level).Add(gameObject);
        Setting(Level);
        if (Level==2)
        {
        CreateManager.inst.CheckLevelUp(Idx,Level);
           
        }
        StarUI[Level-1].SetActive(true);
        for (int i = 0; i < CardManager.inst.CardUi.Count; i++)
        {
            if (CardManager.inst.CardUi[i].TryGetComponent(out CardUI_Info check))
            {
                check.CheckEffect();
            }
        }
    }

    public void remove()
    {
        
        //이벤트삭제
        Event_MoveReset.Dispose();
        Event_BattleMove.Dispose();
        //
        if (TileOb.TryGetComponent(out TileInfo info))
        {
            info.RemoveTile();
        }
        PlayerInfo.Inst.PlayerCardCntRemove(Idx);
        PlayerInfo.Inst.PlayerCardCntLv[Idx].Lv(Level).Remove(gameObject);
        PhotonNetwork.Destroy(gameObject);

        if (IsFiled)
        {
            FiledOut();
        }
        for (int i = 0; i < 3; i++)
        {
            ItemNo(i, Item[i]);
        }
        for (int i = 0; i < CardManager.inst.CardUi.Count; i++)
        {
            if (CardManager.inst.CardUi[i].TryGetComponent(out CardUI_Info check))
            {
                check.CheckEffect();
            }
        }
    }
    

    public void Setting(int Lv)
    {
        int lv = Lv - 1;
        Char_Hp = info.Hp[lv];
        
        Char_Atk_Cool = info.AtSpeed[lv];
        Char_Atk_Damage = info.At[lv];
    }


    void NetCheck()
    {
        if (Level==1)
        {
            
        }
        else if (Level==2)
        {
            
        }
        else if(Level==3)
        {
            
        }
        
        
    }

    public int costCheck()
    {
        int cost = CsvManager.inst.GoldCost(Tier, Level);
        return cost;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {

        if (ClickManager.inst.clickstate==PlayerClickState.item)
        {
            if (pv.Owner!=PhotonNetwork.LocalPlayer)
            {
                return;
            }

            ClickManager.inst.ItemDropCard = gameObject;

            if (eventData.pointerDrag==ClickManager.inst.ClickItem)
            {
                if (eventData.pointerDrag.TryGetComponent(out ItemDraggable info))
                {
                    info.SetUiCard(gameObject);
                }
            }
            
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {

       
        if (ClickManager.inst.clickstate==PlayerClickState.item)
        {
            if (pv.Owner!=PhotonNetwork.LocalPlayer)
            {
                return;
            }
            ClickManager.inst.ItemDropCard = null;
            if (eventData.pointerDrag==ClickManager.inst.ClickItem)
            {
                if (eventData.pointerDrag.TryGetComponent(out ItemDraggable info))
                {
                    info.outUicard();
                }
            }
        }
    }

    public void ItemMove(GameObject ob1)
    {
        for (int i = 0; i < 3; i++)
        {
            if (Item[i]!=-1)
            {
                if (ItemManager.inst.ItemCheck(ob1,Item[i]))
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
        if (idx<=8)
        {
            for (int i = 0; i < 3; i++)
            {
                if (Item[i]==-1)
                {
                    itemseat = i;
                    
                    break;
                }
                else if (Item[i]>=0&&Item[i]<=8)
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
                if (Item[i]==-1)
                {
                    itemseat = i;
                    
                    break;
                }
            }
        }

        if (itemseat==-1)
        {
            return;
        }

        if (itemidx==-1 || itemidx>=54)
        {
            return;
        }


        if (Item[itemseat]>=0&&Item[itemseat]<=8)
        {
            itemStatRemove(itemseat);
            itemStatadd(itemseat, itemidx);
            
        }
        else
        {
            itemStatadd(itemseat, itemidx);
            
        }


    }

    void itemStatRemove(int seat)
    {
        ItemUI[seat].SetActive(false);
        Item[seat] = -1;
        //스탯내림
    }
    void itemStatadd(int seat,int idx)
    {
        int itemicon=CsvManager.inst.itemInfo[idx].Icon;
        Item[seat] = idx;
        if (ItemUI[seat].TryGetComponent(out Image image))
        {
            image.sprite = IconManager.inst.icon[itemicon];
        }
        ItemUI[seat].SetActive(true);
        //스탯올림 상호작용..
    }

    public void ItemNo(int seat,int idx)
    {
        if (idx==-1)
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
        
        if (x==1)
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
            transform.rotation=Quaternion.Euler(0,180,0);
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
            transform.rotation=Quaternion.Euler(0,0,0);
        }
        MoveReset();
    }

    public void FiledIn()
    {
        PlayerInfo.Inst.PlayerFiledCardCntAdd(Character_trait1,Character_trait2,Character_Job1,Character_Job2,Idx);
        
    }
    public void FiledOut()
    {
        PlayerInfo.Inst.PlayerFiledCardCntRemove(Character_trait1,Character_trait2,Character_Job1,Character_Job2,Idx);
        
    }
    
}
