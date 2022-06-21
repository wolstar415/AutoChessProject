using System;
using System.Collections;
using System.Collections.Generic;
using GameS;
using UnityEngine;

public class item29_Scan : MonoBehaviour
{
    [SerializeField] Card_Info info;
    public List<GameObject> Enemies;
    

    private void OnTriggerEnter(Collider other)
    {
        
        if ((other.CompareTag("Cards")||other.CompareTag("Enemy"))&& other.gameObject.layer!=info.TeamIdx+6)
        {
            if (other.GetComponent<Card_Info>().IsFiled )
            {
                Enemies.Add(other.gameObject);
                other.GetComponent<CardState>().Item29Func(true);
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.CompareTag("Cards")||other.CompareTag("Enemy"))&& other.gameObject.layer!=info.TeamIdx+6)
        {
            if (other.GetComponent<Card_Info>().IsFiled )
            {
                Enemies.Remove(other.gameObject);
                other.GetComponent<CardState>().Item29Func(false);

            }
            

        }
    }
}
