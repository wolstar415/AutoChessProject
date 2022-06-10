using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameS
{
    public class RoundManager : MonoBehaviour
    {
        public static RoundManager inst;

        private void Awake()
        {
            inst = this;
        }

        //팀 모두 이동
        public void BattleMoveFunc()
        {
            if (PlayerInfo.Inst.EnemyIdx==-1|| PlayerInfo.Inst.BattleMove==false) return;

            int Enemyidx = PlayerInfo.Inst.EnemyIdx;
            PlayerInfo.Inst.EnemyFiledTile = PositionManager.inst.playerPositioninfo[Enemyidx].EnemyFiledTile;
            PlayerInfo.Inst.PlayerTileOb = PositionManager.inst.playerPositioninfo[Enemyidx].EnemyPlayerTileob;
            EventManager.inst.Sub_CardBattleMove.OnNext(1);
            GoldSet();
            CameraMoveFunc2();
        }

        //원래대로 이동하기
        public void BattleMoveFunc2()
        {
            GoldObReSet();
            if (PlayerInfo.Inst.EnemyIdx==-1|| PlayerInfo.Inst.BattleMove==false) return;
            
            int pidx = PlayerInfo.Inst.PlayerIdx;
            EventManager.inst.Sub_CardBattleMove.OnNext(2);
            PlayerInfo.Inst.PlayerTileOb = PositionManager.inst.playerPositioninfo[pidx].PlayerTileob;
            CameraMoveFunc1();
            PlayerInfo.Inst.EnemyIdx = -1;
            PlayerInfo.Inst.BattleMove = false;
        }

        public void GoldSet()
        {
            int Enemyidx = PlayerInfo.Inst.EnemyIdx;
            for (int i = 0; i <5; i++)
            {
                if (PlayerInfo.Inst.Gold <10*(i+1))
                {
                    GameObject ob = PositionManager.inst.playerPositioninfo[Enemyidx].EnemyGoldOb[i];
                    if (ob.TryGetComponent(out MeshRenderer mesh))
                    {
                        mesh.material.color = PlayerInfo.Inst.colors[0];
                    }
                
                }
                else
                {
                    GameObject ob = PositionManager.inst.playerPositioninfo[Enemyidx].EnemyGoldOb[i];
                    if (ob.TryGetComponent(out MeshRenderer mesh))
                    {
                        mesh.material.color = PlayerInfo.Inst.colors[1];
                    }
                }
            
            
            }
        }

        public void GoldObReSet()
        {
            for (int i = 0; i < PlayerInfo.Inst.EnemyGoldOb.Count; i++)
            {
                if (PlayerInfo.Inst.EnemyGoldOb[i].TryGetComponent(out MeshRenderer mesh))
                {
                    mesh.material.color = PlayerInfo.Inst.colors[0];
                }
            }
        }

        public void CameraMoveFunc1()
        {
            int idx = PlayerInfo.Inst.PlayerIdx;
            PlayerInfo.Inst.camer.transform.position = PositionManager.inst.Camera_Pos[idx].position;
            PlayerInfo.Inst.camer.transform.rotation = PositionManager.inst.Camera_Pos[idx].rotation;
        }
        public void CameraMoveFunc2()
        {
            int idx = PlayerInfo.Inst.EnemyIdx;
            PlayerInfo.Inst.camer.transform.position = PositionManager.inst.Camera_AttackPos[idx].position;
            PlayerInfo.Inst.camer.transform.rotation = PositionManager.inst.Camera_AttackPos[idx].rotation;
        }
    }
}
