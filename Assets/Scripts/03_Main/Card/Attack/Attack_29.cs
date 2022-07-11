using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Attack_29 : AttackFunc
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
            bool skill = manacheck();

            if (skill)
            {
                SkillBasic();
                float da = SkillValue(1);
                info.pv.RPC(nameof(CreateBullet2),RpcTarget.All,PlayerInfo.Inst.PlayerIdx,"Bullet_Skill_29",CreatePos.position,Quaternion.identity,da,info.EnemyTeamIdx);

            }
            else
            {
                float da = stat.Atk_Damage();
                info.pv.RPC(nameof(CreateBullet),RpcTarget.All,PlayerInfo.Inst.PlayerIdx,"Bullet_Arrow",Target.GetComponent<PhotonView>().ViewID,CreatePos.position,Quaternion.identity,da);

            }
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

        [PunRPC]
        void CreateBullet2(int pidx,string name,Vector3 pos,Quaternion qu,float da,int eidx)
        {
            GameObject bullet = ObjectPooler.SpawnFromPool(name, pos, qu);
            
            if (bullet.TryGetComponent(out Buulet_Skill29_Move move))
            {
                move.StartFUnc(pidx,gameObject,da,eidx);
            }
            
            GameObject bullet2 = ObjectPooler.SpawnFromPool(name, pos, qu);
            bullet2.transform.Rotate(0,-30,0);
            if (bullet2.TryGetComponent(out Buulet_Skill29_Move move2))
            {
                move2.StartFUnc(pidx,gameObject,da,eidx);
            }
            
            GameObject bullet3 = ObjectPooler.SpawnFromPool(name, pos, qu);
            bullet3.transform.Rotate(0,30,0);
            if (bullet3.TryGetComponent(out Buulet_Skill29_Move move3))
            {
                move3.StartFUnc(pidx,gameObject,da,eidx);
            }
            
            
        }

        
    }
}
