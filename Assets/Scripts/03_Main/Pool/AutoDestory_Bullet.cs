using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameS
{
    public class AutoDestory_Bullet : MonoBehaviour
    {
        public float deadTime = 5;
        
        // Start is called before the first frame update

        

        private void OnEnable()
        {
            Invoke("ADead",deadTime);
        }

        void ADead()
        {
            gameObject.SetActive(false);
        }
        
        private void OnDisable()
        {

            ObjectPooler.ReturnToPool(gameObject);
        }
    }
}
