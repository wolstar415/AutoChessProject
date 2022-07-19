using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Attack_43 : AttackFunc
    {
        private Coroutine corskill = null;
        private Coroutine corskill2 = null;
        public GameObject skillob;
        public LineRenderer laser1;
        public LineRenderer laser2;
        public LineRenderer laser3;
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

            info.pv.RPC(nameof(CreateBullet),RpcTarget.All,PlayerInfo.Inst.PlayerIdx,"Bullet_Po",Target.GetComponent<PhotonView>().ViewID,CreatePos.position,Quaternion.identity,da);

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
            if (corskill!=null)
            {
                StopCoroutine(corskill);
            }
            corskill = StartCoroutine(ISkillFunc());
        }

        IEnumerator ISkillFunc()
        {
            stat.NetStopFunc(false,3,false);
            info.pv.RPC(nameof(SkillActive),RpcTarget.All,true);
            Target = GameSystem_AllInfo.inst.FindFirstObject(transform.position, info.EnemyTeamIdx, 26f,true);
            int cnt = 15;
            float d = SkillValue(1)/15;
            if (corskill2!=null)
            {
                StopCoroutine(corskill2);
            }
            corskill2 = StartCoroutine(Skillmove());
            while (cnt>0)
            {
                cnt--;
                
                
                var hits= Physics.SphereCastAll(transform.position, 1.5f, transform.forward, 20f,GameSystem_AllInfo.inst.masks[info.EnemyTeamIdx]);

    
                


                for (var i = 0; i < hits.Length; i++)
                {
                    if (hits[i].transform.TryGetComponent(out CardState enemystat))
                    {
                        if (!enemystat.IsDead&&enemystat.info.IsFiled&&enemystat.IsInvin==0)
                        {
                            
                        DamageManager.inst.DamageFunc1(gameObject,hits[i].transform.gameObject,d,eDamageType.Spell_Magic);
                        }
                    }
                    

                    
                }
              
                
                yield return YieldInstructionCache.WaitForSeconds(0.2f);
            }
            info.pv.RPC(nameof(SkillActive),RpcTarget.All,false);
            corskill = null;
            StopCoroutine(corskill2);
            corskill2 = null;
        }

        IEnumerator Skillmove()
        {
            while (!PlayerInfo.Inst.BattleEnd)
            {
                
                if (Target==null||Target.GetComponent<CardState>().IsDead)
                {
                    Target = GameSystem_AllInfo.inst.FindFirstObject(transform.position, info.EnemyTeamIdx, 26f,true);
                }

                if (Target!=null)
                {
                    
                Vector3 dir = Target.transform.position - this.transform.position;
                dir.y = 0;
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10);
                }

                Vector3 endpos = transform.position;
                Vector3 startpos = transform.position+(transform.forward * 20);
                stat.pv.RPC(nameof(RPC_LaserMove),RpcTarget.All,startpos,endpos);
                yield return null;
            }
        }

        [PunRPC]
        void RPC_LaserMove(Vector3 startpos, Vector3 endpos)
        {
            laser1.SetPosition(0,startpos);
            laser1.SetPosition(1,endpos);
            laser2.SetPosition(0,startpos);
            laser2.SetPosition(1,endpos);
            laser3.SetPosition(0,startpos);
            laser3.SetPosition(1,endpos);
        }

        [PunRPC]
        void SkillActive(bool b)
        {
            skillob.SetActive(b);
        }

        public override void BattelEnd()
        {
            if (skillob.activeSelf)
            {
                info.pv.RPC(nameof(SkillActive),RpcTarget.All,false);
            }
        }


        
    }
}
