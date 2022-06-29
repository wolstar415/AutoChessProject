using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class PhotonDestory : MonoBehaviour
    {
        public float destime = 1;

        public PhotonView pv;
        // Start is called before the first frame update
        void Start()
        {
            if (pv.IsMine)
            {
        Invoke("PhoDestory",destime);
                
            }
        }

        void PhoDestory()
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
