using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameS
{
    public enum eDamageType
    {
        Basic_phy,
        Basic_Magic,
        Spell_phy,
        Speel_Magic,
    }
    public class DamageManager : MonoBehaviour
    {
        public static DamageManager inst;

        private void Awake() => inst = this;

        public void DamageFunc1(GameObject card, GameObject target,float damage,eDamageType Type=eDamageType.Basic_phy)
        {
            if (PlayerInfo.Inst.BattleEnd == true) return;
            var card_stat = card.GetComponent<UnitState>();
            var target_stat = target.GetComponent<UnitState>();
            float getmana=damage * 0.01f;
            bool IsCri;
            bool IsPhy = false;
            bool NoAtk = false;
            int TextColor = 0;
   
            bool IsMagic = false;

            

            if (card_stat.IsDead||target_stat.IsDead||!PlayerInfo.Inst.IsBattle||target_stat.IsInvin)
            {
                return;
            }
            if (Type==eDamageType.Basic_phy||Type==eDamageType.Basic_Magic)
            {
                
                
                IsPhy = true;
                NoAtk = target_stat.NoAttackCheck(IsPhy);
                card_stat.BasicFunc(target,NoAtk);
            }

 
            if (Type==eDamageType.Speel_Magic||Type==eDamageType.Basic_Magic)
            {
                IsMagic = true;
            }
            if (NoAtk)
            {
                Debug.Log("회피중");
                string s = CsvManager.inst.GameText(455);
                NetworkManager.inst.TextUi(s,target.transform.position);
                return;
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
                damage = damage + (damage * (card_stat.CriDmg() * 0.01f));
            }

            damage = damage * daMi;

            getmana = getmana+(damage * 0.07f);
            Mathf.Clamp(getmana, 0, 50);
            
            
            target_stat.currentMana += getmana;
            
            if (IsCri)
            {
                
                NetworkManager.inst.TextUi(damage.ToString("F0")+"!",target.transform.position,1.5f,TextColor);

            }
            else
            {
                NetworkManager.inst.TextUi(damage.ToString("F0"),target.transform.position,1,TextColor);
            }
            
            //데미지체크
            //체력확인


            if (target_stat.currentHp<=damage)
            {
                target_stat.UnitDead();
                card_stat.UnitKill();
                return;
            }

                UIManager.inst.DmgGo(TextColor+1, damage, card_stat.DmgIdx);

            target_stat.currentHp -= damage;
            target_stat.currentMana += getmana;




        }

    }
}
