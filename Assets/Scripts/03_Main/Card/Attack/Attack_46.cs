using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class Attack_46 : AttackFunc
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
            
            
            bool skill = manacheck();

            if (skill)
            {
                SkillBasic();
                
                int cnt = 5;
                stat.NetStopFunc(false,0.5f,false);
            
                while (cnt>0)
                {
                    cnt--;
                    if(stat.IsDead) yield break;
                    if (Target==null||Target.GetComponent<CardState>().IsDead)
                    {
                        Target=
                            GameSystem_AllInfo.inst.FindFirstObject(transform.position, info.EnemyTeamIdx, stat.Range(), true);

                    }
                    if (Target == null) break;
                    float da = CsvManager.inst.skillinfo[info.Idx].Realcheck(1, info.Level);
                    GameObject bullet = PhotonNetwork.Instantiate("Bullet_Laser2", CreatePos.position, Quaternion.identity);
                    if (bullet.TryGetComponent(out Buulet_Move1 move))
                    {
                        move.StartFUnc(gameObject,Target,da,false,1);
                    }

                
                
                    yield return YieldInstructionCache.WaitForSeconds(0.1f);
                }
                
                
            }
            else
            {
                float da = stat.Atk_Damage();
                GameObject bullet = PhotonNetwork.Instantiate("Bullet_Laser1", CreatePos.position, Quaternion.identity);
                if (bullet.TryGetComponent(out Buulet_Move1 move))
                {
                    move.StartFUnc(gameObject,Target,da);
                }
            }
            
        }





        
    }
}
