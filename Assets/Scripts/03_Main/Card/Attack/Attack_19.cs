using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Attack_19 : AttackFunc
    {
        
        // ReSharper disable Unity.PerformanceAnalysis
        public LineRenderer line;
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
            GameObject bullet = PhotonNetwork.Instantiate("Bullet_Bullet", CreatePos.position, Quaternion.identity);
            if (bullet.TryGetComponent(out Buulet_Move1 move))
            {
                move.StartFUnc(gameObject,Target,da);
            }
        }

        public override void SkillFunc()
        {
            StartCoroutine(ISkillFunc());
        }

        IEnumerator ISkillFunc()
        {
            
            GameObject ob = GameSystem_AllInfo.inst.FindFirstObject(transform.position, info.EnemyTeamIdx, 26f, false);
            
            if(ob==null) yield break;
            Vector3 pos = ob.transform.position;
            stat.NetStopFunc(false,2f,false);
            stat.pv.RPC(nameof(RPC_SkillFunc), RpcTarget.All, gameObject.transform.position, ob.transform.position, true);
            
            
            yield return YieldInstructionCache.WaitForSeconds(1f);
            
            stat.pv.RPC(nameof(RPC_SkillFunc), RpcTarget.All, Vector3.zero, Vector3.zero, false);
            if (stat.IsDead) yield break;
            fsm.CoolStart(true);
            float da = SkillValue(1);
            GameObject bullet = PhotonNetwork.Instantiate("Bullet_Skill_19", CreatePos.position, Quaternion.identity);
            if (bullet.TryGetComponent(out Buulet_Move2 move))
            {
                move.StartFUnc(gameObject,pos,da,info.EnemyTeamIdx,true,false);
            }
        }

        [PunRPC]
        public void RPC_SkillFunc(Vector3 startpos,Vector3 endpos,bool b)
        {
            if (b)
            {
                //line.transform.parent = null;
                //line.transform.localScale = new Vector3(1,1,1);
                //line.transform.position=Vector3.zero;
                line.SetPosition(0,startpos);
                line.SetPosition(1,endpos);
                line.gameObject.SetActive(true);
            }
            else
            {
                //line.transform.parent = gameObject.transform;
                line.gameObject.SetActive(false);
                
            }
            
        }
    }
}
