using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class UnitState : MonoBehaviourPunCallbacks
    {
        public PhotonView pv;
        public float Hp;
        public float Mp;
        public bool IsDead;
        public bool IsInvin;
        public bool IsStun;
    }
}
