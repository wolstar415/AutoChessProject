using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Attack_16 : AttackFunc
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



        
    }
}
