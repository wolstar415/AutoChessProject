using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameS
{
    public class Card_fight_State_NoCon : Card_FSM_FightState
    {
        private float CurTime = 0;
        public int Check = 0;

        public Card_fight_State_NoCon(Card_FSM_Fight _cardFsmFight): base(_cardFsmFight, eCardFight_STATE.NoCon)
        {
            
        }


        public override void Enter(FSMMsg _msg)
        {
            Fight.state = eCardFight_STATE.NoCon;
            CurTime = 0f;
            if (_msg.m_msgType>0)
            {
                Check += _msg.m_msgType;
            }
            //base.Enter(_msg);

        }

        public override void Update()
        {
            if (Check>0) return;
            CurTime += Time.deltaTime;
            if (CurTime>=Fight.noConTime)
            {
                CurTime = 0f;
                Fight.noConTime = 0;
                Fight.fsm.SetState(eCardFight_STATE.Idle);
                if (Fight.info.stat.IsStun)
                {
                    Fight.info.stat.StunShow(false);
                }
                return;
            }
        }

        public override void End()
        {
            //base.End();
        }

        public override void Finally()
        {
            CurTime = 0f;
            Check = 0;
            //base.Finally();
            //Fight.info.MoveIdx = 1;

        }


    }
    
    
    
    
    
}
