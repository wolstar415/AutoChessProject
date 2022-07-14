using System;
using System.Collections;
using System.Collections.Generic;
using GameS;
using UnityEngine;

public class UIHpbarMove : MonoBehaviour
{
    //Transform cam;

    // Start is called before the first frame update
    void Start()
    {
        UpdateManager.Inst.obs.Add(gameObject);
        //cam = Camera.main.transform;
    }

    private void OnDestroy()
    {
        UpdateManager.Inst.obs.Remove(gameObject);
    }
    // Update is called once per frame
    // void Update()
    // {
    //     transform.LookAt(transform.position + cam.rotation * Vector3.forward, cam.rotation * Vector3.up);   
    // }
}
