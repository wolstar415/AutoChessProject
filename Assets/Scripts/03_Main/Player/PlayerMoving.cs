using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerMoving : MonoBehaviourPunCallbacks
{
    [SerializeField] private PhotonView pv;
    public NavMeshAgent nav;
    [SerializeField]Animator ani;
    bool IsMoving;


    public void check1()
    {
        
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                nav.ResetPath();
                ani.SetBool("Run", true);
                IsMoving = true;
                nav.isStopped = false;
                nav.updatePosition = true;
                nav.updateRotation = true;
                nav.SetDestination(hit.point);
            }
        
    }
    public void movePos(Vector3 pos)
    {
        nav.ResetPath();
        ani.SetBool("Run", true);
        IsMoving = true;
        
        nav.isStopped = false;
        nav.updatePosition = true;
        nav.updateRotation = true;
        nav.SetDestination(pos);
    }
    void Update()
    {
        if (!pv.IsMine)
        {
            return;
        }

        if (IsMoving && (nav.velocity.sqrMagnitude >= 0.2f * 0.2f && nav.remainingDistance <= 0.5f))
           
        // remainingDistance ������ ���������� ���� �Ÿ��� ��ȯ.
        // velocity.sqrMagnitude �ӵ�.
        {
            
            IsMoving = false;
            nav.isStopped = true;
            nav.updatePosition = false;
            nav.updateRotation = false;
            nav.velocity=Vector3.zero;
            ani.SetBool("Run", false);
            nav.ResetPath();
        }

        
        

        
    }
}
