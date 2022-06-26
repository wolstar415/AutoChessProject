using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Attack_7 : AttackFunc
    {

        public override void BasicAttack(GameObject target,float t=0.2f)
        {
            
            
            base.BasicAttack(target);
            StartCoroutine(IAttackFunc());
            
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
            GameObject ob = GameSystem_AllInfo.inst.FindFirstObject(transform.position, info.EnemyTeamIdx, 26f, false);
            
            if(ob==null) yield break;
            float v=SkillValue(1);
            float v2=SkillValue(2);
            if (ob.TryGetComponent(out CardState enemystat))
            {
                enemystat.NetStopFunc(true,v2,true);
            }
            DamageManager.inst.DamageFunc1(gameObject,ob,v,eDamageType.Speel_Magic);
            EffectManager.inst.EffectCreate("Skill7_Effect",ob.transform.position,Quaternion.identity,2);

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


    }
}