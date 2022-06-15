using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameS
{
    public class Card_fight_State_Dead : Card_FSM_FightState
    {

        public Card_fight_State_Dead(Card_FSM_Fight _cardFsmFight): base(_cardFsmFight, eCardFight_STATE.Dead)
        {
        }


        public override void Enter(FSMMsg _msg)
        {
            //base.Enter(_msg);
            
        }

        public override void Update()
        {
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
