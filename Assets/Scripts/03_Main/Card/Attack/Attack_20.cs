using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Attack_20 : AttackFunc
    {

        public override void BasicAttack(GameObject target, float t = 0.3f)
        {



                base.BasicAttack(target, t);
                StartCoroutine(IAttackFunc());
            
        }


        IEnumerator IAttackFunc()
        {
            
            bool skill = manacheck();

            if (skill)
            {
                SkillBasic();
            }
            yield return YieldInstructionCache.WaitForSeconds(0.3f);
            fsm.CoolStart();

            if (skill)
            {
                
                GameObject bullet = PhotonNetwork.Instantiate("Bullet_Skill_20", CreatePos.position, Quaternion.identity);
                if (bullet.TryGetComponent(out Buulet_Move2 move))
                {
                    float da = SkillValue(1);
                    move.StartFUnc(gameObject,Target.transform.position,da,info.EnemyTeamIdx,false,false);
                }
            }
            else
            {
                float da = stat.Atk_Damage();
            DamageManager.inst.DamageFunc1(gameObject, Target, da, eDamageType.Basic_phy);
                
            }
            
        }
        


    }
}
