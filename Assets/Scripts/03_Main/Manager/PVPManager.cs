using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameS
{
    public class PVPManager : MonoBehaviour
    {
        public static PVPManager inst;

        private void Awake()
        {
            inst = this;
        }
    }
}
