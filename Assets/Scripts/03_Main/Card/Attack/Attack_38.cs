using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Attack_38 : AttackFunc
    {
        
        public override void BasicAttack(GameObject target,float t=0.3f)
        {

            //base.BasicAttack(target,t);


            Target = target;
            

            
            StartCoroutine(IAttackFunc());
        }

        
        IEnumerator IAttackFunc()
        {
            bool skill = manacheck();

            if (skill)
            {
                SkillBasic();
                stat.NetStopFunc(false,0.9f,false);
            }
            else
            {
                stat.NetStopFunc(false,0.3f,false);
                
            }
            stat.AniStart("Attack");
            yield return YieldInstructionCache.WaitForSeconds(0.3f);
            fsm.CoolStart();

            if (skill)
            {
                float v = SkillValue(1)*0.01f;
                float da = stat.Atk_Damage()*v;
                DamageManager.inst.DamageFunc1(gameObject, Target, da, eDamageType.Basic_Magic);
                stat.AniStart("Attack2");
                yield return YieldInstructionCache.WaitForSeconds(0.3f);
                stat.AniStart("Attack3");
                DamageManager.inst.DamageFunc1(gameObject, Target, da, eDamageType.Basic_Magic);
                yield return YieldInstructionCache.WaitForSeconds(0.3f);
                DamageManager.inst.DamageFunc1(gameObject, Target, da, eDamageType.Basic_Magic);
            }
            else
            {
                float da = stat.Atk_Damage();
                DamageManager.inst.DamageFunc1(gameObject, Target, da, eDamageType.Basic_phy);
                
            }
        }



        
    }
}
