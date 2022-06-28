using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Attack_18 : AttackFunc
    {
        
        public override void BasicAttack(GameObject target,float t=0.3f)
        {

            base.BasicAttack(target,t);


            StartCoroutine(IAttackFunc());
        }

        
        IEnumerator IAttackFunc()
        {
            float da = stat.Atk_Damage();
            yield return YieldInstructionCache.WaitForSeconds(0.3f);
            fsm.CoolStart();

            
            DamageManager.inst.DamageFunc1(gameObject,Target,da,eDamageType.Basic_phy);
        }
        public override void SkillFunc()
        {
            float t = SkillValue(2);
            float v = SkillValue(1);
            stat.NetStopFunc(false,0.4f,false);
        }

        IEnumerator ISkillFunc(float v,float t)
        {
            yield return YieldInstructionCache.WaitForSeconds(0.4f);
            stat.DefencePlus(0,0,v,true);
            yield return YieldInstructionCache.WaitForSeconds(t);
            stat.DefencePlus(0,0,-v,true);
        }


        
    }
}
