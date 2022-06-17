using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameS
{
    public class Card_fight_State_NoCon : Card_FSM_FightState
    {
        private float CurTime = 0;

        public Card_fight_State_NoCon(Card_FSM_Fight _cardFsmFight): base(_cardFsmFight, eCardFight_STATE.NoCon)
        {
            
        }


        public override void Enter(FSMMsg _msg)
        {
            Fight.state = eCardFight_STATE.NoCon;
            CurTime = 0f;
            //base.Enter(_msg);

        }

        public override void Update()
        {
            CurTime += Time.deltaTime;
            if (CurTime>=Fight.noConTime)
            {
                CurTime = 0f;
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
            //base.Finally();
            //Fight.info.MoveIdx = 1;
        }

        public override void SetMsg(FSMMsg _msg)
        {
            
        }
    }
    
    
    
    
    
}
