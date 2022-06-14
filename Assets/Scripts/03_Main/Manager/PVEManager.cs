using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameS
{
    public class PVEManager : MonoBehaviour
    {
        public static PVEManager inst;

        private void Awake()
        {
            inst = this;
        }

        public void PveCreate()
        {
            int roundidx = PlayerInfo.Inst.RoundIdx;
            
            //각각적들 생성시켜!
            //마스터가 적들넣어서 한번에 삭제시키자.
            
            
        }
    }
}
