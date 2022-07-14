using System.Collections;
using System.Collections.Generic;
using Singleton1;
using UnityEngine;

namespace GameS
{
    public class UpdateManager : MonoSingleton<UpdateManager>
    {
        
        Transform cam;

        public List<GameObject> obs;
        // Start is called before the first frame update
        void Start()
        {
            cam = Camera.main.transform;
        }

        // Update is called once per frame
        void Update()
        {
            //GameObject[] obs = GameObject.FindGameObjectsWithTag("HpBar");

            foreach (var ob in obs)
            {
                
                ob.transform.LookAt(ob.transform.position + cam.rotation * Vector3.forward, cam.rotation * Vector3.up);   
            }
        }

    }
}
