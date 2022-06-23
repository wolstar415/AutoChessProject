using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Attack_Monster : AttackFunc
    {

        public override void BasicAttack(GameObject target)
        {
            base.BasicAttack(target);
           //fsm.NoConTime(stat.Atk_Cool(),false);
            fsm.CoolStart();
            StartCoroutine(IAttackFunc());
        }

        IEnumerator IAttackFunc()
        {
            yield return YieldInstructionCache.WaitForSeconds(0.1f);
            DamageManager.inst.DamageFunc1(gameObject,Target,stat.Atk_Damage());

        }


    }
}
