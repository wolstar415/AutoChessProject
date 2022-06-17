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
            fsm.NoConTime(stat.AtkAniTime());
            fsm.CoolStart();
            StartCoroutine(IAttackFunc());
        }

        IEnumerator IAttackFunc()
        {
            yield return YieldInstructionCache.WaitForSeconds(stat.AtkAniTime());
            DamageManager.inst.DamageFunc1(gameObject,Target,stat.Atk_Damage());

        }


    }
}
