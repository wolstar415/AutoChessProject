using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Attack_27 : AttackFunc
    {
        private Coroutine corskill = null;
        public GameObject skillob;
        public override void BasicAttack(GameObject target,float t=0.3f)
        {

            base.BasicAttack(target,t);


            StartCoroutine(IAttackFunc());
        }

        
        IEnumerator IAttackFunc()
        {
            float da = stat.Atk_Damage();
            stat.AniStart("Attack");
            yield return YieldInstructionCache.WaitForSeconds(0.3f);
            fsm.CoolStart();

            
            DamageManager.inst.DamageFunc1(gameObject,Target,da,eDamageType.Basic_phy);
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
            info.pv.RPC(nameof(SkillActive),RpcTarget.All,true);
            int cnt = 20;
            float d = SkillValue(1)*0.05f;
            while (cnt>0)
            {
                cnt--;

                
                
                dummy_Enemy.Clear();
                dummy_Enemy = GameSystem_AllInfo.inst.FindAllObject(transform.position, info.EnemyTeamIdx, 6.6f);
                
                for (var i = 0; i < dummy_Enemy.Count; i++)
                {
                    DamageManager.inst.DamageFunc1(gameObject,dummy_Enemy[i],d,eDamageType.Speel_Magic);
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
