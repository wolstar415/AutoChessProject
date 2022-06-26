using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Attack_3 : AttackFunc
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
            GameObject bullet = PhotonNetwork.Instantiate("Bullet_Bullet_1", CreatePos.position, Quaternion.identity);
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
            int cnt = 5;
            stat.NetStopFunc(false,0.5f,false);
            
            while (cnt>0)
            {
                cnt--;
                if(stat.IsDead) yield break;
                if (Target==null||Target.GetComponent<CardState>().IsDead)
                {
                    Target=
                        GameSystem_AllInfo.inst.FindFirstObject(transform.position, info.EnemyTeamIdx, stat.Range(), true);

                }
                if (Target == null) break;
                float da = CsvManager.inst.skillinfo[info.Idx].Realcheck(1, info.Level);
                GameObject bullet = PhotonNetwork.Instantiate("Bullet_Bullet_1", CreatePos.position, Quaternion.identity);
                if (bullet.TryGetComponent(out Buulet_Move1 move))
                {
                    move.StartFUnc(gameObject,Target,da,false,1);
                }

                
                
                yield return YieldInstructionCache.WaitForSeconds(0.1f);
            }
            fsm.CoolStart();
            
            


        }
    }
}
