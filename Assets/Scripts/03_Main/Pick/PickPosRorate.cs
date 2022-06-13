using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class PickPosRorate : MonoBehaviour
    {
        
        void Update()
        {
            if (PhotonNetwork.IsMasterClient&&PlayerInfo.Inst.PickRound)
            {
            transform.Rotate(new Vector3(0, 6 * Time.deltaTime, 0));
                
            }
        }
    }
}
