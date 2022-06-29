using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameS
{
    public class Card_fight_State_Attack : Card_FSM_FightState
    {
        private float coolTime = 0f;

        public Card_fight_State_Attack(Card_FSM_Fight _cardFsmFight): base(_cardFsmFight, eCardFight_STATE.Attack)
        {
            
        }


        public override void Enter(FSMMsg _msg)
        {
            //Debug.Log("어택");
            coolTime = 0;
            Fight.state = eCardFight_STATE.Attack;
            Fight.FindEnemy(); // 체크

            if (Fight.Enemy==null)
            {

                Fight.fsm.SetState(eCardFight_STATE.Idle);
                return;
            }
            //base.Enter(_msg);

        }

        public override void Update()
        {

            if (Fight.attackFunc.SkillCheck())
            {
                return;
            }
            //거리가멀거나 적이없거나 무적이거나 죽었을때
            if (Fight.Enemy==null||Fight.EnemyCheck() == false||Vector3.Distance(Fight.transform.position, Fight.Enemy.transform.position) > Fight.info.stat.Range()+1.2)
            {
                Fight.fsm.SetState(eCardFight_STATE.Idle);
                return;
            }
            Vector3 dir = Fight.Enemy.transform.position - Fight.transform.position;
            try
            {

            Fight.transform.rotation = Quaternion.Lerp(Fight.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10f);
            }
            catch (Exception e)
            {
                
            }


            if (Fight.IsCool) // 공격할수있을때 공격!
            {

                
                Fight.IsCool = false;
                Fight.attackFunc.BasicAttack(Fight.Enemy);
                
                
            }
            //base.Update();
        }

        public override void End()
        {
            //base.End();
        }

        public override void Finally()
        {
            //base.Finally();
            //Fight.info.MoveIdx = 1;
        }
    }
    
    
    
    
    
}
