using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test4 : MonoBehaviour
{
    public string s;
    // Start is called before the first frame update
    void Start()
    {
        float damage=100.5f;
        string s="{damage}데미지";
        string c="<color=red>";


        string s2=$"{s}";
        Debug.Log(s2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
