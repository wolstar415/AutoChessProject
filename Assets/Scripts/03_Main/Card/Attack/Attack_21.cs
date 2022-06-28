using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Attack_21 : AttackFunc
    {
        
        public override void BasicAttack(GameObject target,float t=0.3f)
        {

            base.BasicAttack(target,t);


            StartCoroutine(IAttackFunc());
        }

        
        IEnumerator IAttackFunc()
        {
            
            bool skill = manacheck();

            if (skill)
            {
                SkillBasic();
            }
            yield return YieldInstructionCache.WaitForSeconds(0.3f);
            fsm.CoolStart();
            if (skill)
            {
                float da = SkillValue(1);
            DamageManager.inst.DamageFunc1(gameObject,Target,da,eDamageType.Basic_phy);
                EffectManager.inst.EffectCreate("Skill21_Effect",Target.transform.position,Quaternion.identity,2f);
            }
            else
            {
                float da = stat.Atk_Damage();
            DamageManager.inst.DamageFunc1(gameObject,Target,da,eDamageType.Basic_phy);
                
            }
        }



        
    }
}
