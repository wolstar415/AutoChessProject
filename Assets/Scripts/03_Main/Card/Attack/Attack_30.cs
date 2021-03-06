using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Attack_30 : AttackFunc
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
            float da = stat.Atk_Damage();
            info.pv.RPC(nameof(CreateBullet),RpcTarget.All,PlayerInfo.Inst.PlayerIdx,"Bullet_Bubble",Target.GetComponent<PhotonView>().ViewID,CreatePos.position,Quaternion.identity,da);

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
            SkillBasic();

            GameObject ob = GameSystem_AllInfo.inst.FindRandomObject(transform.position, info.EnemyTeamIdx, 26f);
            if (ob == null) return;
            
            StartCoroutine(ISkillFunc(ob.transform.position));
        }

        IEnumerator ISkillFunc(Vector3 pos)
        {
            int cnt = 10;
            float d = SkillValue(1)*0.1f;
            EffectManager.inst.EffectCreate("Skill30_Effect",pos,Quaternion.identity,2f);
            while (cnt>0)
            {
                cnt--;
                dummy_Enemy.Clear();
                dummy_Enemy = GameSystem_AllInfo.inst.FindAllObject(pos, info.EnemyTeamIdx, 6.6f);
                
                for (var i = 0; i < dummy_Enemy.Count; i++)
                {
                    DamageManager.inst.DamageFunc1(gameObject,dummy_Enemy[i],d,eDamageType.Spell_Magic);
                }
                yield return YieldInstructionCache.WaitForSeconds(0.3f);
            }


        }
    }
}
