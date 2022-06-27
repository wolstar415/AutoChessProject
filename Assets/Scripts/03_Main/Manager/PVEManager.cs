using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Random = UnityEngine.Random;

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
                    if (true)
                    {
                        
                
                    
                    pos = PlayerInfo.Inst.EnemyFiledTile[8].transform.position;
                    pos.y = 1.5f;
                   GameObject ob1= PhotonNetwork.Instantiate("Monster1", pos, Quaternion.Euler(0, 180, 0));
                    Enemis.Add(ob1);
                    ob1.GetComponent<Card_Info>().EnemyStart(150,35,1,0);
                    pos = PlayerInfo.Inst.EnemyFiledTile[11].transform.position;
                    pos.y = 1.5f;
                    GameObject ob2=  PhotonNetwork.Instantiate("Monster1", pos, Quaternion.Euler(0, 180, 0));
                    Enemis.Add(ob2);
                    ob2.GetComponent<Card_Info>().EnemyStart(150,35,1,1);

                    PlayerInfo.Inst.pVEdeadCnt = 2;
                    break;
                    }
                
                case 2:
                    if (true)
                    {
                        
                    pos = PlayerInfo.Inst.EnemyFiledTile[8].transform.position;
                    pos.y = 1.5f;
                    GameObject ob1= PhotonNetwork.Instantiate("Monster1", pos, Quaternion.Euler(0, 180, 0));
                    Enemis.Add(ob1);
                    ob1.GetComponent<Card_Info>().EnemyStart(150,35,1,0);
                    
                    pos = PlayerInfo.Inst.EnemyFiledTile[11].transform.position;
                    pos.y = 1.5f;
                    GameObject ob2=  PhotonNetwork.Instantiate("Monster1", pos, Quaternion.Euler(0, 180, 0));
                    Enemis.Add(ob2);
                    ob2.GetComponent<Card_Info>().EnemyStart(150,35,1,1);
                    
                    pos = PlayerInfo.Inst.EnemyFiledTile[23].transform.position;
                    pos.y = 1.5f;
                    GameObject ob3=  PhotonNetwork.Instantiate("Monster1", pos, Quaternion.Euler(0, 180, 0));
                    Enemis.Add(ob3);
                    ob3.GetComponent<Card_Info>().EnemyStart(150,35,1,1);

                    PlayerInfo.Inst.pVEdeadCnt = 3;
                    break;
                    }
                case 3:
                    if (true)
                    {
                        
                    pos = PlayerInfo.Inst.EnemyFiledTile[8].transform.position;
                    pos.y = 1.5f;
                    GameObject ob1= PhotonNetwork.Instantiate("Monster1", pos, Quaternion.Euler(0, 180, 0));
                    Enemis.Add(ob1);
                    ob1.GetComponent<Card_Info>().EnemyStart(150,35,1,0);
                    
                    pos = PlayerInfo.Inst.EnemyFiledTile[11].transform.position;
                    pos.y = 1.5f;
                    GameObject ob2=  PhotonNetwork.Instantiate("Monster1", pos, Quaternion.Euler(0, 180, 0));
                    Enemis.Add(ob2);
                    ob2.GetComponent<Card_Info>().EnemyStart(150,35,1,1);
                    
                    pos = PlayerInfo.Inst.EnemyFiledTile[23].transform.position;
                    pos.y = 1.5f;
                    GameObject ob3=  PhotonNetwork.Instantiate("Monster2", pos, Quaternion.Euler(0, 180, 0));
                    Enemis.Add(ob3);
                    ob3.GetComponent<Card_Info>().EnemyStart(150,35,1,2);

                    PlayerInfo.Inst.pVEdeadCnt = 3;
                    break;
                    }
                case 4:
                    if (true)
                    {
                        
                        pos = PlayerInfo.Inst.EnemyFiledTile[8].transform.position;
                        pos.y = 1.5f;
                        GameObject ob1= PhotonNetwork.Instantiate("Monster3", pos, Quaternion.Euler(0, 180, 0));
                        Enemis.Add(ob1);
                        ob1.GetComponent<Card_Info>().EnemyStart(300,50,1,0);
                    
                        pos = PlayerInfo.Inst.EnemyFiledTile[11].transform.position;
                        pos.y = 1.5f;
                        GameObject ob2=  PhotonNetwork.Instantiate("Monster3", pos, Quaternion.Euler(0, 180, 0));
                        Enemis.Add(ob2);
                        ob2.GetComponent<Card_Info>().EnemyStart(300,50,1,1);
                    
                        pos = PlayerInfo.Inst.EnemyFiledTile[23].transform.position;
                        pos.y = 1.5f;
                        GameObject ob3=  PhotonNetwork.Instantiate("Monster3", pos, Quaternion.Euler(0, 180, 0));
                        Enemis.Add(ob3);
                        ob3.GetComponent<Card_Info>().EnemyStart(300,50,1,2);

                        PlayerInfo.Inst.pVEdeadCnt = 3;
                        break;
                    }
                case 5:
                    if (true)
                    {
                        
                        pos = PlayerInfo.Inst.EnemyFiledTile[8].transform.position;
                        pos.y = 1.5f;
                        GameObject ob1= PhotonNetwork.Instantiate("Monster4", pos, Quaternion.Euler(0, 180, 0));
                        Enemis.Add(ob1);
                        ob1.GetComponent<Card_Info>().EnemyStart(600,70,1,0);
                    
                        pos = PlayerInfo.Inst.EnemyFiledTile[11].transform.position;
                        pos.y = 1.5f;
                        GameObject ob2=  PhotonNetwork.Instantiate("Monster4", pos, Quaternion.Euler(0, 180, 0));
                        Enemis.Add(ob2);
                        ob2.GetComponent<Card_Info>().EnemyStart(600,70,1,1);
                    
                        pos = PlayerInfo.Inst.EnemyFiledTile[23].transform.position;
                        pos.y = 1.5f;
                        GameObject ob3=  PhotonNetwork.Instantiate("Monster4", pos, Quaternion.Euler(0, 180, 0));
                        Enemis.Add(ob3);
                        ob3.GetComponent<Card_Info>().EnemyStart(600,70,1,2);

                        PlayerInfo.Inst.pVEdeadCnt = 3;
                        break;
                    }
                case 6:
                    if (true)
                    {
                        
                        pos = PlayerInfo.Inst.EnemyFiledTile[10].transform.position;
                        pos.y = 1.5f;
                        GameObject ob1= PhotonNetwork.Instantiate("Monster5", pos, Quaternion.Euler(0, 180, 0));
                        Enemis.Add(ob1);
                        ob1.GetComponent<Card_Info>().EnemyStart(1500,100,1,100,Random.Range(9,53));
                        PlayerInfo.Inst.pVEdeadCnt = 1;
                        break;
                    }
                case 7:
                    if (true)
                    {
                        
                        pos = PlayerInfo.Inst.EnemyFiledTile[10].transform.position;
                        pos.y = 1.5f;
                        GameObject ob1= PhotonNetwork.Instantiate("Monster6", pos, Quaternion.Euler(0, 180, 0));
                        Enemis.Add(ob1);
                        ob1.GetComponent<Card_Info>().EnemyStart(2000,150,1,100,Random.Range(9,53));
                        PlayerInfo.Inst.pVEdeadCnt = 1;
                        break;
                    }
                case 8:
                    if (true)
                    {
                        
                        pos = PlayerInfo.Inst.EnemyFiledTile[10].transform.position;
                        pos.y = 1.5f;
                        GameObject ob1= PhotonNetwork.Instantiate("Monster7", pos, Quaternion.Euler(0, 180, 0));
                        Enemis.Add(ob1);
                        ob1.GetComponent<Card_Info>().EnemyStart(4000,200,1,100,Random.Range(9,53));
                        PlayerInfo.Inst.pVEdeadCnt = 1;
                        break;
                    }
                case 9:
                    if (true)
                    {
                        
                        pos = PlayerInfo.Inst.EnemyFiledTile[10].transform.position;
                        pos.y = 1.5f;
                        GameObject ob1= PhotonNetwork.Instantiate("Monster8", pos, Quaternion.Euler(0, 180, 0));
                        Enemis.Add(ob1);
                        ob1.GetComponent<Card_Info>().EnemyStart(6000,250,1,100,Random.Range(9,53));
                        PlayerInfo.Inst.pVEdeadCnt = 1; ;
                        break;
                    }
                default:
                    break;
            }
            //각각적들 생성시켜!
            //마스터가 적들넣어서 한번에 삭제시키자.
            
            
        }
        
        
        
    }
}
