using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameS
{
    public class Attack_10 : AttackFunc
    {
        
        public override void BasicAttack(GameObject target,float t=0.2f)
        {
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
            float t = SkillValue(2);
            float v = SkillValue(1);
            stat.NetStopFunc(false,0.4f,false);
            StartCoroutine(ISkillFunc(v, t));
            EffectManager.inst.EffectCreate("Skill5_Effect",transform.position,Quaternion.identity,2f);

        }
        

        IEnumerator ISkillFunc(float v,float t)
        {
            yield return YieldInstructionCache.WaitForSeconds(0.4f);
            stat.CoolPlus(0,0,v,true);
            yield return YieldInstructionCache.WaitForSeconds(t);
            stat.CoolPlus(0,0,-v,true);
        }
    }
}
