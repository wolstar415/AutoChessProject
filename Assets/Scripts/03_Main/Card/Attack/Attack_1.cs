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
            info.pv.RPC(nameof(CreateBullet),RpcTarget.All,PlayerInfo.Inst.PlayerIdx,"Bullet_Bullet",Target.GetComponent<PhotonView>().ViewID,CreatePos.position,Quaternion.identity,da);



        }

        [PunRPC]
        void CreateBullet(int pidx,string name,int id,Vector3 pos,Quaternion qu,float da)
        {
            GameObject bullet = ObjectPooler.SpawnFromPool(name, pos, qu);
            
            if (bullet.TryGetComponent(out Buulet_Move1 move))
            {
                move.StartFUnc(pidx,gameObject,PhotonView.Find(id).gameObject,da);
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
            info.pv.RPC(nameof(CreateBullet2),RpcTarget.All,PlayerInfo.Inst.PlayerIdx,"Bullet_Skill_1",CreatePos.position,Quaternion.identity,da,pos,info.EnemyTeamIdx,false);
            

        }
        
        [PunRPC]
        void CreateBullet2(int pidx,string name,Vector3 pos,Quaternion qu,float da,Vector3 pos2,int enidx,bool a)
        {
            GameObject bullet = ObjectPooler.SpawnFromPool(name, pos, qu);
            
            if (bullet.TryGetComponent(out Buulet_Move2 move))
            {
                move.StartFUnc(pidx,gameObject,pos2,da,enidx,a);
            }
        }
        
    }
}
