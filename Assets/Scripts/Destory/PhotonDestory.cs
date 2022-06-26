using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class PhotonDestory : MonoBehaviour
    {
        public float destime = 1;
        // Start is called before the first frame update
        void Start()
        {
        Invoke("PhoDestory",destime);
        }

        void PhoDestory()
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
