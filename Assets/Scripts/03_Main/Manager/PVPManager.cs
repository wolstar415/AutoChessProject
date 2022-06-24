using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameS
{
    public class PVPManager : MonoBehaviour
    {
        public static PVPManager inst;
        public List<GameObject> copyob;

        private void Awake()
        {
            inst = this;
        }
        



        public void MasterBattleReady()
        {
            
            //사람들모여서
            List<int> playercheck = new List<int>();

            for (int i = 0; i < NetworkManager.inst.players.Count; i++)
            {
                if (!NetworkManager.inst.players[i].Dead&&NetworkManager.inst.players[i].State==1&&NetworkManager.inst.players[i].Life>=1)
                {
                    playercheck.Add(i);
                }
            }

            if (playercheck.Count<=1)
            {
                Debug.Log("버그");
                
            }

            while (playercheck.Count>0)
            {
                if (playercheck.Count==3)
                {
                    int ran = Random.Range(0, playercheck.Count);
                    int ranidx1 = playercheck[ran];
                    playercheck.RemoveAt(ran);
                    ran=Random.Range(0, playercheck.Count);
                    int ranidx2 = playercheck[ran];
                    playercheck.RemoveAt(ran);
                    ran=Random.Range(0, playercheck.Count);
                    int ranidx3 = playercheck[ran];
                    playercheck.RemoveAt(ran);
                    var instBattleinfo1 = GameSystem_AllInfo.inst.battleinfos[ranidx1];
                    var instBattleinfo2 = GameSystem_AllInfo.inst.battleinfos[ranidx2];
                    var instBattleinfo3 = GameSystem_AllInfo.inst.battleinfos[ranidx3];
                    instBattleinfo1.enemyidx = ranidx2;
                    instBattleinfo2.enemyidx = ranidx1;
                    instBattleinfo2.IsBattleMove = true;
                    instBattleinfo3.enemyidx = ranidx1;
                    instBattleinfo1.IsCopy = true;
                    instBattleinfo1.copyidx = ranidx3;
                }
                else
                {
                    int ran = Random.Range(0, playercheck.Count);
                    int ranidx = playercheck[ran];
                    playercheck.RemoveAt(ran);
                    ran=Random.Range(0, playercheck.Count);
                    int ranidx2 = playercheck[ran];
                    playercheck.RemoveAt(ran);
                    var instBattleinfo1 = GameSystem_AllInfo.inst.battleinfos[ranidx];
                    var instBattleinfo2 = GameSystem_AllInfo.inst.battleinfos[ranidx2];
                    instBattleinfo1.enemyidx = ranidx2;
                    instBattleinfo2.enemyidx = ranidx;
                    instBattleinfo2.IsBattleMove = true;
                }
            }

            MasterInfo.inst.BtSent();

        }
    }
}
