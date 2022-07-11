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
            info.pv.RPC(nameof(CreateBullet),RpcTarget.All,PlayerInfo.Inst.PlayerIdx,"Bullet_Bullet_1",Target.GetComponent<PhotonView>().ViewID,CreatePos.position,Quaternion.identity,da);

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
                info.pv.RPC(nameof(CreateBullet2),RpcTarget.All,PlayerInfo.Inst.PlayerIdx,"Bullet_Bullet_1",Target.GetComponent<PhotonView>().ViewID,CreatePos.position,Quaternion.identity,da);


                
                
                yield return YieldInstructionCache.WaitForSeconds(0.1f);
            }
            fsm.CoolStart();
            
            


        }
        
        [PunRPC]
        void CreateBullet2(int pidx,string name,int id,Vector3 pos,Quaternion qu,float da)
        {
            GameObject bullet = ObjectPooler.SpawnFromPool(name, pos, qu);
            
            if (bullet.TryGetComponent(out Buulet_Move1 move))
            {
                move.StartFUnc(pidx,gameObject,PhotonView.Find(id).gameObject,da,false,1);
            }
        }
    }
}
