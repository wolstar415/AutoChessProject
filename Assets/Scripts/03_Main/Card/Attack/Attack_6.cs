using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Attack_6 : AttackFunc
    {
        

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
                 stat.AniStart("Skill");
             }
             else
             {
                 
                 stat.AniStart("Attack");
             }
            yield return YieldInstructionCache.WaitForSeconds(0.17f);
            fsm.CoolStart();

            if (skill)
            {
                float da2=0;

                int a = GameSystem_AllInfo.inst.FindAllObject(transform.position,info.EnemyTeamIdx,3.3f).Count;
                if (a==1)
                {
                    da2 = CsvManager.inst.skillinfo[info.Idx].Realcheck(2, info.Level);
                }
                else
                {
                    da2 = CsvManager.inst.skillinfo[info.Idx].Realcheck(1, info.Level);
                    
                }
            DamageManager.inst.DamageFunc1(gameObject,Target,da+da2,eDamageType.Basic_Magic);
                EffectManager.inst.EffectCreate("Skill6_Effect",Target.transform.position,Quaternion.identity,3f);
            }
            else
            {
            DamageManager.inst.DamageFunc1(gameObject,Target,da,eDamageType.Basic_phy);
                
            }
        }



        
    }
}
