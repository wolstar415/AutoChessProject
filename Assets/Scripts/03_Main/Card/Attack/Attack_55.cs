using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Attack_55 : AttackFunc
    {

        public GameObject Manabar;
        public bool skillOn = false;
        public Animator ani1;
        public Animator ani2;
        public GameObject model1;
        public GameObject model2;
        // ReSharper disable Unity.PerformanceAnalysis
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
            GameObject bullet = PhotonNetwork.Instantiate("Bullet_Bullet_2", CreatePos.position, Quaternion.identity);
            if (bullet.TryGetComponent(out Buulet_Move1 move))
            {
                move.StartFUnc(gameObject,Target,da);
            }
        }
        public override bool SkillCheck()
        {
            if (skillOn||stat.currentMana<stat.ManaMax()) // 잠시만 쓸꺼 
            {
                return false;
            }
            if (IsFastSkill)
            {
                SkillBasic();
                SkillFunc();
            }
            return IsFastSkill;
        }
        public override void SkillFunc()
        {
            if (skillOn) return;

            float v = SkillValue(1);
            float v2 = SkillValue(2);
            skillOn = true;
            stat.pv.RPC(nameof(RPC_SkillFunc),RpcTarget.All);
            
            stat.AtkPlus(0,0,v,true);
            stat.HpPlus(0,0,v2,true);
        }
        


        [PunRPC]
        public void RPC_SkillFunc()
        {
            skillOn = true;
            model1.SetActive(false);
            model2.SetActive(true);
            Manabar.SetActive(false);
            stat.ani = ani2;
            stat.BuffNomana++;
        }

        public override void BattelEnd()
        {
            model1.SetActive(true);
            model2.SetActive(false);
            Manabar.SetActive(true);
            skillOn = false;
            stat.ani = ani1;
            stat.BuffNomana=0;
        }

        
    }
}
