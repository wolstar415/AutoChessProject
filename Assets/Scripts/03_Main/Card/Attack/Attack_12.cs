using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Attack_12 : AttackFunc
    {
        
        public override void BasicAttack(GameObject target,float t=0.2f)
        {

            //base.BasicAttack(target);
            Target = target;
            

            
            stat.NetStopFunc(false,0.3f,false);
            StartCoroutine(IAttackFunc());
        }

        
        IEnumerator IAttackFunc()
        {
            float da = stat.Atk_Damage();

            if (stat.currentHp>Target.GetComponent<CardState>().currentHp)
            {
                float v = SkillValue(1) * 0.01f;
                da *= v;
                stat.AniStart("Skill");
            }
            else
            {
            stat.AniStart("Attack");
                
            }
            yield return YieldInstructionCache.WaitForSeconds(0.2f);
            fsm.CoolStart();
            DamageManager.inst.DamageFunc1(gameObject,Target,da,eDamageType.Basic_phy);
        }



        
    }
}
