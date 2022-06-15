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
    public bool IsCool=false;


     public eCardFight_STATE state; // 인스펙터 확인용
    [Header("기본")] 
    public bool stop = false;
        public Card_Info info;

     
        // Start is called before the first frame update
        void Start()
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
            if (Enemies.Count == 0) return;

            Enemies.OrderBy(obj =>
            {
                return Vector3.Distance(transform.position, obj.transform.position);
            });
        }


        public List<GameObject> EnemyReal()
        {
            List<GameObject> result = Enemies.Where(ob => ob.GetComponent<UnitState>().IsDead == false &&
                                                          ob.GetComponent<UnitState>().IsInvin == false).ToList();

            
            return result;
        }
        public GameObject FindEnemy()
        {

            if (EnemyCheck() == false)
                return Enemy = null;

            if (Enemy!=null)
                return Enemy;
            
            
            if (EnemyReal().Count==0)
                return Enemy = FindNearestObject();

            
            Enemysort();
            if (Enemy==null)
            {
                return Enemy = Enemies[0];
            }


            return null;
        }

        public GameObject FindNearestObject()
        {
            Collider[] c=null;
            Physics.OverlapSphereNonAlloc(transform.position, 30f, c,
                GameSystem_AllInfo.inst.masks[info.EnemyTeamIdx]);
            c.Where(ob => ob.GetComponent<UnitState>().IsDead == false &&
                          ob.GetComponent<UnitState>().IsInvin == false).OrderBy(ob => Vector3.Distance(transform.position, ob.transform.position));
            
            if (c.Length >0)
            {
                Enemy = c[0].gameObject;
                return Enemy;
            }

            return null;
        }
        public GameObject FindFarObject()
        {
            Collider[] c=null;
            Physics.OverlapSphereNonAlloc(transform.position, 30f, c,
                GameSystem_AllInfo.inst.masks[info.EnemyTeamIdx]);
            c.Where(ob => ob.GetComponent<UnitState>().IsDead == false &&
                          ob.GetComponent<UnitState>().IsInvin == false).OrderByDescending(ob => Vector3.Distance(transform.position, ob.transform.position));
            
            if (c.Length >0)
            {
                Enemy = c[0].gameObject;
                return Enemy;
            }

            return null;
        }

        public void EnemyEnter()
        {
            fsm.SetMsg(new FSMMsg(2));
        }

        public bool EnemyCheck()
        {
            if (Enemy == null) return false;

            if (Enemy.TryGetComponent(out UnitState state))
            {
                if (state.IsDead||state.IsInvin)
                {
                    Enemy = null;
                    return false;
                }
            }

            return true;
        }

        public void CoolStart()
        {
            StartCoroutine(CoolFunc(info.Atk_Cool) );
        }
        IEnumerator CoolFunc(float f)
        {
            yield return YieldInstructionCache.WaitForSeconds(f);
            IsCool = true;

        }

    }
}
