using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Attack_32 : AttackFunc
    {
        
        public override void BasicAttack(GameObject target,float t=0.3f)
        {

            base.BasicAttack(target,t);


            StartCoroutine(IAttackFunc());
        }

        
        IEnumerator IAttackFunc()
        {
            float da = stat.Atk_Damage();
            stat.AniStart("Attack");
            yield return YieldInstructionCache.WaitForSeconds(0.3f);
            fsm.CoolStart();

            if (Target.TryGetComponent(out CardState enemystat))
            {
                if (enemystat.currentMana>0)
                {
                    float v = SkillValue(1);
                    enemystat.currentMana -= 20;
                    stat.shiled += v;
                }
            }
            DamageManager.inst.DamageFunc1(gameObject,Target,da,eDamageType.Basic_phy);
        }



        
    }
}
