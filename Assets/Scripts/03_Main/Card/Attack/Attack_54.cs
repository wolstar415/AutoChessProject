using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Attack_54 : AttackFunc
    {
        

        // ReSharper disable Unity.PerformanceAnalysis
        public bool skillOn = false;
        public Animator ani1;
        public Animator ani2;
        public GameObject model1;
        public GameObject model2;
        public override void BasicAttack(GameObject target,float t=0.2f)
        {
            base.BasicAttack(target);
            StartCoroutine(IAttackFunc());
        }

        
        IEnumerator IAttackFunc()
        {
            float da = stat.Atk_Damage();
            yield return YieldInstructionCache.WaitForSeconds(0.2f);
            fsm.CoolStart();
            DamageManager.inst.DamageFunc1(gameObject,Target,da,eDamageType.Basic_phy);
        }

        public override void SkillFunc()
        {
            if (skillOn) return;

            float v = SkillValue(1);
            skillOn = true;
            stat.AniStart("Skill");
            stat.NetStopFunc(false,1f,false);
            stat.InvinSet(1f);
            StartCoroutine(ISkillFunc());
            stat.CoolPlus(0,0,v,true);
        }

        IEnumerator ISkillFunc()
        {
            yield return YieldInstructionCache.WaitForSeconds(1);
            stat.pv.RPC(nameof(RPC_SkillFunc),RpcTarget.All);
            
            
        }


        [PunRPC]
        public void RPC_SkillFunc()
        {
            model1.SetActive(false);
            model2.SetActive(true);
            stat.ani = ani2;
        }

        public override void BattelStart()
        {
            StartCoroutine(ShiledCheck());
        }

        IEnumerator ShiledCheck()
        {
            
            while (stat.shiled>0)
            {
                yield return null;
            }

            SkillFunc();
        }

        public override void BattelEnd()
        {
            StopAllCoroutines();
            model1.SetActive(true);
            model2.SetActive(false);
            skillOn = false;
            stat.ani = ani1;
        }
    }
}
