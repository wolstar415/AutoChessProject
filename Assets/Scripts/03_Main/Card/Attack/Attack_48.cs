using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Attack_48 : AttackFunc
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
            bool skill = manacheck();

            if (skill)
            {
                SkillBasic();
                float da = SkillValue(1);

                info.pv.RPC(nameof(CreateBullet2),RpcTarget.All,PlayerInfo.Inst.PlayerIdx,"Bullet_Skill_48",CreatePos.position,Quaternion.identity,da,Target.transform.position,info.EnemyTeamIdx,false);


            }
            else
            {
                float da = stat.Atk_Damage();

                info.pv.RPC(nameof(CreateBullet),RpcTarget.All,PlayerInfo.Inst.PlayerIdx,"Bullet_Bullet",Target.GetComponent<PhotonView>().ViewID,CreatePos.position,Quaternion.identity,da);

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
