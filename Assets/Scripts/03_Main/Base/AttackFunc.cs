using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameS
{
    public class AttackFunc : MonoBehaviour
    {
        public Card_Info info;
        public UnitState stat;
        public Card_FSM_Fight fsm;

        public bool IsFastSkill; //스킬을 바로 쓰는건지

        public virtual void BasicAttack()
        {
            //여기서 평타대신 스킬나간다면 처리여기서하기.
            
        }

        /// <summary>
        /// 즉발로 스킬쓰는지 체크
        /// </summary>
        /// <returns></returns>
        public bool SkillCheck()
        {
            if (stat.Mp<100) // 잠시만 쓸꺼 
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
            stat.Mp = 0;
        }
    }
}
