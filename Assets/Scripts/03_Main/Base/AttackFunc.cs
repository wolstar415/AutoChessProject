using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class AttackFunc : MonoBehaviourPunCallbacks
    {
        public Card_Info info;
        public UnitState stat;
        public Card_FSM_Fight fsm;
        

        public bool IsFastSkill; //스킬을 바로 쓰는건지

        [Header("ㄷㅇㅁ")]
        [SerializeField] protected Transform CreatePos;
        [SerializeField] protected GameObject bulletPrefab;
        [SerializeField] protected GameObject Target;
        public virtual void BasicAttack(GameObject target)
        {
            Target = target;
            

                stat.AniStart("Attack");
            
            //여기서 평타대신 스킬나간다면 처리여기서하기.
            
        }

        /// <summary>
        /// 즉발로 스킬쓰는지 체크
        /// </summary>
        /// <returns></returns>
        public bool SkillCheck()
        {
            if (stat.currentMana<stat.ManaMax()) // 잠시만 쓸꺼 
            {
                return false;
            }
            if (IsFastSkill)
            {
                SkillFunc();
            }
            return IsFastSkill;
        }

        public virtual void SkillFunc()
        {
            stat.currentMana = 0;
            int have44 = stat.info.IsItemHave(44);
            if (have44>0)
            {
                stat.MpHeal(have44*20);
            }
            
            
            Collider[] c = Physics.OverlapSphere(transform.position, 6f, GameSystem_AllInfo.inst.masks[info.EnemyTeamIdx]);


            for (var i = 0; i < c.Length; i++)
            {
                if (c[i].TryGetComponent(out Card_Info cc))
                {
                    if (cc.IsFiled&&!cc.stat.IsDead&&cc.IsItemHave(34)>0)
                    {
                        cc.stat.Item34Func(info.pv.ViewID);
                    }
                }
            }
        }


    }
}
