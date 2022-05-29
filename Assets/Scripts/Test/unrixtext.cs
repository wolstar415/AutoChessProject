using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class unrixtext : MonoBehaviour
{
    public Subject<int> subject1 = new Subject<int>();
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            subject1.OnNext(123);
        }
    }
}
