using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public enum Fight_FSM { 
None=0,
idle,
Moving,
Attacking
}


public class Card_Info : MonoBehaviour
{
    [SerializeField] bool IsRangeAttack;
    public bool IsFighting = false;
    [SerializeField] Fight_FSM fightFSM;
    public int TeamIdx = 0;
    public int EnemyTeamIdx = 0;
    public bool IsAttacker=false;
    public GameObject Enemy=null;
    public bool IsDead = false;
    public float Hp = 0;
    [SerializeField] NavMeshAgent nav;
    public float Range = 2f;
    public bool IsCool=false;
    
    // Start is called before the first frame update
    void Start()
    {
        fightFSM = Fight_FSM.None;

        fightFSM= Fight_FSM.idle;
        IsFighting = true;
        IsCool = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsFighting)
        {
            FightFSM();
        }

    }

    void FightFSM()
    {
        switch (fightFSM)
        {
            case Fight_FSM.None: break;
            case Fight_FSM.idle:
                if (Enemy == null)
                {
                    EnemyFind(); // 적을 찾는 함수
                }

                
                if (Enemy !=null)
                {
                    if (Enemy.TryGetComponent(out Card_Info com))
                    {
                        if (com.IsDead == true)
                        {
                            Enemy = null;
                            EnemyFind();
                        }
                        else
                        {
                            fightFSM = Fight_FSM.Moving;
                            nav.isStopped = false;
                            
                        }
                    }
                    
                }
                break;
            case Fight_FSM.Moving:
                if (Vector3.Distance(transform.position, Enemy.transform.position) <= Range)
                {
                    nav.isStopped = true;
                    fightFSM = Fight_FSM.Attacking;
                }
                else
                {

                nav.SetDestination(Enemy.transform.position);
                }

                break;
            case Fight_FSM.Attacking:

                if (Enemy.TryGetComponent(out Card_Info com2))
                {
                    if (com2.IsDead == true)
                    {
                        Enemy = null;
                        fightFSM = Fight_FSM.idle;
                        break;
                    }
                }
                Vector3 dir = Enemy.transform.position - this.transform.position;
                transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10f);
                if (Vector3.Distance(transform.position,Enemy.transform.position)> Range+0.1f)
                {
                    fightFSM = Fight_FSM.idle;
                    break;
                }
                if (IsCool)
                {
                    IsCool = false;
                    StartCoroutine(CoolCheck());
                    Debug.Log("공격!");
                }
                break;
            default:
                break;
        }
    }

    void EnemyFind()
    {
        Collider[] c = Physics.OverlapSphere(transform.position, 30f, GameSystem_AllInfo.inst.masks[EnemyTeamIdx]);
        // 오브젝트 주위 레이어로 검색된 주위 오브젝트 뽑아냅니다.
        if (c.Length >0)
        {
        Enemy= GameSystem_AllInfo.inst.FindNearestObject(transform.position,c); 
            //가장 가까운적 뽑아내는 함수

        }
    }

    IEnumerator CoolCheck()
    {
        var a = new WaitForSeconds(2);
        yield return a;
        IsCool = true;
    }
}
