using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Attack_20 : AttackFunc
    {

        public override void BasicAttack(GameObject target, float t = 0.3f)
        {



                base.BasicAttack(target, t);
                StartCoroutine(IAttackFunc());
            
        }


        IEnumerator IAttackFunc()
        {
            
            bool skill = manacheck();

            if (skill)
            {
                SkillBasic();
            }
            yield return YieldInstructionCache.WaitForSeconds(0.3f);
            fsm.CoolStart();

            if (skill)
            {
                
                    float da = SkillValue(1);
                    info.pv.RPC(nameof(CreateBullet2),RpcTarget.All,PlayerInfo.Inst.PlayerIdx,"Bullet_Skill_20",CreatePos.position,Quaternion.identity,da,Target.transform.position,info.EnemyTeamIdx,false);

            }
            else
            {
                float da = stat.Atk_Damage();
            DamageManager.inst.DamageFunc1(gameObject, Target, da, eDamageType.Basic_phy);
                
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
