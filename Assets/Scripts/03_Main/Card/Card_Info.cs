using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using UnityEngine.Serialization;

public enum Fight_FSM { 
None=0,
Idle,
Moving,
Attacking
}


public class Card_Info : MonoBehaviour
{
    public bool IsFighting = false;
    public bool IsAttacker=false;
    public int TeamIdx = 0;
    public int EnemyTeamIdx = 0;
    public int Idx;
    
    public bool IsDead = false;
    public int GoldCost;//가격

    public string Name;//이름
    [Header("스탯")] 
    public int Level = 0;//레벨
    public int Food;//인구수
    public bool IsRangeAttack;//공격타입(원거리,근거리)
    public float Hp = 0; //체력
    public float Range = 2f;//사정거리
    public float Atk_Cool;//공속
    public float Atk_Damage;//데미지
    public float Defence;//방어
    public float Defence_Magic;//마법방어
    public float Speed;//이동속도
    public float Mana;//마나
    public float ManaMax;//최대마나
    

    [Header("특성 계열")]
    public int Character_Job1; //특성1
    public int Character_Job2; //특성2
    public int  Character_trait1;//계열1
    public int  Character_trait2;//계열2
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame


    


    
}
