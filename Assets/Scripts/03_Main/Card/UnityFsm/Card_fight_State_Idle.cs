using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameS
{
    public class Card_fight_State_Idle : Card_FSM_FightState
    {
        private float CurTime = 0;

        public Card_fight_State_Idle(Card_FSM_Fight _cardFsmFight): base(_cardFsmFight, eCardFight_STATE.Idle)
        {
        }


        public override void Enter(FSMMsg _msg)
        {
            Fight.state = eCardFight_STATE.Idle;
            CurTime = 0f;
            NextFind();
        }

        void NextFind()
        {
            Fight.Enemy=Fight.FindEnemy();
            if (Fight.Enemy==null)
            return;

            if (Vector3.Distance(Fight.transform.position,Fight.Enemy.transform.position)<=Fight.info.stat.Range())
            {
                Fight.fsm.SetState(eCardFight_STATE.Attack);
            }
            else
            {
                Fight.fsm.SetState(eCardFight_STATE.Moving);
            }
            

        }

        public override void Update()
        {
            if (Fight.Enemy!=null)
            return;
            
            CurTime += Time.deltaTime;
            if (CurTime>=0.5f)
            {
                CurTime = 0f;
                NextFind();
            }
            //base.Update();
        }

        public override void End()
        {
            //base.End();
        }

        public override void Finally()
        {
            CurTime = 0f;
            //base.Finally();
            //Fight.info.MoveIdx = 1;
        }

        public override void SetMsg(FSMMsg _msg)
        {
            if (_msg.m_msgType == 2) // 적들이 새로 들어왔으니 바로 검색!
            {
                NextFind();
            }
        }
    }
    
    
    
    
    
}
