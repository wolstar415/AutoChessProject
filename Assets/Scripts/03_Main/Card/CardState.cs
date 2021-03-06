using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class CardState : UnitState
    {
        
        [Header("캐릭터스텟")] 
        public float Char_Hp; //체력
        public float Char_Range ;//사정거리
        public float Char_Atk_Cool;//공속
        public float Char_Atk_Damage;//데미지
        public float Char_Magic_Damage;//마법데미지
        public float Char_Defence;//방어
        public float Char_Defence_Magic;//마법방어
        public float Char_Speed;//이동속도
        public float Char_Mana;//마나
        public float Char_ManaMax;//최대마나
        public float Char_CriPer;//크리
        public float Char_CriDmg;//크리데미지
        public float Char_NoAttack;//회피
        
        [Header("아이템스탯")] 
        public float Item_Hp; //체력
        public float Item_Range ;//사정거리
        public float Item_Atk_Cool;//공속
        public float Item_Atk_Damage;//데미지
        public float Item_Magic_Damage;//마법데미지
        public float Item_Defence;//방어
        public float Item_Defence_Magic;//마법방어
        public float Item_Speed;//이동속도
        public float Item_Mana;//마나
        public float Item_CriPer;//크리
        public float Item_CriDmg;//크리데미지
        public float Item_ManaMax;//최대마나
        public float Item_NoAttack;//회피
        [Header("버프스탯")] 
        public float Buff_Hp; //체력
        public float Buff_Range ;//사정거리
        public float Buff_Atk_Cool;//공속
        public float Buff_Atk_Damage;//데미지
        public float Buff_Magic_Damage;//마법데미지
        public float Buff_Defence;//방어
        public float Buff_Defence_Magic;//마법방어
        public float Buff_Speed;//이동속도
        public float Buff_Mana;//마나
        public float Buff_CriPer;//크리
        public float Buff_CriDmg;//크리데미지
        public float Buff_ManaMax;//최대마나
        public float Buff_NoAttack;//회피
        //private static readonly int Cool = Animator.StringToHash("Speed");

        public int Checkidx = 0; //중립몹전용



        // public override float AtkAniTime()
        // {
        //     float f = Item_Atk_Cool + Buff_Atk_Cool;
        //     f = attackTime * (1 - (f * 0.01f));
        //     f = Mathf.Clamp(f,0.03f, 5f);
        //     return f;
        // }
        public override float HpMax()//최대체력
        {
            float hp=Char_Hp+Item_Hp+Buff_Hp;

            if (IsCard&&info.IsFiled&&GameSystem_AllInfo.inst.playerJobcnt[info.TeamIdx].JobAndTrait[1]>=2)
            {
                if (info.IsHaveJob(1))
                {
                    if (GameSystem_AllInfo.inst.playerJobcnt[info.TeamIdx].JobAndTrait[1] >= 6) hp += 900;
                    else if (GameSystem_AllInfo.inst.playerJobcnt[info.TeamIdx].JobAndTrait[1] >= 4) hp += 500;
                    else if (GameSystem_AllInfo.inst.playerJobcnt[info.TeamIdx].JobAndTrait[1] >= 2) hp += 250;
                }
            }
            
            hp=Mathf.Clamp(hp, 0, 99999999);
            return hp;
        }
        public override float Mana()//마나
        {
            
            float mp=Char_Mana+Item_Mana+Buff_Mana;
            mp=Mathf.Clamp(mp, 0, ManaMax());
            return mp;
        }
        public override float ManaMax()//최대마나
        {
            float mp=Char_ManaMax+Item_ManaMax+Buff_ManaMax;
            mp=Mathf.Clamp(mp, 0, 9999999);
            return mp;
        }
        
        public override float NoAttack()//회피
        {
            float v = Char_NoAttack + Item_NoAttack + Buff_NoAttack;
            
            if (IsCard&&info.IsFiled&&GameSystem_AllInfo.inst.playerJobcnt[info.TeamIdx].JobAndTrait[23]>=3&&info.IsHaveJob(23))
            {
                if (GameSystem_AllInfo.inst.playerJobcnt[info.TeamIdx].JobAndTrait[23] >= 9) v += 90;
                else if (GameSystem_AllInfo.inst.playerJobcnt[info.TeamIdx].JobAndTrait[23] >= 6) v += 60;
                else v += 30;
            }
            v=Mathf.Clamp(v, 0f, 90);
            return v;
        }
        public override float Range()//사정거리
        {
            float v = Char_Range + Item_Range + Buff_Range;
            if (info.IsItemHave(18)>0)
            {
                v += (info.IsItemHave(18) * 2.6f);
            }
            v=Mathf.Clamp(v, 0.1f, 15f);
            return v;
        }
        public override float Atk_Cool()//공속
        {
            float v = Char_Atk_Cool;
            float v2=Item_Atk_Cool+Buff_Atk_Cool;
            if (IsItemFunc29 > 0) v2 -= 30f;
            if (Job31_2 >= 5) v2 += 50;
            else if (Job31_2 >= 4) v2 += 40;
            else if (Job31_2 >= 3) v2 += 30;
            else if (Job31_2 >= 2) v2 += 20;
            else if (Job31_2 >= 1) v2 += 10;

            v = v / (1 + (v2*0.01f));
            v=Mathf.Clamp(v, 0.2f, 5f);
           
            return v;
        }

        public override float Atk_Damage() //데미지
        {
            float v = Char_Atk_Damage + Item_Atk_Damage + Buff_Atk_Damage;
            int i = info.IsItemHave(9);
            if (IsItemFunc19>=25)
            {
                v += 75*info.IsItemHave(19);
            }
            else
            {
                v += IsItemFunc19 * 2*info.IsItemHave(19);
            }
            if (i>0)
            {
                if (info.Level==2)
                {
                    v += 70;
                }
                else if (info.Level == 3)
                {
                    v += 100;
                    
                }
                else
                {
                    v += 50;
                }
            }
            v=Mathf.Clamp(v, 0, 9999999);
            return v;
        }
        public override float Magic_Damage()//마법데미지
        {
            float v = Char_Magic_Damage + Item_Magic_Damage + Buff_Magic_Damage;
            if (IsCard&&info.IsFiled&&GameSystem_AllInfo.inst.playerJobcnt[info.TeamIdx].JobAndTrait[28]>=3)
            {
                if (GameSystem_AllInfo.inst.playerJobcnt[info.TeamIdx].JobAndTrait[28] >= 9) v += 200;
                else if (GameSystem_AllInfo.inst.playerJobcnt[info.TeamIdx].JobAndTrait[28] >= 6) v += 120;
                else v += 40;
            }
            if (IsItemFunc19>=25)
            {
                v += 75*info.IsItemHave(19);
            }
            else
            {
                v += IsItemFunc19 * 2*info.IsItemHave(19);
            }
            v=Mathf.Clamp(v, 0, 9999999);
            return v;
        }
        public override float Defence()//방어
        {
            float v = Char_Defence + Item_Defence + Buff_Defence;
            if (Job3) v += 50;
            if (IsItemFunc24>0)
            {
                v *= 0.5f;
            }


                
            
            v=Mathf.Clamp(v, 0, 9999999);
            return v;
        }
        public override float Defence_Magic()//마법방어
        {
            float v = Char_Defence_Magic + Item_Defence_Magic + Buff_Defence_Magic;
            if (Job3) v += 50;

            if (IsCard&&info.IsFiled&&GameSystem_AllInfo.inst.playerJobcnt[info.TeamIdx].JobAndTrait[8]>=2)
            {
                if (GameSystem_AllInfo.inst.playerJobcnt[info.TeamIdx].JobAndTrait[8] >= 4) v += 100;
                else if (GameSystem_AllInfo.inst.playerJobcnt[info.TeamIdx].JobAndTrait[8] >= 3) v += 60;
                else v += 40;
            }
            if (IsItemFunc22>0)
            {
                v *= 0.5f;
            }
            v=Mathf.Clamp(v, 0, 9999999);
            return v;
        }
        public override float Speed()//이동속도
        {
            float v = Char_Speed + Item_Speed + Buff_Speed;
            v=Mathf.Clamp(v, 10, 1100);
            return v;
        }

        public override float CriPer()//크리
        {
            float v = Char_CriPer + Item_CriPer + Buff_CriPer;
            if (IsCard&&info.IsFiled&&GameSystem_AllInfo.inst.playerJobcnt[info.TeamIdx].JobAndTrait[25]>=3&&info.IsHaveJob(25))
            {
                if (GameSystem_AllInfo.inst.playerJobcnt[info.TeamIdx].JobAndTrait[25] >= 9) v += 30;
                else if (GameSystem_AllInfo.inst.playerJobcnt[info.TeamIdx].JobAndTrait[25] >= 6) v += 20;
                else v += 5;
            }
            v=Mathf.Clamp(v, 0, 100);
            return v;
        }
        public override float CriDmg()//크리데미지
        {
            float v = Char_CriDmg + Item_CriDmg + Buff_CriDmg;
            if (IsCard&&info.IsFiled&&GameSystem_AllInfo.inst.playerJobcnt[info.TeamIdx].JobAndTrait[25]>=3&&info.IsHaveJob(25))
            {
                if (GameSystem_AllInfo.inst.playerJobcnt[info.TeamIdx].JobAndTrait[25] >= 9) v += 225;
                else if (GameSystem_AllInfo.inst.playerJobcnt[info.TeamIdx].JobAndTrait[25] >= 6) v += 150;
                else v += 75;
            }
            v=Mathf.Clamp(v, 0, 5000);
            return v;
        }

        public void HpPlus(float Char, float Item, float buff, bool network = false,bool hpSet =false)
        {
            Char_Hp += Char;
            Item_Hp += Item;
            Buff_Hp += buff;
            currentHp = currentHp + Char + Item + buff;
            if (network)
            {
            pv.RPC(nameof(RPC_HpPlus),RpcTarget.All,Char_Hp,Item_Hp,Buff_Hp,hpSet);
                
            }
        }
        public void ManaPlus(float Char, float Item, float buff, bool network = false,bool mpSet =false)
        {
            Char_Mana += Char;
            Item_Mana += Item;
            Buff_Mana += buff;
            float f1 = Char_Mana;
            float f2 = Item_Mana;
            float f3 = Buff_Mana;
            currentMana = currentMana + Char + Item + buff;
            if (network)
            {
                pv.RPC(nameof(RPC_ManaPlus),RpcTarget.All,f1,f2,f3,mpSet);
                
            }
        }
        public void ManaMaxPlus(float Char, float Item, float buff, bool network = false,bool mpSet =false)
        {
            Char_ManaMax += Char;
            Item_ManaMax += Item;
            Buff_ManaMax += buff;
            float f1 = Char_ManaMax;
            float f2 = Item_ManaMax;
            float f3 = Buff_ManaMax;
            if (network)
            {
                pv.RPC(nameof(RPC_ManaMaxPlus),RpcTarget.All,f1,f2,f3,mpSet);
                
            }
        }
        public void RangePlus(float Char, float Item, float buff, bool network = false)
        {
            Char_Range += Char;
            Item_Range += Item;
            Buff_Range += buff;
            float f1 = Char_Range;
            float f2 = Item_Range;
            float f3 = Buff_Range;
            RangeSet();
            if (network)
            {
                pv.RPC(nameof(RPC_RangePlus),RpcTarget.All,f1,f2,f3);
                
            }
        }
        public void CoolPlus(float Char, float Item, float buff, bool network = false)
        {
            Char_Atk_Cool += Char;
            Item_Atk_Cool += Item;
            Buff_Atk_Cool += buff;
            float f1 = Char_Atk_Cool;
            float f2 = Item_Atk_Cool;
            float f3 = Buff_Atk_Cool;
            if (network)
            {
                pv.RPC(nameof(RPC_CoolPlus),RpcTarget.All,f1,f2,f3);
                
            }
            else
            { 
                //float f = (100+f2 + f3) * 0.01f;
                //ani.SetFloat(Cool,f);
            }
        }
        public void AtkPlus(float Char, float Item, float buff, bool network = false)
        {
            Char_Atk_Damage += Char;
            Item_Atk_Damage += Item;
            Buff_Atk_Damage += buff;
            float f1 = Char_Atk_Damage;
            float f2 = Item_Atk_Damage;
            float f3 = Buff_Atk_Damage;
            if (network)
            {
                pv.RPC(nameof(RPC_AtkPlus),RpcTarget.All,f1,f2,f3);
                
            }
        }
        public void MagicPlus(float Char, float Item, float buff, bool network = false)
        {
            Char_Magic_Damage += Char;
            Item_Magic_Damage += Item;
            Buff_Magic_Damage += buff;
            float f1 = Char_Magic_Damage;
            float f2 = Item_Magic_Damage;
            float f3 = Buff_Magic_Damage;
            if (network)
            {
                pv.RPC(nameof(RPC_MagicPlus),RpcTarget.All,f1,f2,f3);
                
            }
        }
        public void DefencePlus(float Char, float Item, float buff, bool network = false)
        {
            Char_Defence += Char;
            Item_Defence += Item;
            Buff_Defence += buff;
            float f1 = Char_Defence;
            float f2 = Item_Defence;
            float f3 = Buff_Defence;
            if (network)
            {
                pv.RPC(nameof(RPC_DefencePlus),RpcTarget.All,f1,f2,f3);
                
            }
        }
        public void Defence_MagicPlus(float Char, float Item, float buff, bool network = false)
        {
            Char_Defence_Magic += Char;
            Item_Defence_Magic += Item;
            Buff_Defence_Magic += buff;
            float f1 = Char_Defence_Magic;
            float f2 = Item_Defence_Magic;
            float f3 = Buff_Defence_Magic;
            if (network)
            {
                pv.RPC(nameof(RPC_Defence_MagicPlus),RpcTarget.All,f1,f2,f3);
                
            }
        }
        public void SpeedPlus(float Char, float Item, float buff, bool network = false)
        {
            Char_Speed += Char;
            Item_Speed += Item;
            Buff_Speed += buff;
            float f1 = Char_Speed;
            float f2 = Item_Speed;
            float f3 = Buff_Speed;
            if (network)
            {
                pv.RPC(nameof(RPC_SpeedPlus),RpcTarget.All,f1,f2,f3);
                
            }
            
        }
        public void CriPerPlus(float Char, float Item, float buff, bool network = false)
        {
            Char_CriPer += Char;
            Item_CriPer += Item;
            Buff_CriPer += buff;
            float f1 = Char_CriPer;
            float f2 = Item_CriPer;
            float f3 = Buff_CriPer;
            if (network)
            {
                pv.RPC(nameof(RPC_CriPerPlus),RpcTarget.All,f1,f2,f3);
                
            }
        }
        public void CriDmgPlus(float Char, float Item, float buff, bool network = false)
        {
            Char_CriDmg += Char;
            Item_CriDmg += Item;
            Buff_CriDmg += buff;
            float f1 = Char_CriDmg;
            float f2 = Item_CriDmg;
            float f3 = Buff_CriDmg;
            if (network)
            {
                pv.RPC(nameof(RPC_CriDmgPlus),RpcTarget.All,f1,f2,f3);
                
            }
        }
        public void NoAttackPlus(float Char, float Item, float buff, bool network = false)
        {
            Char_NoAttack += Char;
            Item_NoAttack += Item;
            Buff_NoAttack += buff;
            float f1 = Char_NoAttack;
            float f2 = Item_NoAttack;
            float f3 = Buff_NoAttack;
            if (network)
            {
                pv.RPC(nameof(RPC_NoAttackPlus),RpcTarget.All,f1,f2,f3);
                
            }
        }
        [PunRPC]
        void RPC_NoAttackPlus(float Char, float Item, float buff,bool hpset)
        {
            Char_NoAttack = Char;
            Item_NoAttack = Item;
            Buff_NoAttack = buff;

        }
        [PunRPC]
        void RPC_HpPlus(float Char, float Item, float buff,bool hpset)
        {
            Char_Hp = Char;
            Item_Hp = Item;
            Buff_Hp = buff;
            
            
            if (hpset)
            {
                HpAndMpSet();
            }


            
        }
        [PunRPC]
        void RPC_ManaPlus(float Char, float Item, float buff,bool mpset)
        {
            Char_Mana = Char;
            Item_Mana = Item;
            Buff_Mana = buff;
            if (mpset)
            {
                HpAndMpSet();
            }
            
        }
        [PunRPC]
        void RPC_ManaMaxPlus(float Char, float Item, float buff,bool mpset)
        {
            Char_ManaMax = Char;
            Item_ManaMax = Item;
            Buff_ManaMax = buff;
            if (mpset)
            {
                HpAndMpSet();
            }
            
        }
        [PunRPC]
        void RPC_RangePlus(float Char, float Item, float buff)
        {
            Char_Range = Char;
            Item_Range = Item;
            Buff_Range = buff;
        }
        [PunRPC]
        void RPC_CoolPlus(float Char, float Item, float buff)
        {
            Char_Atk_Cool = Char;
            Item_Atk_Cool = Item;
            Buff_Atk_Cool = buff;
            //float c = 100 + Item_Atk_Cool + Buff_Atk_Cool;
            //c = c * 0.01f;
            //ani.SetFloat(Cool,c);
        }
        [PunRPC]
        void RPC_AtkPlus(float Char, float Item, float buff)
        {
            Char_Atk_Damage = Char;
            Item_Atk_Damage = Item;
            Buff_Atk_Damage = buff;
        }
        [PunRPC]
        void RPC_MagicPlus(float Char, float Item, float buff)
        {
            Char_Magic_Damage = Char;
            Item_Magic_Damage = Item;
            Buff_Magic_Damage = buff;
        }
        [PunRPC]
        void RPC_DefencePlus(float Char, float Item, float buff)
        {
            Char_Defence = Char;
            Item_Defence = Item;
            Buff_Defence = buff;
        }
        [PunRPC]
        void RPC_Defence_MagicPlus(float Char, float Item, float buff)
        {
            Char_Defence_Magic = Char;
            Item_Defence_Magic = Item;
            Buff_Defence_Magic = buff;
        }
        [PunRPC]
        void RPC_SpeedPlus(float Char, float Item, float buff)
        {
            Char_Speed = Char;
            Item_Speed = Item;
            Buff_Speed = buff;
            nav.speed = Speed()*0.01f;
        }
        [PunRPC]
        void RPC_CriPerPlus(float Char, float Item, float buff)
        {
            Char_CriPer = Char;
            Item_CriPer = Item;
            Buff_CriPer = buff;
        }
        [PunRPC]
        void RPC_CriDmgPlus(float Char, float Item, float buff)
        {
            Char_CriDmg = Char;
            Item_CriDmg = Item;
            Buff_CriDmg = buff;
        }

        public void Stat_AllSet(bool HpAndMpSet=false)
        {
            Char_Set();
            Item_Set();
            Buff_Set(HpAndMpSet);
        }


        public void Char_Set(bool HpAndMpReset=false)
        {
            float[] stat =
            {
                Char_Hp,
                Char_Range,
                Char_Atk_Cool,
                Char_Atk_Damage,
                Char_Magic_Damage,
                Char_Defence,
                Char_Defence_Magic,
                Char_Speed,
                Char_Mana,
                Char_ManaMax,
                Char_CriPer,
                Char_CriDmg,
                
            };
            pv.RPC(nameof(RPC_Char_Set),RpcTarget.All,stat);
            if (HpAndMpReset) AllHpAndMp();
        }
        public void Item_Set(bool HpAndMpReset=false)
        {
            float[] stat =
            {
                Item_Hp,
                Item_Range,
                Item_Atk_Cool,
                Item_Atk_Damage,
                Item_Magic_Damage,
                Item_Defence,
                Item_Defence_Magic,
                Item_Speed,
                Item_Mana,
                Item_ManaMax,
                Item_CriPer,
                Item_CriDmg,
                
            };
            pv.RPC(nameof(RPC_Item_Set),RpcTarget.All,stat);
            if (HpAndMpReset) AllHpAndMp();
        }
        public void Buff_Set(bool HpAndMpReset=false)
        {
            float[] stat =
            {
                Buff_Hp,
                Buff_Range,
                Buff_Atk_Cool,
                Buff_Atk_Damage,
                Buff_Magic_Damage,
                Buff_Defence,
                Buff_Defence_Magic,
                Buff_Speed,
                Buff_Mana,
                Buff_ManaMax,
                Buff_CriPer,
                Buff_CriDmg,
                
            };
            pv.RPC(nameof(RPC_Buff_Set),RpcTarget.All,stat);
            if (HpAndMpReset) AllHpAndMp();
        }

        [PunRPC]
        void RPC_Char_Set(float[] stat)
        {
            Char_Hp= stat[0];
            Char_Range= stat[1];
            Char_Atk_Cool= stat[2];
            Char_Atk_Damage= stat[3];
            Char_Magic_Damage= stat[4];
            Char_Defence= stat[5];
            Char_Defence_Magic= stat[6];
            Char_Speed= stat[7];
            Char_Mana= stat[8];
            Char_ManaMax= stat[9];
            Char_CriPer= stat[10];
            Char_CriDmg= stat[11];
            
            
            
        }
        [PunRPC]
        void RPC_Item_Set(float[] stat)
        {
            Item_Hp= stat[0];
            Item_Range= stat[1];
            Item_Atk_Cool= stat[2];
            Item_Atk_Damage= stat[3];
            Item_Magic_Damage= stat[4];
            Item_Defence= stat[5];
            Item_Defence_Magic= stat[6];
            Item_Speed= stat[7];
            Item_Mana= stat[8];
            Item_ManaMax= stat[9];
            Item_CriPer= stat[10];
            Item_CriDmg= stat[11];
            
        }
        [PunRPC]
        void RPC_Buff_Set(float[] stat)
        {
            Buff_Hp= stat[0];
            Buff_Range= stat[1];
            Buff_Atk_Cool= stat[2];
            Buff_Atk_Damage= stat[3];
            Buff_Magic_Damage= stat[4];
            Buff_Defence= stat[5];
            Buff_Defence_Magic= stat[6];
            Buff_Speed= stat[7];
            Buff_Mana= stat[8];
            Buff_ManaMax= stat[9];
            Buff_CriPer= stat[10];
            Buff_CriDmg= stat[11];
            
        }

        public void BattleEndReset()
        {
            AttackCnt = 0;
            AttackOb = null;
            OneAttackCnt = 0;
            
            // if (TryGetComponent(out CapsuleCollider col))
            // {
            //     col.enabled = true;
            // }
            pv.RPC(nameof(RPC_BattleEndReset),RpcTarget.All);
        }

        [PunRPC]
        void RPC_BattleEndReset()
        {
            Buff_Hp= 0;
            Buff_Range= 0;
            Buff_Atk_Cool= 0;
            Buff_Atk_Damage= 0;
            Buff_Magic_Damage= 0;
            Buff_Defence= 0;
            Buff_Defence_Magic= 0;
            Buff_Speed= 0;
            Buff_Mana= 0;
            Buff_ManaMax= 0;
            Buff_CriPer = 0;
            Buff_CriDmg= 0;
            Buff_NoAttack= 0;
            
            IsItemFunc11 = false;
            IsItemFunc12 = false;
            IsItemFunc19 = 0;
            IsItemFunc22 = 0;
            IsItemFunc24 = 0;
            IsItemFunc26 = false;
            IsItemFunc36 = false;
            IsItemFunc37 = 0;
            IsItemFunc49 = false;


            if (Job3)
            {
                Job3 = false;
                job3Effect?.SetActive(false);
                
            }
            if (Job4)
            {
                Job4 = false;
                job4Effect?.SetActive(false);
                
            }
            if (Job29)
            {
                Job29 = false;
                job29Effect?.SetActive(false);
                
            }
            Job4 = false;
            Job7 = 0;
            Job9 = false;
            Job10 = 0;
            Job27 = 0;
            
            Job31 = false;
            Job31_2 = 0;
            BuffNomana = 0;
            AttackFailed = 0;



            NoHeal = false;
            CorDotDamage = null;
            
            
            IsItemFunc46 = 0;
            
            if (pv.IsMine)
            {
                currentHp = HpMax();
                currentMana = Mana();
                ReSetFunc1();
                if (StunOb.activeSelf)
                {
                    StunShow(false);
                }
                
            }
            StopAllCoroutines();

            IsInvin = 0;
            IsDead = false;
            IsStun = false;

            info.fsm.attackFunc.BattelEnd();
        }
        public void ReSetFunc1()
        {
            pv.RPC(nameof(RPC_ReSetFunc1),RpcTarget.All);
            
        }
        [PunRPC]
        public void RPC_ReSetFunc1()
        {
            //float c = 100 + Item_Atk_Cool + Buff_Atk_Cool;
           // c = c * 0.01f;
            //c = Mathf.Clamp(c, 0.1f, 5);
            //ani.SetFloat(Cool,c);
            nav.speed = Speed()*0.01f;
            HpAndMpSet();
            if (pv.IsMine)
            {
                RangeSet();
            }

        

        }



        public void HpAndMpSet()
        {
            
            
            if (currentMana==0||ManaMax()==0)
            {
                ManaSlider.value = 0;
            }
            else
            { 
                float mper = currentMana / ManaMax() ;
                ManaSlider.value = mper;
            }
            

            if (currentHp==0||HpMax()==0)
            {
                HpSlider.value = 0;
                HpSliderCheck.value = 0;
            }
            else
            {

                float hper = currentHp / HpMax();
                HpSlider.value = hper;
                HpSliderCheck.value = hper;
            }

            HpSize();
        }

        public void AllHpAndMp()
        {
            pv.RPC(nameof(RPC_HPAndMpSet),RpcTarget.All);
        }
        [PunRPC]
        void RPC_HPAndMpSet()
        {
            HpAndMpSet();
        }
        
        public override void UnitKill()
        {
            KillCnt++;
        }

        public override void UnitDead()
        {
            int have23 = info.IsItemHave(23);
            int have45 = info.IsItemHave(45);



            
            
            
            
            if (!PlayerInfo.Inst.PVP)
            {
                if (!IsCard)
                {
                    PlayerInfo.Inst.PveDeadCheck();

                    pveDeadItem();
                }
                else
                {
                    PlayerInfo.Inst.DeadCheck();
                }
            }
            else
            {
                pv.RPC(nameof(RPC_UnitDead),RpcTarget.All,info.TeamIdx);
                
            }
            if (have23>0)
            {
                
                int lv = info.Level;
                float hp=1000*lv*3;
                float damage=30*lv;
                float cool=0.5f;
                float range=2.6f;
                float speed=350f;
                for (int i = 0; i < have23; i++)
                {
                    
                    NetworkManager.inst.UnitCreate(IsCopy,"Unit_item23",transform.position,Quaternion.identity,hp,damage,cool,range,speed,info.TeamIdx,info.EnemyTeamIdx,DmgIdx);
                }
            }
            if (have45>0)
            {
                //구원이펙트
                Collider[] c = Physics.OverlapSphere(transform.position, 10f, GameSystem_AllInfo.inst.masks[info.TeamIdx]);


                for (var i = 0; i < c.Length; i++)
                {
                    if (c[i].TryGetComponent(out Card_Info cc))
                    {
                        if (cc.IsFiled&&!cc.stat.IsDead)
                        {
                            cc.stat.HpHeal(800*have45);
                        }
                    }
                }
            }
            //pv.RPC(nameof(RPC_Dead),RpcTarget.All,true);
            DeadCheck(true);
            if (info.IsItemHave(29)>0)
            {
                if (ItemFunc29Scan.Enemies.Count>0)
                {
                    for (int i = 0; i < ItemFunc29Scan.Enemies.Count; i++)
                    {
                        if (ItemFunc29Scan.Enemies[i].TryGetComponent(out CardState c))
                        {
                            c.Item29Func(false);
                        }
                    }   
                }
            }


        }

        [PunRPC]
        void RPC_UnitDead(int pidx)
        {
            if (PlayerInfo.Inst.PlayerIdx==pidx)
            {
                
                PlayerInfo.Inst.DeadCheck(IsCopy);
            }


            
            
        }

        public void DeadCheck(bool b)
        {
            StopAllCoroutines();
            pv.RPC(nameof(RPC_Dead),RpcTarget.All,b);
        }

        [PunRPC]
        void RPC_Dead(bool b)
        {
            IsDead = b;
            gameObject.SetActive(!b);
        }


        public void pveDeadItem()
        {
            if (Checkidx==0)
            {
                int itemidx = UnityEngine.Random.Range(0, 8);
                EffectManager.inst.pveItemCreate(transform.position,"Box",itemidx,1);
                
            }
            else if (Checkidx==1)
            {
                int coinidx = UnityEngine.Random.Range(1, 3);
                EffectManager.inst.pveItemCreate(transform.position,"Coin",0,coinidx);
            }
            else if (Checkidx==2)
            {
                int itemidx = UnityEngine.Random.Range(0, 8);
                int coinidx = UnityEngine.Random.Range(0, 3);
                EffectManager.inst.pveItemCreate(transform.position,"Box",itemidx,1);
                EffectManager.inst.pveItemCreate(transform.position,"Coin",0,coinidx);
            }
            else if (Checkidx==100)
            {
                if (info.Item[0]>=0)
                {
                    EffectManager.inst.pveItemCreate(transform.position,"Box",info.Item[0],1);
                }
            }
            

            
            
        }
        
        public void Item29Func(bool b)
        {
            pv.RPC(nameof(RPC_Item29Func),RpcTarget.All,b);
        }

        [PunRPC]
        void RPC_Item29Func(bool b)
        {
            if (b)
            {
                IsItemFunc29++;
            }
            else
            {
                    
                IsItemFunc29--;
            }
            
            if (IsItemFunc29==1)
            {
                float f2 = Item_Atk_Cool;
                float f3 = Buff_Atk_Cool;
                float f = (70+f2 + f3) * 0.01f;
                //ani.SetFloat(Cool,f);
            }
            else
            {
                float f2 = Item_Atk_Cool;
                float f3 = Buff_Atk_Cool;
                float f = (100+f2 + f3) * 0.01f;
                //ani.SetFloat(Cool,f);
            }
        }

        public void Job22Func()
        {
            if (IsDead) return;

            StartCoroutine(IJob22Func());
        }

        IEnumerator IJob22Func()
        {
            float v = 75;
            if (PlayerInfo.Inst.TraitandJobCnt[22] >= 4) v = 150;
            yield return YieldInstructionCache.WaitForSeconds(5);
            while (true)
            {
                if (IsDead||PlayerInfo.Inst.BattleEnd||PlayerInfo.Inst.IsBattle==false)
                {
                    break;
                }
                CoolPlus(0,0,v);
                EffectManager.inst.EffectCreate("Job22_Effect",transform.position,Quaternion.Euler(-90,0,0),1.5f);
            yield return YieldInstructionCache.WaitForSeconds(4);
            CoolPlus(0,0,-v);
            yield return YieldInstructionCache.WaitForSeconds(4);
            }
        }
        
        public void Job27Func()
        {
            int cnt = PlayerInfo.Inst.TraitandJobCnt[27];
            pv.RPC(nameof(RPC_Job27Func),RpcTarget.All,cnt);

        }
        
        [PunRPC]
        void RPC_Job27Func(int cnt)
        {
            Job27 = cnt;
        }

    }
    
    

}
