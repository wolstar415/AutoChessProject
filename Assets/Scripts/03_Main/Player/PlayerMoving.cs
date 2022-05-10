using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerMoving : MonoBehaviour
{
    [SerializeField]NavMeshAgent Nav;
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
                Nav.SetDestination(hit.point);
                ani.SetBool("Run", true);
                IsMoving = true;
                Nav.isStopped = false;
            }

        }
        if (IsMoving && (Nav.velocity.sqrMagnitude >= 0.2f * 0.2f && Nav.remainingDistance <= 0.5f))
        {
            IsMoving = false;
            Nav.isStopped = true;
            ani.SetBool("Run", false);
            
        }

        
        

        
    }
}
