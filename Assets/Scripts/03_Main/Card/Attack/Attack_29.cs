using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Attack_29 : AttackFunc
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
            
            
            
            //총알생성
            bool skill = manacheck();

            if (skill)
            {
                SkillBasic();
                float da = SkillValue(1);
                GameObject bullet = PhotonNetwork.Instantiate("Bullet_Skill_29", CreatePos.position, Quaternion.identity);
                if (bullet.TryGetComponent(out Buulet_Move2 move))
                {
                    move.StartFUnc(gameObject,Target.transform.position,da,info.EnemyTeamIdx,true,false);
                }
                GameObject bullet1 = PhotonNetwork.Instantiate("Bullet_Skill_29", CreatePos.position, Quaternion.identity);
                if (bullet1.TryGetComponent(out Buulet_Move2 move1))
                {
                    move1.StartFUnc(gameObject,Target.transform.position,da,info.EnemyTeamIdx,true,false);
                    move1.transform.Rotate(0,-30,0);
                }
                GameObject bullet2 = PhotonNetwork.Instantiate("Bullet_Skill_29", CreatePos.position, Quaternion.identity);
                if (bullet2.TryGetComponent(out Buulet_Move2 move2))
                {
                    move2.StartFUnc(gameObject,Target.transform.position,da,info.EnemyTeamIdx,true,false);
                    move2.transform.Rotate(0,30,0);
                }
                
            }
            else
            {
                float da = stat.Atk_Damage();
                GameObject bullet = PhotonNetwork.Instantiate("Bullet_Arrow", CreatePos.position, Quaternion.identity);
                if (bullet.TryGetComponent(out Buulet_Move1 move))
                {
                    move.StartFUnc(gameObject,Target,da);
                }
            }
        }



        
    }
}
