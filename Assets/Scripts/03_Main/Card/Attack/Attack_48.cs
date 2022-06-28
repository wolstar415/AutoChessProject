using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Attack_48 : AttackFunc
    {
        
        public override void BasicAttack(GameObject target,float t=0.2f)
        {

            base.BasicAttack(target);
            StartCoroutine(IAttackFunc());
        }

        
        IEnumerator IAttackFunc()
        {
            //float f = stat.AtkAniTime();
            fsm.CoolStart();
            yield return YieldInstructionCache.WaitForSeconds(0.1f);
            AttackFunc();
            
        }

        void AttackFunc()
        {
            bool skill = manacheck();

            if (skill)
            {
                SkillBasic();
                float da = SkillValue(1);
                GameObject bullet = PhotonNetwork.Instantiate("Bullet_Skill_48", CreatePos.position, Quaternion.identity);
                if (bullet.TryGetComponent(out Buulet_Move2 move))
                {
                    move.StartFUnc(gameObject,Target.transform.position,da,info.EnemyTeamIdx,false,false);
                }

            }
            else
            {
                float da = stat.Atk_Damage();
                GameObject bullet = PhotonNetwork.Instantiate("Bullet_Bullet", CreatePos.position, Quaternion.identity);
                if (bullet.TryGetComponent(out Buulet_Move1 move))
                {
                    move.StartFUnc(gameObject,Target,da);
                }
            }
        }



        
    }
}
