using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Attack_1 : AttackFunc
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
            if (PlayerInfo.Inst.TraitandJobCnt[21]>=3)
            {
                float f = 30;
                if (PlayerInfo.Inst.TraitandJobCnt[21] >= 9) f = 90;
                else if (PlayerInfo.Inst.TraitandJobCnt[21] >= 6) f = 60;

                if (Random.Range(0,100f)<=f)
                {
                    yield return YieldInstructionCache.WaitForSeconds(0.1f);
                    if (Target.GetComponent<CardState>().IsDead==false)
                    {
                        AttackFunc();
                        
                    }
                }

            }
        }

        void AttackFunc()
        {
            //총알생성
            float da = stat.Atk_Damage();
            GameObject bullet = PhotonNetwork.Instantiate("Bullet_Arrow", CreatePos.position, Quaternion.identity);
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
            stat.AniStart("Skill");
            stat.NetStopFunc(false,0.4f,false);
            fsm.CoolStart();
            yield return YieldInstructionCache.WaitForSeconds(0.4f);

            
            if (stat.IsDead) yield break;
            stat.AniStart("Attack");
            GameObject ob =
                GameSystem_AllInfo.inst.FindFirstObject(transform.position, info.EnemyTeamIdx, 25f, true);

            Vector3 pos;
            if (ob==null)
            {
                pos = transform.position;
            }
            else
            {
                pos=ob.transform.position;
            }
            
            

            float da = CsvManager.inst.skillinfo[info.Idx].Realcheck(1,info.Level);
            GameObject bullet = PhotonNetwork.Instantiate("Bullet_Skill_1", CreatePos.position, Quaternion.identity);
            if (bullet.TryGetComponent(out Buulet_Move2 move))
            {
                move.StartFUnc(gameObject,pos,da,info.EnemyTeamIdx,false,false);
            }
            

        }
    }
}
