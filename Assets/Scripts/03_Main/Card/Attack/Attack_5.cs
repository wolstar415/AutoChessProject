using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Attack_5 : AttackFunc
    {
        
        public GameObject SkillEffect;
        public GameObject ManaBar;

        // ReSharper disable Unity.PerformanceAnalysis
        public override void BasicAttack(GameObject target,float t=0.2f)
        {
             if (stat.currentMana>=stat.ManaMax())
             {
                 
                 SkillFunc();
                 SkillBasic();
                 return;
             }
            base.BasicAttack(target);
            

            StartCoroutine(IAttackFunc());
        }

        
        IEnumerator IAttackFunc()
        {
            float da = stat.Atk_Damage();
            yield return YieldInstructionCache.WaitForSeconds(0.14f);
            fsm.CoolStart();
            DamageManager.inst.DamageFunc1(gameObject,Target,da,eDamageType.Basic_phy);
        }

        public override void SkillFunc()
        {
            float v = CsvManager.inst.skillinfo[info.Idx].Realcheck(1,info.Level);
            stat.NetStopFunc(false,0.2f,false);
            stat.AtkPlus(0,0,v,true);
            stat.AniStart("skill");
            EffectManager.inst.EffectCreate("Skill5_Effect",transform.position,Quaternion.identity,5);

            fsm.CoolStart(true);
        }


        
    }
}
