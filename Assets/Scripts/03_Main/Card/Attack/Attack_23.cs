using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Attack_23 : AttackFunc
    {
        
        public override void BasicAttack(GameObject target,float t=0.3f)
        {

            base.BasicAttack(target,t);


            StartCoroutine(IAttackFunc());
        }

        public override void BattelStart()
        {
            StartCoroutine(IFunc());
        }
        IEnumerator IFunc()
        {
            Vector3 pos;
            yield return YieldInstructionCache.WaitForSeconds(3f);
            while (!PlayerInfo.Inst.BattleEnd)
            {
                pos = transform.position;
                EffectManager.inst.EffectCreate("Skill23_Effect",pos,Quaternion.identity);
                yield return YieldInstructionCache.WaitForSeconds(0.8f);
                dummy_Enemy.Clear();
                dummy_Enemy = GameSystem_AllInfo.inst.FindAllObject(pos, info.EnemyTeamIdx, 3.3f);

                float v = SkillValue(1);
                for (int i = 0; i < dummy_Enemy.Count; i++)
                {
                    DamageManager.inst.DamageFunc1(gameObject,dummy_Enemy[i],v,eDamageType.Speel_Magic);
                }
                yield return YieldInstructionCache.WaitForSeconds(2.2f);
            }
        }

        IEnumerator IAttackFunc()
        {
            float da = stat.Atk_Damage();
            stat.AniStart("Attack");
            yield return YieldInstructionCache.WaitForSeconds(0.3f);
            fsm.CoolStart();

           
            DamageManager.inst.DamageFunc1(gameObject,Target,da,eDamageType.Basic_phy);
        }

        public override void BattelEnd()
        {
            StopAllCoroutines();
        }
    }
}
