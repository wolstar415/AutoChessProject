using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Attack_10 : AttackFunc
    {
        
        public override void BasicAttack(GameObject target,float t=0.2f)
        {
            base.BasicAttack(target);
            StartCoroutine(IAttackFunc());
        }

        IEnumerator IAttackFunc()
        {
            fsm.CoolStart();
            yield return YieldInstructionCache.WaitForSeconds(0.1f);
            AttackFunc();
        }

        void AttackFunc()
        {
            //총알생성
            float da = stat.Atk_Damage();
            GameObject bullet = PhotonNetwork.Instantiate("Bullet_Bullet", CreatePos.position, Quaternion.identity);
            if (bullet.TryGetComponent(out Buulet_Move1 move))
            {
                move.StartFUnc(gameObject,Target,da);
            }
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
            stat.CoolPlus(0,0,v,true);
            yield return YieldInstructionCache.WaitForSeconds(t);
            stat.CoolPlus(0,0,-v,true);
        }
    }
}
