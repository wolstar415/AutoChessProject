using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameS
{
    public class Attack_47 : AttackFunc
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
            bool skill = manacheck();
            if (skill)
            {
                SkillBasic();
                float v=SkillValue(1);

                dummy_Enemy.Clear();
                dummy_Enemy = GameSystem_AllInfo.inst.FindAllObject(Target.transform.position, info.EnemyTeamIdx, 3.3f);
                for (int i = 0; i < dummy_Enemy.Count; i++)
                {
                    DamageManager.inst.DamageFunc1(gameObject,dummy_Enemy[i],v,eDamageType.Speel_Magic);
                    
                }
                    
                EffectManager.inst.EffectCreate("Skill47_Effect",Target.transform.position,Quaternion.identity,2f);
            }
            else
            {
                float da = stat.Atk_Damage();
                GameObject bullet = PhotonNetwork.Instantiate("Bullet_Bullet", CreatePos.position, Quaternion.identity);
                if (bullet.TryGetComponent(out Buulet_Move1 move))
                {
                    move.StartFUnc(gameObject,Target,da);
                }
                
            }
            
            

        }



        
    }
}
