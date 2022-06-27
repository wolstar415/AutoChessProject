using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Attack_33 : AttackFunc
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
            float da = stat.Atk_Damage();
            GameObject bullet = PhotonNetwork.Instantiate("Bullet_Fire", CreatePos.position, Quaternion.identity);
            if (bullet.TryGetComponent(out Buulet_Move1 move))
            {
                move.StartFUnc(gameObject,Target,da);
            }
        }
        
        public override void SkillFunc()
        {
            stat.NetStopFunc(false,0.3f,false);
            stat.AniStart("Skill");
            StartCoroutine(ISkillFunc());
        }

        IEnumerator ISkillFunc()
        {
            
            yield return YieldInstructionCache.WaitForSeconds(0.3f);
            GameObject ob = GameSystem_AllInfo.inst.FindFirstObject(transform.position, info.EnemyTeamIdx, 26f, true);
            
            if(ob==null) yield break;
            float v=SkillValue(1);
            
            DamageManager.inst.DamageFunc1(gameObject,ob,v,eDamageType.Speel_Magic);
            EffectManager.inst.EffectCreate("Skill33_Effect",ob.transform.position,Quaternion.identity,2);

        }



        
    }
}
