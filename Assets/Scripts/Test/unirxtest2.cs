using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class unirxtest2 : MonoBehaviour
{
    public unrixtext test1;

    public IDisposable Dead;
    // Start is called before the first frame update
    void Start()
    {
        Dead = test1.subject1.Subscribe(time => Ch());
        if (gameObject.name=="나")
        {
            Dead.Dispose();
        }


    }

    void Ch()
    {
        Debug.Log(gameObject.name);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
