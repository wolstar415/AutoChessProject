using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Attack_49 : AttackFunc
    {
        
        public override void BasicAttack(GameObject target,float t=0.3f)
        {

            base.BasicAttack(target,t);


            StartCoroutine(IAttackFunc());
        }

        
        IEnumerator IAttackFunc()
        {
            float da = stat.Atk_Damage();
            yield return YieldInstructionCache.WaitForSeconds(0.3f);
            fsm.CoolStart();

            
            DamageManager.inst.DamageFunc1(gameObject,Target,da,eDamageType.Basic_phy);
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
            dummy_Enemy.Clear();
            dummy_Enemy = GameSystem_AllInfo.inst.FindAllObject(transform.position, info.EnemyTeamIdx, 26f);


            float v=SkillValue(1);
            int cnt = (int)SkillValue(2);
            int check = 0;
            for (int i = 0; i < dummy_Enemy.Count; i++)
            {
            DamageManager.inst.DamageFunc1(gameObject,dummy_Enemy[i],v,eDamageType.Spell_Magic);
            EffectManager.inst.EffectCreate("Skill49_Effect",dummy_Enemy[i].transform.position,Quaternion.identity,2);
                if (check>=cnt)
                {
                    break;
                }
            }
            
            

        }


        
    }
}
