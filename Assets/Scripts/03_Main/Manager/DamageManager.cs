using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public enum eDamageType
    {
        Basic_phy,
        Basic_Magic,
        Spell_phy,
        Speel_Magic,
        True,
    }
    public class DamageManager : MonoBehaviour
    {
        public static DamageManager inst;

        private void Awake() => inst = this;
        private List<GameObject> Listob = new List<GameObject>();

        
        public void DamageFunc1(GameObject card, GameObject target,float damage,eDamageType Type=eDamageType.Basic_phy)
        {

            if (PlayerInfo.Inst.BattleEnd == true) return;
            var card_stat = card.GetComponent<CardState>();
            var target_stat = target.GetComponent<CardState>();
            float getmana=damage * 0.01f;
            float adddamage = 0f;
            bool IsCri;
            bool IsPhy = false;
            bool NoAtk = false;
            int TextColor = 0;
   
            bool IsMagic = false;
            float damageHeal = 0;
            

            

            if (card_stat.IsDead||target_stat.IsDead||!PlayerInfo.Inst.IsBattle||target_stat.IsInvin>0)
            {
                return;
            }

            if (Type!=eDamageType.True)
            {
                
                
                
                
                if (Type==eDamageType.Basic_phy||Type==eDamageType.Basic_Magic)
                {
                    
                    
                    IsPhy = true;
                    if (card_stat.info.IsItemHave(18)==0)
                        NoAtk = target_stat.NoAttackCheck(IsPhy);
                    
                    
                    card_stat.BasicFunc(target,NoAtk);
                    if (NoAtk)
                    {
//                        Debug.Log("회피중");
                        string s = CsvManager.inst.GameText(455);
                        NetworkManager.inst.TextUi(s,target.transform.position);
                        return;
                    }

                    if (card_stat.IsItemFunc49)
                    {
                        card_stat.IsItemFunc49 = false;
                        target_stat.NetStopFunc(true,4,true);
                    }
                    int item10=card_stat.info.IsItemHave(10);
                    int item21=card_stat.info.IsItemHave(21);
                    int item22=card_stat.info.IsItemHave(22);
                    int item30=card_stat.info.IsItemHave(30);
                    
                    if (item10>0)
                    {
                        adddamage = adddamage + (damage * 0.25f);
                        if (target_stat.HpMax()>2200)
                        {
                            adddamage *= 2;
                        }

                        adddamage*= item10;
                    }
                    
                    if (card_stat.info.IsItemHave(20)>0)
                    {
                        Item20Fun(card,target);
                        
                    }
                    if (item21>0)
                    {
                        card_stat.CoolPlus(0,0,item21*6,true);
                        
                    }

                    if (card_stat.info.IsItemHave(19)>0)
                    {
                        card_stat.ItemFuncAdd(19);
                        
                    }

                    if (item22>0&&card_stat.AttackCnt%3==0)
                    {
                     //스태틱 이펙트
                     DamageFunc1(card, target, 60 * item22, eDamageType.Speel_Magic);
                     
                        card_stat.ItemFuncAdd(22,true,5,false);
                    }
                    if (card_stat.info.IsItemHave(24)>0)
                    {
                        card_stat.ItemFuncAdd(24,true,5,false);
                        
                    }
                    if (item30>0)
                    {
                        //스태틱 이펙트
                        target_stat.DotDamageGo(card,target);
                    }


                    
                }

     
                if (Type==eDamageType.Speel_Magic||Type==eDamageType.Basic_Magic)
                {
                    IsMagic = true;
                    int item41=card_stat.info.IsItemHave(30);
                    if (item41>0)
                    {
                        //스태틱 이펙트
                        target_stat.DotDamageGo(card,target);
                    }
                }

                
                float daMi=1;
                IsCri = card_stat.CriCheck(IsPhy,IsMagic);
                if (IsPhy)
                {
                    daMi = 100 / (100 + target_stat.Defence());


                }
                else
                {
                    TextColor = 1;
                    daMi = 100 / (100 + target_stat.Defence_Magic());
                }

                if (IsCri)
                {
                    adddamage = adddamage + (damage * (card_stat.CriDmg() * 0.01f));

                }

            
                damage += adddamage;
                damage = damage * daMi;

                
                if (target_stat.info.IsItemHave(27)>0)
                {
                    float f = 0.25f * target_stat.info.IsItemHave(27);
                    damage = damage - (damage * f);
                }
                if (IsCri&&target_stat.info.IsItemHave(26)>0)
                {
                    damage *= 0.2f;
                }
                
            
            
                if (damage <= 0) return;
            
            
            
                if (IsPhy&&target_stat.IsItemFunc26==false&&target_stat.info.IsItemHave(26)>0)
                {
                    float d = 75;
                    if (target_stat.info.Level == 2) d = 100;
                    else if (target_stat.info.Level == 3) d = 150;
                    DamageFunc1(target,card,d,eDamageType.Speel_Magic);
                    //덤불조끼 반사!
                    target_stat.ItemFuncAdd(26, true, 2.5f,false);
                }
                getmana = getmana+(damage * 0.07f);
                getmana=Mathf.Clamp(getmana, 0, 50);
                target_stat.MpHeal(getmana);
            
            
            
                if (IsCri)
                {
                    
                    NetworkManager.inst.TextUi(damage.ToString("F0")+"!",target.transform.position,1.5f,TextColor);

                }
                else
                {
                    NetworkManager.inst.TextUi(damage.ToString("F0"),target.transform.position,1,TextColor);
                }
                
            }
            else
            {
                TextColor = 2;
                NetworkManager.inst.TextUi(damage.ToString("F0"),target.transform.position,1,TextColor);

            }
            
                UIManager.inst.DmgGo(TextColor+1, damage, card_stat.DmgIdx);
            
            //데미지체크
            //체력확인

            if (target_stat.shiled>0)
            {

                if (target_stat.shiled>damage)
                {
                    damage = 0;
                    target_stat.shiled -= damage;
                }
                else
                {
                    damage -= target_stat.shiled;
                    target_stat.shiled = 0;
                }
            }

            if (damage<=0) return;



            if (target_stat.currentHp<=damage)
            {
                target_stat.UnitDead();
                card_stat.UnitKill();
                return;
            }


            target_stat.currentHp -= damage;
            target_stat.currentMana += getmana;



            #region  아이템체크

            


            if (target_stat.info.IsItemHave(11)>0&&target_stat.IsItemFunc11==false)
            {
                float f = target_stat.currentHp / target_stat.HpMax();
                if (f<=0.6)
                {
                    target_stat.IsItemFunc11 = true;
                    target_stat.InvinSet(2);
                    
                }
            }
            if (target_stat.info.IsItemHave(12)>0&&target_stat.IsItemFunc12==false)
            {
                float max = target_stat.HpMax();
                float f = target_stat.currentHp / max;
                if (f<=0.4)
                {
                    target_stat.IsItemFunc12 = true;
                    target_stat.shiled += (max * 0.25f);
                }
            }
            if (card_stat.info.IsItemHave(13)>0)
            {
                int h = target_stat.info.IsItemHave(13);
                damageHeal += (damage * 0.25f * h);
                
            }
            if (card_stat.IsItemFunc46>0)
            {
                damageHeal += (damage * card_stat.IsItemFunc46*0.4f);
            }
            #endregion
            
            


            
            if (damageHeal>0) card_stat.HpHeal(damageHeal);


        }
        

        void Item20Fun(GameObject card,GameObject target) // 루난 주변 적 하나에게 탄환을 발사
        {
            Vector3 CreatePos = card.transform.position;
            var stat = card.GetComponent<CardState>();
            GameObject Target = null;
            Listob.Clear();
            
            Collider[] c = Physics.OverlapSphere(CreatePos, 7f, GameSystem_AllInfo.inst.masks[stat.info.EnemyTeamIdx]);


            for (var i = 0; i < c.Length; i++)
            {
                if (c[i].TryGetComponent(out Card_Info info))
                {
                    if (target!=c[i]&&info.IsFiled&&!info.stat.IsDead&&info.stat.IsInvin==0)
                    {
                        Target = c[i].gameObject;
                        break;
                    }
                }
            }


            if (Target == null) return;
            GameObject bullet = PhotonNetwork.Instantiate("Bullet_Item20", CreatePos, Quaternion.identity);
            if (bullet.TryGetComponent(out Buulet_Move1 move))
            {
                move.StartFUnc(card,Target,stat.Atk_Damage()*0.7f,false);
            }
        }



    }
}
