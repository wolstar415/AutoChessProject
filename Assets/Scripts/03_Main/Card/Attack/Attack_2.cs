using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Attack_2 : AttackFunc
    {

        public override void BasicAttack(GameObject target,float t=0.2f)
        {
            if (stat.currentMana>=stat.ManaMax())
            {
                Target = target;
                SkillFunc();
                SkillBasic();
                return;
            }
            base.BasicAttack(target);
            StartCoroutine(IAttackFunc());
        }

        IEnumerator IAttackFunc()
        {

            fsm.CoolStart();
            yield return YieldInstructionCache.WaitForSeconds(0.1f);
            AttackFunc();
        }

        void AttackFunc()
        {
            //총알생성
            float da = stat.Atk_Damage();
            GameObject bullet = PhotonNetwork.Instantiate("Bullet_Bullet", CreatePos.position, Quaternion.identity);
            if (bullet.TryGetComponent(out Buulet_Move1 move))
            {
                move.StartFUnc(gameObject,Target,da);
            }
        }

        public override void SkillFunc()
        {
            StartCoroutine(ISkill());
        }

        IEnumerator ISkill()
        {
            stat.NetStopFunc(false,0.2f,false);
            fsm.CoolStart();
            yield return YieldInstructionCache.WaitForSeconds(0.2f);
            if (stat.IsDead) yield break;
            dummy_Enemy.Clear();
            dummy_Enemy =GameSystem_AllInfo.inst.FindAllObject(transform.position, info.EnemyTeamIdx, 25f);

            if (dummy_Enemy.Count == 0) yield break;
            
            GameObject ob = dummy_Enemy[0];
            
            float checkf = dummy_Enemy[0].GetComponent<CardState>().Range();

            for (int i = 1; i < dummy_Enemy.Count; i++)
            {
                float check_range2 = dummy_Enemy[i].GetComponent<CardState>().Range();
                if (check_range2>checkf)
                {
                    ob= dummy_Enemy[i];
                    checkf = check_range2;
                }
            }
            EffectManager.inst.EffectCreate("Skill2_Effect",ob.transform.position,Quaternion.identity,1.5f);

            dummy_Enemy.Clear();
            dummy_Enemy =GameSystem_AllInfo.inst.FindAllObject(ob.transform.position, info.EnemyTeamIdx, 3.5f);

            float v1 = CsvManager.inst.skillinfo[info.Idx].Realcheck(1, info.Level);
            float v2 = CsvManager.inst.skillinfo[info.Idx].Realcheck(2, info.Level);
            if (dummy_Enemy.Count == 0) yield break;
            
            for (var i = 0; i < dummy_Enemy.Count; i++)
            {
                if (dummy_Enemy[i].TryGetComponent(out CardState EnemyStat))
                {
                    EnemyStat.NoAttack(true,v2);
                }
                DamageManager.inst.DamageFunc1(gameObject,dummy_Enemy[i],v1,eDamageType.Speel_Magic);
            }
            
            


        }
    }
}
