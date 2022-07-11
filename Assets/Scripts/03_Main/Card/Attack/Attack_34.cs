using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Attack_34 : AttackFunc
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
            GameObject ob = GameSystem_AllInfo.inst.FindFirstObject(transform.position, info.EnemyTeamIdx, 26f, false);
            
            if(ob==null) yield break;
            float v=SkillValue(1);

            info.pv.RPC(nameof(CreateBullet),RpcTarget.All,PlayerInfo.Inst.PlayerIdx,"Bullet_Skill_34",Target.GetComponent<PhotonView>().ViewID,CreatePos.position,Quaternion.identity,v);

        }
        [PunRPC]
        void CreateBullet(int pidx,string name,int id,Vector3 pos,Quaternion qu,float da)
        {
            GameObject bullet = ObjectPooler.SpawnFromPool(name, pos, qu);
            
            if (bullet.TryGetComponent(out Buulet_Skill34_Move move))
            {
                move.StartFUnc(pidx,gameObject,PhotonView.Find(id).gameObject,da);
            }
        }

        public void DamageFunc()
        {
            float v=SkillValue(2);
            stat.shiled += v;
        }

        
    }
}
