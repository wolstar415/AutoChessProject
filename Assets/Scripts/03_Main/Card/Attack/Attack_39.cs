using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Attack_39 : AttackFunc
    {
        private Coroutine corskill = null;
        public GameObject skillob;
        float angleRange=50f;
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
            int cnt = 15;
            float d = SkillValue(1)/15;
            while (cnt>0)
            {
                cnt--;

                GameObject ob = GameSystem_AllInfo.inst.FindFirstObject(transform.position, info.EnemyTeamIdx, 26f,true);
                if (ob==null)
                {
                    break;
                }
                transform.LookAt(ob.transform);
                dummy_Enemy.Clear();
                dummy_Enemy = GameSystem_AllInfo.inst.FindAllObject(transform.position, info.EnemyTeamIdx, 14f);
                float dotValue = Mathf.Cos(Mathf.Deg2Rad * ((angleRange) / 2));
                
                
                for (var i = 0; i < dummy_Enemy.Count; i++)
                {
                    Vector3 direction = dummy_Enemy[i].transform.position - transform.position;
                    direction.Normalize();
                    if (Vector3.Dot(direction,transform.forward)>dotValue)
                    {
                    
                        DamageManager.inst.DamageFunc1(gameObject,dummy_Enemy[i],d,eDamageType.Speel_Magic);

                    }
                }
              
                
                yield return YieldInstructionCache.WaitForSeconds(0.2f);
            }
            info.pv.RPC(nameof(SkillActive),RpcTarget.All,false);
            corskill = null;
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
