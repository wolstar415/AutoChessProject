using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameS
{
    public class Cheat : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                //NetworkManager.inst.CameraMovePick();
            }
            else if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                //NetworkManager.inst.CameraMoveNormal(1);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                PlayerInfo.Inst.Life--;
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
