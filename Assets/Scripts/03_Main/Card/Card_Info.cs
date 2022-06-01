using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

using UnityEngine.Serialization;

public enum Fight_FSM { 
None=0,
Idle,
Moving,
Attacking
}


public class Card_Info : MonoBehaviourPunCallbacks
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
    //[Header("UI")]
     
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
        PlayerInfo.Inst.PlayerCardCnt[Idx]++;
        PlayerInfo.Inst.PlayerCardCntLv[Idx].Lv(1).Add(gameObject);


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
    }

    public void remove()
    {
        if (TileOb.TryGetComponent(out TileInfo info))
        {
            info.RemoveTile();
        }
        PlayerInfo.Inst.PlayerCardCnt[Idx]--;
        PlayerInfo.Inst.PlayerCardCntLv[Idx].Lv(Level).Remove(gameObject);
        PhotonNetwork.Destroy(gameObject);
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
        return 0;
    }
    

    // Update is called once per frame


    


    
}
