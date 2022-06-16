using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

public class Card_fight : MonoBehaviour
{
    /*
    [SerializeField] private PhotonView pv;
    [SerializeField]private Card_Info info;
    [SerializeField] Fight_FSM fightFSM;
    [SerializeField] GameObject EnemyMoving;
    public GameObject Enemy;
    [SerializeField] NavMeshAgent nav;
    [Header("적")]
    public List<GameObject> Enemies;
    [SerializeField] bool IsCool=false;
    private void Start()
    {
        IsCool = true;
        fightFSM = Fight_FSM.None;
        
        
        
        
        
    }

    void Update()
    {
        if (!pv.IsMine)
        {
            return;
        }
        if (info.IsFighting)
        {
            FightFSM();
        }
    }
    
    void FightFSM()
    {
        switch (fightFSM)
        {
                return;
            case Fight_FSM.None: break;
            case Fight_FSM.Idle:
                


                if (EnemyMoving !=null)
                {
                    if (EnemyMoving.TryGetComponent(out Card_Info com))
                    {
                        if (com.IsDead == true)
                        {
                            EnemyMoving = null;
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
                if (EnemyMoving==null)
                {
                    fightFSM = Fight_FSM.Idle;
                    break;
                }
                if (Vector3.Distance(transform.position, EnemyMoving.transform.position) <= info.stat.Range)
                {
                    nav.isStopped = true;
                    fightFSM = Fight_FSM.Attacking;
                }
                else
                {

                    nav.SetDestination(EnemyMoving.transform.position);
                }

                break;
            case Fight_FSM.Attacking:

                if (Enemy==null)
                {
                    fightFSM = Fight_FSM.Idle;
                    break;
                }
                if (Enemy.TryGetComponent(out Card_Info com2))
                {
                    if (com2.IsDead == true)
                    {
                        Enemy = null;
                        fightFSM = Fight_FSM.Idle;
                        break;
                    }
                }
                Vector3 dir = Enemy.transform.position - this.transform.position;
                transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10f);
                if (Vector3.Distance(transform.position,Enemy.transform.position)> info.stat.Range+0.1f)
                {
                    fightFSM = Fight_FSM.Idle;
                    break;
                }
                if (IsCool)
                {
                    IsCool = false;
                    StartCoroutine(CoolCheck());
                    Debug.Log("공격중!");
                }
                break;
            default:
                break;
        }
    }
    
    void Enemysort()
    {

        Enemies.OrderBy(obj =>
        {
            return Vector3.Distance(transform.position, obj.transform.position);
        });



    }

    public void EnemyFindFunc()
    {
        if (Enemy==null)
        {
            Enemysort();
            Enemy = Enemies[0];
            fightFSM = Fight_FSM.Attacking;
        }
    }
    
    void EnemyFind()
    {
        if (Enemies.Count>0)
        {
            Enemysort();
            Enemy = Enemies[0];
            fightFSM = Fight_FSM.Attacking;
            
        }
        else
        {
            
            Collider[] c = Physics.OverlapSphere(transform.position, 30f, GameSystem_AllInfo.inst.masks[info.EnemyTeamIdx]);
            // 주변 적들 찾아냄.
            if (c.Length >0)
            {
                EnemyMoving= GameSystem_AllInfo.inst.FindNearestObject(transform.position,c); 
                //정렬
            
            }
        }
    }

    IEnumerator CoolCheck()
    {
        var a = new WaitForSeconds(2);
        yield return a;
        IsCool = true;
    }
    */
}
