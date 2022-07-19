using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Attack_53 : AttackFunc
    {
        
        public override void BasicAttack(GameObject target,float t=0.3f)
        {

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
                float v=SkillValue(1);

                dummy_Enemy.Clear();
                dummy_Enemy = GameSystem_AllInfo.inst.FindAllObject(transform.position, info.EnemyTeamIdx, 3.3f);
                for (int i = 0; i < dummy_Enemy.Count; i++)
                {
                    DamageManager.inst.DamageFunc1(gameObject,dummy_Enemy[i],v,eDamageType.Spell_Magic);
                    
                }
                    
                EffectManager.inst.EffectCreate("Skill53_Effect",Target.transform.position,Quaternion.Euler(-90,0,0),3f);
            }
            else
            {
                DamageManager.inst.DamageFunc1(gameObject,Target,da,eDamageType.Basic_phy);
                
            }
        }


        
    }
}
