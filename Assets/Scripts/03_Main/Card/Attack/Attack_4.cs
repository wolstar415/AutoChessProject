using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Attack_4 : AttackFunc
    {

        public bool Skillon = false;
        public GameObject SkillEffect;
        public GameObject ManaBar;
        private Coroutine attack=null;

        // ReSharper disable Unity.PerformanceAnalysis
        public override void BasicAttack(GameObject target,float t=0.2f)
        {
             if (Skillon==false&&stat.currentMana>=stat.ManaMax())
             {
                 
                 SkillFunc();
                 SkillBasic();
                 return;
             }
            base.BasicAttack(target);
            
            if (attack != null)
            {
                StopCoroutine(attack);
            }
            attack = StartCoroutine(IAttackFunc());
        }


        public override bool SkillCheck()
        {
            if (Skillon) return false;
            if (stat.currentMana<stat.ManaMax()) // 잠시만 쓸꺼 
            {
                return false;
            }
            if (IsFastSkill)
            {
                
                SkillFunc();
                SkillBasic();
            }
            return IsFastSkill;
        }
        IEnumerator IAttackFunc()
        {
            float da = stat.Atk_Damage();
            yield return YieldInstructionCache.WaitForSeconds(0.14f);
            fsm.CoolStart();
            DamageManager.inst.DamageFunc1(gameObject,Target,da,eDamageType.Basic_phy);
            if (Skillon)
            {
                
            float da2 = CsvManager.inst.skillinfo[info.Idx].Realcheck(1,info.Level);
            DamageManager.inst.DamageFunc1(gameObject,Target,da2,eDamageType.Spell_Magic);
            }

            attack = null;
        }

        public override void SkillFunc()
        {
            if (Skillon) return;
            Skillon = true;
            stat.NetStopFunc(false,0.3f,false);
            StartCoroutine(ISkillFunc());

        }

        IEnumerator ISkillFunc()
        {
            
            
            stat.AniStart("Skill");
            yield return YieldInstructionCache.WaitForSeconds(0.3f);
            if(stat.IsDead) yield break;
            float v = CsvManager.inst.skillinfo[info.Idx].Realcheck(2,info.Level);
            info.pv.RPC(nameof(RPC_Skillfunc),RpcTarget.All,v);

            fsm.CoolStart(true);
            stat.BuffNomana = 1;
            stat.currentMana = 0;
        }
        

        public override void BattelEnd()
        {
            StopAllCoroutines();
            Skillon = false;
            stat.BuffNomana = 0;
            SkillEffect.SetActive(false);
            ManaBar.SetActive(true);
        }

        [PunRPC]
        public void RPC_Skillfunc(float v)
        {

                
                stat.CoolPlus(0,0,v,false);
                
                SkillEffect.SetActive(true);
                ManaBar.SetActive(false);
                

        }
    }
}
