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
        private static readonly int Cool = Animator.StringToHash("Speed");

        public int Checkidx = 0; //중립몹전용



        public override float AtkAniTime()
        {
            float f = Item_Atk_Cool + Buff_Atk_Cool;
            f = attackTime * (1 - (f * 0.01f));
            return f;
        }
        public override float HpMax()//최대체력
        {
            float hp=Char_Hp+Item_Hp+Buff_Hp;
            Mathf.Clamp(hp, 0, 99999999);
            return hp;
        }
        public override float Mana()//마나
        {
            
            float mp=Char_Mana+Item_Mana+Buff_Mana;
            Mathf.Clamp(mp, 0, ManaMax());
            return mp;
        }
        public override float ManaMax()//최대마나
        {
            float mp=Char_ManaMax+Item_ManaMax+Buff_ManaMax;
            Mathf.Clamp(mp, 0, 9999999);
            return mp;
        }
        
        public override float NoAttack()//회피
        {
            float v = Char_NoAttack + Item_NoAttack + Buff_NoAttack;
            Mathf.Clamp(v, 0f, 100);
            return v;
        }
        public override float Range()//사정거리
        {
            float v = Char_Range + Item_Range + Buff_Range;
            Mathf.Clamp(v, 0.1f, 10f);
            return v;
        }
        public override float Atk_Cool()//공속
        {
            float v = Char_Atk_Cool;
            float v2=Item_Range+Buff_Range;
            v = v - (v * (v2 * 0.01f));
            Mathf.Clamp(v, 0.1f, 5f);
            return v;
        }

        public override float Atk_Damage() //데미지
        {
            float v = Char_Atk_Damage + Item_Atk_Damage + Buff_Atk_Damage;
            int i = info.IsItemHave(9);
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
            Mathf.Clamp(v, 0, 9999999);
            return v;
        }
        public override float Magic_Damage()//마법데미지
        {
            float v = Char_Magic_Damage + Item_Magic_Damage + Buff_Magic_Damage;
            Mathf.Clamp(v, 0, 9999999);
            return v;
        }
        public override float Defence()//방어
        {
            float v = Char_Defence + Item_Defence + Buff_Defence;
            Mathf.Clamp(v, 0, 9999999);
            return v;
        }
        public override float Defence_Magic()//마법방어
        {
            float v = Char_Defence_Magic + Item_Defence_Magic + Buff_Defence_Magic;
            Mathf.Clamp(v, 0, 9999999);
            return v;
        }
        public override float Speed()//이동속도
        {
            float v = Char_Speed + Item_Speed + Buff_Speed;
            Mathf.Clamp(v, 10, 522);
            return v;
        }

        public override float CriPer()//크리
        {
            float v = Char_CriPer + Item_CriPer + Buff_CriPer;
            Mathf.Clamp(v, 0, 100);
            return v;
        }
        public override float CriDmg()//크리데미지
        {
            float v = Char_CriDmg + Item_CriDmg + Buff_CriDmg;
            Mathf.Clamp(v, 0, 9999999);
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

            float f = (100+f2 + f3) * 0.01f;
            ani.SetFloat("Speed",f);
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
            float c = 100 + Item_Atk_Cool + Buff_Atk_Cool;
            c = c * 0.01f;
            ani.SetFloat(Cool,c);
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
            if (pv.IsMine)
            {
                currentHp = HpMax();
                currentMana = Mana();
                ReSetFunc1();
            }
        }
        public void ReSetFunc1()
        {
            pv.RPC(nameof(RPC_ReSetFunc1),RpcTarget.All);
            
        }
        [PunRPC]
        public void RPC_ReSetFunc1()
        {
            float c = 100 + Item_Atk_Cool + Buff_Atk_Cool;
            c = c * 0.01f;
            ani.SetFloat(Cool,c);
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
            pv.RPC(nameof(RPC_Dead),RpcTarget.All,true);
            
            
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

    }
}
