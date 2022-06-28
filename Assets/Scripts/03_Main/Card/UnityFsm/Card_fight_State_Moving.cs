using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameS
{
    public class Card_fight_State_Moving : Card_FSM_FightState
    {
        private bool IsMoving = false;
        private static readonly int Run = Animator.StringToHash("Run");

        public float culTime;
        public Card_fight_State_Moving(Card_FSM_Fight _cardFsmFight): base(_cardFsmFight, eCardFight_STATE.Moving)
        {
        }


        public override void Enter(FSMMsg _msg)
        {
            culTime = 0;
            Fight.state = eCardFight_STATE.Moving;
            //Fight.Enemy = null;
            Fight.FindEnemy(); // 혹시모르니까 버그체크
            if (Fight.Enemy==null)
            {
                Fight.fsm.SetState(eCardFight_STATE.Idle);
                Fight.info.stat.ani.Play("Idle");
                return;
            }
            
            MoveFunc();


        }

        void Checking()
        {
            Fight.Enemy = null; // 새로들어온적이 가까울수도있으니 한번 체크. 이동은 가장 가까운애한테가도록
            Fight.FindEnemy();
            if (Fight.Enemy==null)
            {
                Fight.fsm.SetState(eCardFight_STATE.Idle);
                Fight.info.stat.ani.Play("Idle");
                return;
            }
            MoveFunc();
        }

        void MoveFunc()
        {
            IsMoving = true;
            Fight.nav.isStopped = false;
            if (Fight.info.IsAni)
            {
                Fight.info.stat.gani.Play("Run");
            }
            else
            {
                Fight.info.stat.ani.SetBool(Run,true);
            }
        }

        public override void Update()
        {
            if (!IsMoving) return;


            if (Fight.attackFunc.SkillCheck())
            {
                return;
            }


            if (Fight.EnemyCheck() == false) //죽었거나 무적일경우 제거
                Fight.Enemy = null;
            
            culTime += Time.deltaTime;
            if (culTime>=0.5f)
            {
                culTime = 0;
                Fight.Enemy = null;
                Fight.FindEnemy();
            }
                if (Fight.Enemy == null)
            {
                Fight.fsm.SetState(eCardFight_STATE.Idle);
                Fight.info.stat.ani.Play("Idle");
                return;
            }
            //이동
            Fight.nav.SetDestination(Fight.Enemy.transform.position); 
            
            //거리체크

            if (Vector3.Distance(Fight.transform.position, Fight.Enemy.transform.position) <= Fight.info.stat.Range()+1f)
            {
//             Debug.Log($"거리 :{Vector3.Distance(Fight.transform.position, Fight.Enemy.transform.position)}\n 사정거리:{Fight.info.stat.Range()}");
                Fight.nav.isStopped = true;
                Fight.fsm.SetState(eCardFight_STATE.Attack);
            }
            //base.Update();
        }

        public override void End()
        {
            //base.End();
        }

        public override void Finally()
        {
            
                Fight.info.stat.ani.SetBool(Run,false);
            
            //base.Finally();
            //Fight.info.MoveIdx = 1;
            IsMoving = false;
            Fight.nav.isStopped = true;
        }
        public override void SetMsg(FSMMsg _msg)
        {
            
            if (_msg.m_msgType == 2) // 적들이 새로 들어왔으니 바로 검색!
            {
                Checking();
            }
        }
    }
    
    
    
    
    
}
