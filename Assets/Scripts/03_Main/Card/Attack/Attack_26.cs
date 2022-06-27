using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Attack_26 : AttackFunc
    {
        
        public override void BasicAttack(GameObject target,float t=0.3f)
        {

            base.BasicAttack(target,t);


            StartCoroutine(IAttackFunc());
        }

        
        IEnumerator IAttackFunc()
        {
            float da = stat.Atk_Damage();
            stat.AniStart("Attack");
            yield return YieldInstructionCache.WaitForSeconds(0.3f);
            fsm.CoolStart();

            
            DamageManager.inst.DamageFunc1(gameObject,Target,da,eDamageType.Basic_phy);
        }

        public override void SkillFunc()
        {
            StartCoroutine(ISkillFunc());
        }

        IEnumerator ISkillFunc()
        {
            GameObject ob = GameSystem_AllInfo.inst.FindFirstObject(transform.position, info.EnemyTeamIdx, 26f, false);
            if (ob==null)  yield break;
            fsm.noCon++;
            fsm.fsm.SetState(eCardFight_STATE.NoCon);
            stat.nav.enabled = false;
            var ob_stat = ob.GetComponent<CardState>();
            while (true)
            {
                if (ob_stat.IsDead)
                {
                    stat.nav.enabled = true;
                    fsm.noCon--;
                    yield break;
                }

                transform.LookAt(ob.transform);
                Vector3 dir = ob.transform.position - transform.position;
                dir.Normalize();
                dir.y = 0;
                transform.position = transform.position + (dir * Time.deltaTime * 30);

                if (Vector3.Distance(transform.position,ob.transform.position)<=1)
                {
                    break;
                }
                yield return null;
            }
            stat.nav.enabled = true;
            fsm.noCon--;
            EffectManager.inst.EffectCreate("Skill26_Effect",transform.position,Quaternion.identity,1.5f);
            
            dummy_Enemy.Clear();
            dummy_Enemy = GameSystem_AllInfo.inst.FindAllObject(transform.position, info.EnemyTeamIdx, 3.3f);
            float d1 = SkillValue(1);
            float d2 = SkillValue(2);
            for (int i = 0; i < dummy_Enemy.Count; i++)
            {
                if (dummy_Enemy[i].TryGetComponent(out CardState enemystat))
                {
                    enemystat.NetStopFunc(true,d2,false);
                    enemystat.Jump(enemystat.transform.position,3,1,1.5f);
                    DamageManager.inst.DamageFunc1(gameObject,dummy_Enemy[i],d1,eDamageType.Speel_Magic);
                }
                
            }
            
            
        }
    }
}
