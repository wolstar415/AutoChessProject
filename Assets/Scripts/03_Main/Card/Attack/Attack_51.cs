using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Attack_51 : AttackFunc
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
                

                StartCoroutine(SkillFunc());

            }
            else
            {
                float da = stat.Atk_Damage();
                GameObject bullet = PhotonNetwork.Instantiate("Bullet_Dark", CreatePos.position, Quaternion.identity);
                if (bullet.TryGetComponent(out Buulet_Move1 move))
                {
                    move.StartFUnc(gameObject,Target,da);
                }
            }
        }

        IEnumerator SkillFunc()
        {
            float da = SkillValue(1)*0.1f;
            int cnt = 10;
            while (cnt > 0)
            {
                cnt--;
                GameObject bullet = PhotonNetwork.Instantiate("Bullet_Skill_51", CreatePos.position, Quaternion.identity);

                if (bullet.TryGetComponent(out Buulet_Skill51_Move move))
                {
                    move.StartFUnc(gameObject,Target,da);
                }
                yield return YieldInstructionCache.WaitForSeconds(0.1f);
            }
        }



        
    }
}
