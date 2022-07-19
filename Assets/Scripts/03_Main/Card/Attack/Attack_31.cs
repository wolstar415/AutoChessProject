using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Attack_31 : AttackFunc
    {
        
        public override void BasicAttack(GameObject target,float t=0.3f)
        {

            Target = target;

            stat.NetStopFunc(false,t,false);


            StartCoroutine(IAttackFunc());
        }

        
        IEnumerator IAttackFunc()
        {
            bool skill = manacheck();

            if (skill)
            {
                SkillBasic();
                stat.AniStart("Skill");
            }
            else
            {
                stat.AniStart("Attack");
                
            }
            yield return YieldInstructionCache.WaitForSeconds(0.3f);
            fsm.CoolStart();

            if (skill)
            {
                EffectManager.inst.EffectCreate("Skill31_Effect",Target.transform.position,Quaternion.identity,2f);
                float da = SkillValue(1);
                DamageManager.inst.DamageFunc1(gameObject, Target, da, eDamageType.Spell_Magic);
                
                
            }
            else
            {
                float da = stat.Atk_Damage();
                DamageManager.inst.DamageFunc1(gameObject, Target, da, eDamageType.Basic_phy);
                
            }
        }



        
    }
}
