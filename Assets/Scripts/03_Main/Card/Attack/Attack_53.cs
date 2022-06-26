using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Attack_53 : AttackFunc
    {
        
        public GameObject SkillEffect;
        public GameObject ManaBar;

        // ReSharper disable Unity.PerformanceAnalysis
        public override void BasicAttack(GameObject target,float t=0.2f)
        {

            //base.BasicAttack(target);
            Target = target;
            

            
            stat.NetStopFunc(false,t,false);

            StartCoroutine(IAttackFunc());
        }

        
        IEnumerator IAttackFunc()
        {
            float da = stat.Atk_Damage();
            bool skill=false;
             if (stat.currentMana>=stat.ManaMax())
             {
                 SkillBasic();
                 skill = true;
                
             }

            stat.AniStart("Attack");
            yield return YieldInstructionCache.WaitForSeconds(0.2f);
            fsm.CoolStart();

            if (skill)
            {
                float da2=SkillValue(1);
                float v=SkillValue(2);
                
                if (Target.TryGetComponent(out CardState enemystat))
                {
                    Vector3 dir = Target.transform.position - transform.position;
                    dir.Normalize();
                    dir.y = 0;
                enemystat.NetStopFunc(true,v,true);
                enemystat.Knockback(dir,20);
                    
                }
            DamageManager.inst.DamageFunc1(gameObject,Target,da+da2,eDamageType.Basic_Magic);
                EffectManager.inst.EffectCreate("Skill8_Effect",Target.transform.position,Quaternion.identity,5f);
            }
            else
            {
            DamageManager.inst.DamageFunc1(gameObject,Target,da,eDamageType.Basic_phy);
                
            }
        }



        
    }
}
