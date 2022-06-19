using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class iteminfo : MonoBehaviour
    {
        public PhotonView pv;
        public int idx = 0;
        public bool IsCoin;
        public bool IsItem;
        public bool IsPick = false;
        public SphereCollider co;
        
        public void StartFunc(int _idx)
        {
            idx = _idx;
            Invoke("pick",1.5f);
        }

        void pick()
        {
            co.enabled=true;
            IsPick = true;
        }
    }
}
