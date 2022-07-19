using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Attack_13 : AttackFunc
    {
        
        public override void BasicAttack(GameObject target,float t=0.3f)
        {
            
            
            
            if (manacheck())
            {
                Target = target;
            StartCoroutine(ISkillFunc());
                SkillBasic();
            }
            else
            {
                base.BasicAttack(target,t);
            StartCoroutine(IAttackFunc());
            }
            
        }

 

        IEnumerator ISkillFunc()
        {
            stat.NetStopFunc(false,0.4f,false);
            stat.AniStart("Skill");
            yield return YieldInstructionCache.WaitForSeconds(0.4f);
            fsm.CoolStart();
            float v=SkillValue(1);
            
            dummy_Enemy.Clear();
            dummy_Enemy = GameSystem_AllInfo.inst.FindAllObject(Target.transform.position, info.EnemyTeamIdx, 4f);

            for (int i = 0; i < dummy_Enemy.Count; i++)
            {
            DamageManager.inst.DamageFunc1(gameObject,dummy_Enemy[i],v,eDamageType.Spell_Magic);
                
            }
            EffectManager.inst.EffectCreate("Skill13_Effect",Target.transform.position,Quaternion.Euler(-90,0,0),2);

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
            float da = stat.Atk_Damage();
            info.pv.RPC(nameof(CreateBullet),RpcTarget.All,PlayerInfo.Inst.PlayerIdx,"Bullet_Ice",Target.GetComponent<PhotonView>().ViewID,CreatePos.position,Quaternion.identity,da);
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



        
    }
}
