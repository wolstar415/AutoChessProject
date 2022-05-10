using System.Collections;
using System.Collections.Generic;
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
    public bool IsAttacker=false;
    public GameObject Enemy=null;
    public bool IsDead = false;
    public float Hp = 0;
    [SerializeField] NavMeshAgent nav;
    // Start is called before the first frame update
    void Start()
    {
        fightFSM = Fight_FSM.None;

        fightFSM= Fight_FSM.idle;
        IsFighting = true;
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
            case Fight_FSM.idle:
                if (Enemy.TryGetComponent(out Card_Info com))
                {
                    if (com.IsDead==true)
                    {
                        Enemy = null;
                    }
                }
                if (Enemy !=null)
                {
                    fightFSM = Fight_FSM.Moving;
                    nav.SetDestination(Enemy.transform.position);
                }
                break;
            case Fight_FSM.Moving:
                
                break;
            case Fight_FSM.Attacking:
                break;
            default:
                break;
        }
    }

    void EnemyFind()
    {

    }
}
