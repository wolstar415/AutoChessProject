using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Scan : MonoBehaviour
{
    [SerializeField] Card_Info info;
    [SerializeField] Card_fight info_fight;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cards")&& other.gameObject.layer!=info.TeamIdx+6)
        {
            info_fight.Enemies.Add(other.gameObject);
            info_fight.EnemyFindFunc();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Cards")&& other.gameObject.layer!=info.TeamIdx+6)
        {
            
            info_fight.Enemies.Remove(other.gameObject);
            if (info_fight.Enemy==other.gameObject)
            {
                info_fight.Enemy = null;
            }

        }
    }
}
