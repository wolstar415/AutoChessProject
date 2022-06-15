using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameS
{
    public class Card_FSM_Fight : MonoBehaviour
    {
        public FsmClass<eCardFight_STATE> fsm = new FsmClass<eCardFight_STATE>();

        public Card_Info info;
        // Start is called before the first frame update
        void Start()
        {
            fsm.AddFsm(new Card_fight_State1(this));
            fsm.Stop();
        }

        public void BattleStart()
        {
            fsm.SetState(eCardFight_STATE.Idle);
        }

        // Update is called once per frame
        void Update()
        {
            fsm.Update();
        }
    }
}
