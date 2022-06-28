using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Attack_42 : AttackFunc
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
                
                
                
                dummy_Enemy.Clear();
                dummy_Enemy = GameSystem_AllInfo.inst.FindAllObject(transform.position, info.EnemyTeamIdx, 6f);
                float d1 = SkillValue(1);
                float d2 = SkillValue(2);
                EffectManager.inst.EffectCreate("Skill42_Effect",transform.position,Quaternion.identity,4f);

                for (int i = 0; i < dummy_Enemy.Count; i++)
                {
                    if (dummy_Enemy[i].TryGetComponent(out CardState enemystat))
                    {
                        enemystat.NetStopFunc(true,d2,false);
                        enemystat.Jump(enemystat.transform.position,3,1,1.5f);
                        DamageManager.inst.DamageFunc1(gameObject,dummy_Enemy[i],d1,eDamageType.Speel_Magic);
                    }
                
                }
                
            }
            else
            {
            DamageManager.inst.DamageFunc1(gameObject,Target,da,eDamageType.Basic_phy);
                
            }
        }



        
    }
}
