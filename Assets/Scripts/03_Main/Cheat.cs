using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GameS
{
    public class Cheat : MonoBehaviour
    {
    public int idx = 0;
    public bool IsA = false;
    
#if unity_editor
    [MenuItem("MyMenu/NextMap")]
    static void NextMap()
    {
        int idx = GameObject.Find("Cheat").GetComponent<Cheat>().idx;
        idx++;
        if (idx>=8)
        {
            idx=0;
        }

        if (GameObject.Find("Cheat").GetComponent<Cheat>().IsA)
        {
            GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>().camer.transform.position =
                GameObject.Find("PositionManager").GetComponent<PositionManager>().Camera_AttackPos[idx].localPosition;
            GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>().camer.transform.rotation =
                GameObject.Find("PositionManager").GetComponent<PositionManager>().Camera_AttackPos[idx].localRotation;

        }
        else
        {
            GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>().camer.transform.position =
                GameObject.Find("PositionManager").GetComponent<PositionManager>().Camera_Pos[idx].localPosition;
            GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>().camer.transform.rotation =
                GameObject.Find("PositionManager").GetComponent<PositionManager>().Camera_Pos[idx].localRotation;

        }
        Debug.Log($"인덱스:{idx}");
        GameObject.Find("Cheat").GetComponent<Cheat>().idx = idx;
    }
    
    [MenuItem("MyMenu/beforeMap")]
    static void beforeMap()
    {
        int idx = GameObject.Find("Cheat").GetComponent<Cheat>().idx;
        idx--;
        if (idx<=-1)
        {
            idx =7;
        }
        if (GameObject.Find("Cheat").GetComponent<Cheat>().IsA)
        {
            GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>().camer.transform.position =
                GameObject.Find("PositionManager").GetComponent<PositionManager>().Camera_AttackPos[idx].localPosition;
            GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>().camer.transform.rotation =
                GameObject.Find("PositionManager").GetComponent<PositionManager>().Camera_AttackPos[idx].localRotation;

        }
        else
        {
            GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>().camer.transform.position =
                GameObject.Find("PositionManager").GetComponent<PositionManager>().Camera_Pos[idx].localPosition;
            GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>().camer.transform.rotation =
                GameObject.Find("PositionManager").GetComponent<PositionManager>().Camera_Pos[idx].localRotation;

        }
        Debug.Log($"인덱스:{idx}");
        GameObject.Find("Cheat").GetComponent<Cheat>().idx = idx;
    }
    [MenuItem("MyMenu/change")]
    static void change()
    {
        int idx = GameObject.Find("Cheat").GetComponent<Cheat>().idx;
        GameObject.Find("Cheat").GetComponent<Cheat>().IsA = !GameObject.Find("Cheat").GetComponent<Cheat>().IsA;
        if (GameObject.Find("Cheat").GetComponent<Cheat>().IsA)
        {
            Debug.Log($"인덱스:{idx} , 현재 공격중");
            GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>().camer.transform.position =
                GameObject.Find("PositionManager").GetComponent<PositionManager>().Camera_AttackPos[idx].localPosition;
            GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>().camer.transform.rotation =
                GameObject.Find("PositionManager").GetComponent<PositionManager>().Camera_AttackPos[idx].localRotation;

        }
        else
        {
            Debug.Log($"인덱스:{idx} , 현재 방어중");
            GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>().camer.transform.position =
                GameObject.Find("PositionManager").GetComponent<PositionManager>().Camera_Pos[idx].localPosition;
            GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>().camer.transform.rotation =
                GameObject.Find("PositionManager").GetComponent<PositionManager>().Camera_Pos[idx].localRotation;

        }
        
    }
    [MenuItem("MyMenu/기본값")]
    static void check2()
    {
        GameObject.Find("Cheat").GetComponent<Cheat>().idx=0;
        GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>().camer.transform.position =
            GameObject.Find("PositionManager").GetComponent<PositionManager>().Camera_Pos[0].localPosition;
        GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>().camer.transform.rotation =
            GameObject.Find("PositionManager").GetComponent<PositionManager>().Camera_Pos[0].localRotation;


        GameObject.Find("Cheat").GetComponent<Cheat>().IsA = false;

    }
    
    [MenuItem("MyMenu/PickNext")]
    static void Pick1()
    {
        int idx = GameObject.Find("Cheat").GetComponent<Cheat>().idx;
        idx++;
        if (idx>=8)
        {
            idx=0;
        }
        
            GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>().camer.transform.position =
                GameObject.Find("PositionManager").GetComponent<PositionManager>().Camera_PickPos[idx].localPosition;
            GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>().camer.transform.rotation =
                GameObject.Find("PositionManager").GetComponent<PositionManager>().Camera_PickPos[idx].localRotation;

        
        Debug.Log($"인덱스:{idx}");
        GameObject.Find("Cheat").GetComponent<Cheat>().idx = idx;

    }
    [MenuItem("MyMenu/PickBefore")]
    static void Pick2()
    {
        int idx = GameObject.Find("Cheat").GetComponent<Cheat>().idx;
        idx--;
        if (idx<=-1)
        {
            idx=7;
        }
        
        GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>().camer.transform.position =
            GameObject.Find("PositionManager").GetComponent<PositionManager>().Camera_PickPos[idx].localPosition;
        GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>().camer.transform.rotation =
            GameObject.Find("PositionManager").GetComponent<PositionManager>().Camera_PickPos[idx].localRotation;

        
        Debug.Log($"인덱스:{idx}");
        GameObject.Find("Cheat").GetComponent<Cheat>().idx = idx;

    }
                [MenuItem("MyMenu/change")]
        static void Check()
        {
            GameObject ob1 = GameObject.Find("Check1");
            GameObject ob2 = GameObject.Find("Check2");

            
            Collider[] c = Physics.OverlapSphere(ob1.transform.position, 3, LayerMask.GetMask("Player_1"));
            if (c.Length==0) 
            {
                Debug.Log($"안들어옴 거리:{Vector3.Distance(ob1.transform.position,ob2.transform.position)}");

            }
            else
            {
                Debug.Log($"들어옴 거리:{Vector3.Distance(ob1.transform.position,ob2.transform.position)}");

            }
            
        
        }
        
        [MenuItem("MyMenu/change")]
        static void Check2()
        {
            GameObject ob1 = GameObject.Find("Check1");
            GameObject ob2 = GameObject.Find("Check2");

            GameObject line=GameObject.Find("SkillLine1");
            
            line.GetComponent<LineRenderer>().SetPosition(0,ob1.transform.position);
            line.GetComponent<LineRenderer>().SetPosition(1,ob2.transform.position);


        }

#endif
        
        [MenuItem("MyMenu/change")]
        static void Check()
        {
            GameObject ob1 = GameObject.Find("Check1");
            GameObject ob2 = GameObject.Find("Check2");

            
            Collider[] c = Physics.OverlapSphere(ob1.transform.position, 9, LayerMask.GetMask("Player_1"));
            
            float dotValue = Mathf.Cos(Mathf.Deg2Rad * ((50) / 2));
            Vector3 direction = ob2.transform.position - ob1.transform.position;
            direction.Normalize();
            
                if (Vector3.Dot(direction,ob1.transform.forward)>dotValue)
                {
                    
                    Debug.Log($"들어옴 거리:{Vector3.Distance(ob1.transform.position,ob2.transform.position)}");

                }
                else
                {
                    Debug.Log($"안들어옴 거리:{Vector3.Distance(ob1.transform.position,ob2.transform.position)}");

                }
                
            
        
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                idx++;
                if (idx>=8)
                {
                    idx =0;
                }
                PlayerInfo.Inst.camer.transform.position = PositionManager.inst.Camera_Pos[idx].position;
                PlayerInfo.Inst.camer.transform.rotation = PositionManager.inst.Camera_Pos[idx].rotation;
                Debug.Log($"인덱스:{idx}");
            }
            else if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                idx--;
                if (idx<=-1)
                {
                    idx =7;
                }
                PlayerInfo.Inst.camer.transform.position = PositionManager.inst.Camera_Pos[idx].position;
                PlayerInfo.Inst.camer.transform.rotation = PositionManager.inst.Camera_Pos[idx].rotation;

                Debug.Log($"인덱스:{idx}");
                //NetworkManager.inst.CameraMoveNormal(1);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                IsA = !IsA;
                if (IsA)
                {
                Debug.Log($"인덱스:{idx} , 현재 공격중");
                    PlayerInfo.Inst.camer.transform.position = PositionManager.inst.Camera_AttackPos[idx].position;
                    PlayerInfo.Inst.camer.transform.rotation = PositionManager.inst.Camera_AttackPos[idx].rotation;

                }
                else
                {
                Debug.Log($"인덱스:{idx} , 현재 방어중");
                    PlayerInfo.Inst.camer.transform.position = PositionManager.inst.Camera_Pos[idx].position;
                    PlayerInfo.Inst.camer.transform.rotation = PositionManager.inst.Camera_Pos[idx].rotation;

                }

            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                ItemManager.inst.ItemAdd(9);
                ItemManager.inst.ItemAdd(10);
                ItemManager.inst.ItemAdd(11);
                ItemManager.inst.ItemAdd(12);
                ItemManager.inst.ItemAdd(13);
                ItemManager.inst.ItemAdd(14);
                ItemManager.inst.ItemAdd(15);
                ItemManager.inst.ItemAdd(16);
                ItemManager.inst.ItemAdd(17);
                ItemManager.inst.ItemAdd(18);
                ItemManager.inst.ItemAdd(19);
            }
            else if (Input.GetKeyDown(KeyCode.V))
            {
                ItemManager.inst.ItemAdd(20);
                ItemManager.inst.ItemAdd(21);
                ItemManager.inst.ItemAdd(21);
                ItemManager.inst.ItemAdd(22);
                ItemManager.inst.ItemAdd(23);
                ItemManager.inst.ItemAdd(24);
                ItemManager.inst.ItemAdd(25);
                ItemManager.inst.ItemAdd(26);
                ItemManager.inst.ItemAdd(27);
                ItemManager.inst.ItemAdd(28);
                ItemManager.inst.ItemAdd(29);
            }
            else if (Input.GetKeyDown(KeyCode.B))
            {
                ItemManager.inst.ItemAdd(30);
                ItemManager.inst.ItemAdd(31);
                ItemManager.inst.ItemAdd(32);
                ItemManager.inst.ItemAdd(33);
                ItemManager.inst.ItemAdd(34);
                ItemManager.inst.ItemAdd(35);
                ItemManager.inst.ItemAdd(36);
                ItemManager.inst.ItemAdd(37);
                ItemManager.inst.ItemAdd(38);
                ItemManager.inst.ItemAdd(39);
                ItemManager.inst.ItemAdd(40);
            }
            else if (Input.GetKeyDown(KeyCode.N))
            {
                ItemManager.inst.ItemAdd(41);
                ItemManager.inst.ItemAdd(42);
                ItemManager.inst.ItemAdd(43);
                ItemManager.inst.ItemAdd(44);
                ItemManager.inst.ItemAdd(45);
                ItemManager.inst.ItemAdd(46);
                ItemManager.inst.ItemAdd(47);
                ItemManager.inst.ItemAdd(48);
                ItemManager.inst.ItemAdd(49);
                ItemManager.inst.ItemAdd(50);
                ItemManager.inst.ItemAdd(51);
                ItemManager.inst.ItemAdd(52);
                ItemManager.inst.ItemAdd(53);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad4))
            {
                // ItemManager.inst.ItemAdd(17);
                // ItemManager.inst.ItemAdd(25);
                // ItemManager.inst.ItemAdd(32);
                // ItemManager.inst.ItemAdd(38);
                // ItemManager.inst.ItemAdd(43);
                // ItemManager.inst.ItemAdd(47);
                // ItemManager.inst.ItemAdd(50);
                // ItemManager.inst.ItemAdd(52);
                PlayerInfo.Inst.PlayerCard_Filed[0].GetComponent<CardState>().NetStopFunc(true,1f,true);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad5))
            {
                CreateManager.inst.CreateCharacter(50);
                CreateManager.inst.CreateCharacter(51);
                CreateManager.inst.CreateCharacter(52);
                CreateManager.inst.CreateCharacter(53);
                ItemManager.inst.ItemAdd(44);
                ItemManager.inst.ItemAdd(44);
                ItemManager.inst.ItemAdd(44);
                ItemManager.inst.ItemAdd(44);
                ItemManager.inst.ItemAdd(44);
                ItemManager.inst.ItemAdd(44);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad6))
            {
                //PlayerInfo.Inst.EnemyIdx = -1;
                // PlayerInfo.Inst.BattleMove = false;
                ItemManager.inst.ItemAdd(Random.Range(0,9));
            }
            else if (Input.GetKeyDown(KeyCode.Keypad7))
            {
                for (int i = 0; i < PlayerInfo.Inst.PlayerCard_Filed.Count; i++)
                {
                    var a = PlayerInfo.Inst.PlayerCard_Filed[i].GetComponent<CardState>();
                    Debug.Log("----------------------------------------------");
                    Debug.Log($"이름 : {a.info.name} ,공속 : {a.Atk_Cool()}");
                    Debug.Log("----------------------------------------------");
                }
            }
            else if (Input.GetKeyDown(KeyCode.Keypad9))
            {
                PlayerInfo.Inst.Gold += 5;

            }
            else if (Input.GetKeyUp(KeyCode.T))
            {
                UIManager.inst.TimeEnd();
            }
            else if (Input.GetKeyUp(KeyCode.F))
            {
                NetworkManager.inst.MasterInfoOrder();
            }
            else if (Input.GetKeyUp(KeyCode.G))
            {
                UIManager.inst.PlayerInfoBattleStart();
            }
            else if (Input.GetKeyUp(KeyCode.S))
            {
                NetworkManager.inst.TextUi("999",PlayerInfo.Inst.PlayerOb.transform.position,5);
                NetworkManager.inst.TextUi("999",PlayerInfo.Inst.PlayerOb.transform.position-new Vector3(0,0,3),1);
            }
        }
    }
}
