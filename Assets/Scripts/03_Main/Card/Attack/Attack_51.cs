using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Attack_51 : AttackFunc
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
            bool skill = manacheck();

            if (skill)
            {
                SkillBasic();
                
                float da = SkillValue(1)*0.1f;
                //
                info.pv.RPC(nameof(skillcheck1),RpcTarget.All,PlayerInfo.Inst.PlayerIdx,"Bullet_Skill_51",Target.GetComponent<PhotonView>().ViewID,CreatePos.position,Quaternion.identity,da);

            }
            else
            {
                float da = stat.Atk_Damage();
                info.pv.RPC(nameof(CreateBullet),RpcTarget.All,PlayerInfo.Inst.PlayerIdx,"Bullet_Dark",Target.GetComponent<PhotonView>().ViewID,CreatePos.position,Quaternion.identity,da);

            }
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
        IEnumerator SkillFunc(int pidx,string name,GameObject ob,Vector3 pos,Quaternion qu,float da)
        {
            var check = ob.GetComponent<Card_Info>().stat;
            int cnt = 10;
            while (cnt > 0)
            {
                if (check.IsDead)
                {
                    break;
                }
                cnt--;
                GameObject bullet = ObjectPooler.SpawnFromPool(name, pos, qu);
                if (bullet.TryGetComponent(out Buulet_Skill51_Move move))
                {
                    move.StartFUnc(pidx,gameObject,ob,da);
                }
                
                
                yield return YieldInstructionCache.WaitForSeconds(0.1f);
            }
        }

        [PunRPC]
        void skillcheck1(int pidx,string name,int id,Vector3 pos,Quaternion qu,float da)
        {
            StartCoroutine(SkillFunc(pidx,name,PhotonView.Find(id).gameObject,pos,qu,da));
        }



        
    }
}
