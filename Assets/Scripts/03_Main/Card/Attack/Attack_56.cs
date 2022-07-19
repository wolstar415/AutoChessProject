using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Attack_56 : AttackFunc
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
                 stat.AniStart("Skill");
             }
             else
             {
                 stat.AniStart("Attack");
             }

            yield return YieldInstructionCache.WaitForSeconds(0.2f);
            fsm.CoolStart();

            if (skill)
            {
                float da2=SkillValue(1);
                float v=SkillValue(2);
                float heal = 0;
                dummy_Enemy.Clear();
                dummy_Enemy = GameSystem_AllInfo.inst.FindAllObject(transform.position, info.EnemyTeamIdx, 3.3f);
                if (dummy_Enemy.Count > 0)
                {
                    for (int i = 0; i < dummy_Enemy.Count; i++)
                    {
                        DamageManager.inst.DamageFunc1(gameObject,dummy_Enemy[i],da2,eDamageType.Spell_Magic);
                        heal += v;
                    }

                    if (heal>0)
                    {
                        stat.HpHeal(heal);
                    }
                    EffectManager.inst.EffectCreate("Skill56_Effect",transform.position,Quaternion.Euler(-90,0,0),2f);
                }
            }
            else
            {
            DamageManager.inst.DamageFunc1(gameObject,Target,da,eDamageType.Basic_phy);
                
            }
        }



        
    }
}
