using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameS
{
    public enum eCardFight_STATE
    {
        None,
        Idle,
        Moving,
        Attack,
        Dead,
        NoCon,
    }
    public class Card_FSM_FightState : FsmState<eCardFight_STATE>
    {
        protected Card_FSM_Fight Fight;
        public Card_FSM_FightState(Card_FSM_Fight _cardFsmFight,eCardFight_STATE _stateType) : base(_stateType)
        {
            Fight = _cardFsmFight;
        }

        public override void Enter(FSMMsg _msg)
        {
            if (!Fight.info.IsFiled)
                return; // 혹시나하는 버그방지용
            
        }
    }
}
