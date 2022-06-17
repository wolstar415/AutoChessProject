using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace GameS
{
    public class PVEManager : MonoBehaviourPunCallbacks
    {
        public static PVEManager inst;
        public List<GameObject> Enemis;

        private void Awake()
        {
            inst = this;
        }

        public void PVEstart()
        {
            
            PveCreate();
        }

        void PveCreate()
        {
            int roundidx = PlayerInfo.Inst.RoundIdx;
            Vector3 pos;

            switch (roundidx)
            {
                case 1:
                    pos = PlayerInfo.Inst.EnemyFiledTile[8].transform.position;
                    pos.y = 1.5f;
                   GameObject ob1= PhotonNetwork.Instantiate("Monster1", pos, Quaternion.Euler(0, 180, 0));
                    Enemis.Add(ob1);
                    ob1.GetComponent<Card_Info>().EnemyStart(150,35,1,0);
                    pos = PlayerInfo.Inst.EnemyFiledTile[11].transform.position;
                    pos.y = 1.5f;
                    GameObject ob2=  PhotonNetwork.Instantiate("Monster1", pos, Quaternion.Euler(0, 180, 0));
                    Enemis.Add(ob2);
                    ob2.GetComponent<Card_Info>().EnemyStart(150,35,1,0);

                    PlayerInfo.Inst.pVEdeadCnt = 2;
                    break;
                default:
                    break;
            }
            //각각적들 생성시켜!
            //마스터가 적들넣어서 한번에 삭제시키자.
            
            
        }
        
        
        
    }
}
