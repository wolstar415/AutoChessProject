using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Attack_7 : AttackFunc
    {

        public override void BasicAttack(GameObject target,float t=0.2f)
        {
            
            
            base.BasicAttack(target);
            StartCoroutine(IAttackFunc());
            
        }

        public override void SkillFunc()
        {
            stat.NetStopFunc(false,0.3f,false);
            stat.AniStart("Skill");
            StartCoroutine(ISkillFunc());
            //SkillBasic();
        }

        IEnumerator ISkillFunc()
        {
            
            yield return YieldInstructionCache.WaitForSeconds(0.3f);
            GameObject ob = GameSystem_AllInfo.inst.FindFirstObject(transform.position, info.EnemyTeamIdx, 26f, false);
            
            if(ob==null) yield break;
            float v=SkillValue(1);
            float v2=SkillValue(2);
            if (ob.TryGetComponent(out CardState enemystat))
            {
                enemystat.NetStopFunc(true,v2,true);
            }
            DamageManager.inst.DamageFunc1(gameObject,ob,v,eDamageType.Speel_Magic);
            EffectManager.inst.EffectCreate("Skill7_Effect",ob.transform.position,Quaternion.identity,2);

        }

        IEnumerator IAttackFunc()
        {
            //float f = stat.AtkAniTime();
            fsm.CoolStart();
            yield return YieldInstructionCache.WaitForSeconds(0.1f);
            AttackFunc();
            
        }
        public 

        void AttackFunc()
        {
            //총알생성
            float da = stat.Atk_Damage();

            info.pv.RPC(nameof(CreateBullet),RpcTarget.All,PlayerInfo.Inst.PlayerIdx,"Bullet_Fire",Target.GetComponent<PhotonView>().ViewID,CreatePos.position,Quaternion.identity,da);

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
