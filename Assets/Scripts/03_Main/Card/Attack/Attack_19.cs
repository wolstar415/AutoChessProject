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
            var obstat = ob.GetComponent<CardState>();
            Vector3 pos;
            stat.NetStopFunc(false,2f,false);
            stat.pv.RPC(nameof(RPC_Active), RpcTarget.All, true);
            
            
            float cultime = 0;
            while (cultime<=2f)
            {
                cultime += Time.deltaTime;
                if (obstat.IsDead)
                {
                    stat.NetStopFunc(false,0.1f,false);
                    stat.pv.RPC(nameof(RPC_Active), RpcTarget.All, false);
                    fsm.CoolStart(true);
                    yield break;
                }
            stat.pv.RPC(nameof(LineMove), RpcTarget.All,transform.position,ob.transform.position);
                
                yield return null;
            }
            stat.pv.RPC(nameof(RPC_Active), RpcTarget.All, false);
            if (stat.IsDead) yield break;
            fsm.CoolStart(true);
            float da = SkillValue(1);
            GameObject bullet = PhotonNetwork.Instantiate("Bullet_Skill_19", CreatePos.position, Quaternion.identity);
            if (bullet.TryGetComponent(out Buulet_Move2 move))
            {
                move.StartFUnc(gameObject,ob.transform.position,da,info.EnemyTeamIdx,true,false);
            }
        }

        [PunRPC]
        public void RPC_Active(bool b)
        {
            line.gameObject.SetActive(b);
            
        }

        [PunRPC]
        void LineMove(Vector3 startpos, Vector3 endpos)
        {
            line.SetPosition(0,startpos);
            line.SetPosition(1,endpos);
        }

        public override void BattelEnd()
        {
            if (line.gameObject.activeSelf)
            {
                stat.pv.RPC(nameof(RPC_Active),RpcTarget.All,false);
            }
        }
    }
}
