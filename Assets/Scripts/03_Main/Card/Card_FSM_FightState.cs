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
        Skill,
    }
    public class Card_FSM_FightState : FsmState<eCardFight_STATE>
    {
        protected Card_FSM_Fight cardFsmFight;
        public Card_FSM_FightState(Card_FSM_Fight _cardFsmFight,eCardFight_STATE _stateType) : base(_stateType)
        {
            cardFsmFight = _cardFsmFight;
        }
    }
}
