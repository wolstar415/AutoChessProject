using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;
namespace GameS
{
    public class Card_FSM_Fight : MonoBehaviourPunCallbacks
    {
        public FsmClass<eCardFight_STATE> fsm = new FsmClass<eCardFight_STATE>();
        public PhotonView pv;
    public GameObject Enemy;
    public NavMeshAgent nav;
    public AttackFunc attackFunc;
    
    [Header("적")]
    public List<GameObject> Enemies;

    public List<GameObject> Enmies2=new List<GameObject>();
    public bool IsCool=false;
    private Coroutine CoCool=null;


     public eCardFight_STATE state; // 인스펙터 확인용
     [Header("기본")] 
     public float noConTime = 0;
    public bool stop = false;
        public Card_Info info;
        private FSMMsg noconmsg = new FSMMsg(0);

     
        // Start is called before the first frame update
        // void Start()
        // {
        //     fsm.AddFsm(new Card_fight_State_None(this));
        //     fsm.AddFsm(new Card_fight_State_Idle(this));
        //     fsm.AddFsm(new Card_fight_State_Moving(this));
        //     fsm.AddFsm(new Card_fight_State_Attack(this));
        //     fsm.AddFsm(new Card_fight_State_Dead(this));
        //     fsm.AddFsm(new Card_fight_State_NoCon(this));
        //
        //     fsm.SetState(eCardFight_STATE.None,false,new FSMMsg(1));
        //     
        // }

        private void Awake()
        {
            fsm.AddFsm(new Card_fight_State_None(this));
            fsm.AddFsm(new Card_fight_State_Idle(this));
            fsm.AddFsm(new Card_fight_State_Moving(this));
            fsm.AddFsm(new Card_fight_State_Attack(this));
            fsm.AddFsm(new Card_fight_State_Dead(this));
            fsm.AddFsm(new Card_fight_State_NoCon(this));

            fsm.SetState(eCardFight_STATE.None,false,new FSMMsg(1));
        }


        public void BattleStart()
        {
            fsm.SetState(eCardFight_STATE.Idle);
            IsCool = true;
            stop = false;
            
            // 
        }

        // Update is called once per frame
        void Update()
        {
            if (!info.pv.IsMine||stop)
            {
                return;
            }
            fsm.Update();
        }
        
        
        
        public void Enemysort()
        {
            if (Enmies2.Count == 0) return;

            Enmies2.OrderBy(obj =>
            {
                return Vector3.Distance(transform.position, obj.transform.position);
            });
        }


        public List<GameObject> EnemyReal()
        {

            List<GameObject> result = Enemies.
                Where(ob => ob.GetComponent<UnitState>().IsDead == false &&ob.GetComponent<Card_Info>().IsFiled&&
                                                          ob.GetComponent<UnitState>().IsInvin == 0).
                OrderBy(obj =>
            {
                return Vector3.Distance(transform.position, obj.transform.position);
            }).ToList();

            
            return result;
        }
        public GameObject FindEnemy()
        {



            if (EnemyCheck() == false)
                return Enemy = null;


            

            if (Enemy != null)
            {
                
                if (Enemy.activeSelf==true)
                {
                return Enemy;
                    
                }
                return null;
            
            }
            Enmies2.Clear();
            if (Enemies.Count>0)
            {
                
            Enmies2 = EnemyReal();
            }

            if (Enmies2.Count==0)
                Enemy = GameSystem_AllInfo.inst.FindFirstObject(transform.position,info.EnemyTeamIdx,26f,true);



            if (Enemy!=null)
            {
                return Enemy;
            }
            else if (Enmies2.Count>0)
            {
                return Enemy = Enmies2[0];
            }


            return null;
        }



        public void EnemyEnter()
        {
            fsm.SetMsg(new FSMMsg(2));
        }

        public bool EnemyCheck()
        {

            if (Enemy==null)
            {
                return true;
            }
            if (Enemy.TryGetComponent(out UnitState state))
            {
                if (state.IsDead||state.IsInvin>0||state.info.IsFiled==false)
                {
                    Enemy = null;
                    return false;
                }
            }

            return true;
        }


        public void CoolStart(bool Re=false)
        {
            if(CoCool!=null) StopCoroutine(CoCool);
            if (Re)
            {
                CoCool=StartCoroutine(CoolFunc(0.2f) );
                return;
            }
            CoCool=StartCoroutine(CoolFunc(info.stat.Atk_Cool()) );
        }
        IEnumerator CoolFunc(float f)
        {
            yield return YieldInstructionCache.WaitForSeconds(f);
            IsCool = true;
            CoCool = null;

        }
        
        public void NoConTime(float Time,bool b,int msg=0)
        {
            if (b)
            {
            noConTime += Time;
                
            }
            else
            {
                noConTime = Time;

            }

            noconmsg.m_msgType = msg;
            fsm.SetState(eCardFight_STATE.NoCon,false,noconmsg);
        }

    }
}
