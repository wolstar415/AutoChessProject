using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerMoving : MonoBehaviour
{
    [SerializeField]NavMeshAgent nav;
    [SerializeField]Animator ani;
    bool IsMoving;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                nav.SetDestination(hit.point);
                ani.SetBool("Run", true);
                IsMoving = true;
                nav.isStopped = false;
            }

        }
        Debug.Log("nav.velocity.sqrMagnitude ::::::" + nav.velocity.sqrMagnitude);
        Debug.Log("nav.remainingDistance ::::::" + nav.remainingDistance);
        if (IsMoving && (nav.velocity.sqrMagnitude >= 0.2f * 0.2f && nav.remainingDistance <= 0.5f))
        // remainingDistance 지정된 목적지까지 남은 거리를 반환.
        // velocity.sqrMagnitude 거리를 구할 때 사용한다.
        {
            IsMoving = false;
            nav.isStopped = true;
            ani.SetBool("Run", false);
            
        }

        
        

        
    }
}
