using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Attack_9 : AttackFunc
    {

        public int Skillint = 0;

        // ReSharper disable Unity.PerformanceAnalysis
        public override void BasicAttack(GameObject target,float t=0.2f)
        {
            base.BasicAttack(target);
            StartCoroutine(IAttackFunc());
        }

        IEnumerator IAttackFunc()
        {
            //float f = stat.AtkAniTime();
            //fsm.NoConTime(stat.Atk_Cool(),false);
            fsm.CoolStart();
            yield return YieldInstructionCache.WaitForSeconds(0.1f);
            AttackFunc();
        }

        void AttackFunc()
        {
            bool skill = false;
            //총알생성
            if (manacheck())
            {
                SkillBasic();
                float da = stat.Atk_Damage()*1.5f;
                float da2 = SkillValue(1);
                GameObject bullet = PhotonNetwork.Instantiate("Bullet_Skill_9", CreatePos.position, Quaternion.identity);
                if (bullet.TryGetComponent(out Buulet_Skill9_Move move))
                {
                    move.StartFUnc(gameObject,Target.transform.position,da+da2,info.EnemyTeamIdx);
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

        public void BulletFunc()
        {
            Skillint++;

            if (Skillint<=5)
            {
                stat.CoolPlus(0,0,20);
            }
        }

        public override void BattelEnd()
        {
            Skillint = 0;
        }
    }
}
