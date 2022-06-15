using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameS
{
    public class Card_fight_State_None : Card_FSM_FightState
    {

        public Card_fight_State_None(Card_FSM_Fight _cardFsmFight): base(_cardFsmFight, eCardFight_STATE.None)
        {
        }


        public override void Enter(FSMMsg _msg)
        {
            base.Enter(_msg);
            
            Fight.stop = true;
            Fight.state = eCardFight_STATE.None;
            if (_msg!=null)
                return; //맨처음일때는 밑에 못하게
            
            
            //다 돌아오게하기. 위치 등등등.

        }
        
        public override void End()
        {
            //공격상태로 변경하기
            //
        }

    }
    
    
    
    
    
}
