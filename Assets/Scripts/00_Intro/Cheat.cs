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
#endif
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
            else if (Input.GetKeyDown(KeyCode.Keypad4))
            {
                PlayerInfo.Inst.EnemyIdx = 1;
                PlayerInfo.Inst.BattleMove = true;
                RoundManager.inst.BattleMoveFunc();
            }
            else if (Input.GetKeyDown(KeyCode.Keypad5))
            {
                //PlayerInfo.Inst.EnemyIdx = -1;
                // PlayerInfo.Inst.BattleMove = false;
                RoundManager.inst.BattleMoveFunc2();
            }
            else if (Input.GetKeyDown(KeyCode.Keypad6))
            {
                //PlayerInfo.Inst.EnemyIdx = -1;
                // PlayerInfo.Inst.BattleMove = false;
                ItemManager.inst.ItemAdd(Random.Range(0,9));
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
