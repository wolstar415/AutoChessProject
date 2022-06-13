using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
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

        public void RoundStart(int Round)
        {
            int RoundCheck = CsvManager.inst.RoundCheck3[Round];
            PlayerInfo.Inst.RoundIdx = RoundCheck;
            PlayerInfo.Inst.Round = Round;

            if (RoundCheck<100)
            {
                Round_PVE();
            }
            else if (RoundCheck<1000)
            {
                Round_PVP();
            }
            else
            {
                Round_Pick();
            }
        }










        void Round_PVP()
        {
            Round_PVP_StartFunc();
        }

        void Round_PVE()
        {
            Round_PVE_StartFunc();
        }

        void Round_Pick()
        {
            Round_PICk_StartFunc();
            
        }


         void Round_PVP_StartFunc()
        {
            if (PlayerInfo.Inst.Dead == true) return;
            
        }
         void Round_PVE_StartFunc()
        {
            if (PlayerInfo.Inst.Dead == true) return;
            
        }
         void Round_PICk_StartFunc()
        {
            if (PlayerInfo.Inst.Dead == true) return;
            int idxcheck = PlayerInfo.Inst.RoundIdx;
            int idx = PlayerInfo.Inst.PlayerIdx;

            PlayerInfo.Inst.PickRound = true;
            //UI설정
            UIManager.inst.PickUiSetting();
            //마스터에게 액션실행
            if (PhotonNetwork.IsMasterClient)
            {
                //카드들 생성
                MasterInfo.inst.PickPlayerCheck();
                MasterInfo.inst.PickCardCreate(idxcheck);
            }
            //카메라이동
            PlayerInfo.Inst.camer.transform.SetPositionAndRotation(PositionManager.inst.Camera_PickPos.position,PositionManager.inst.Camera_PickPos.rotation);
            //플레이어이동
            PlayerInfo.Inst.PlayerOb.transform.position = PositionManager.inst.PickPos[idx].position;
            //길막기 모두 정상
            PickRoundManager.inst.PickAllOpen();
            
            
        }
        
        public void Round_PVP_EndFunc()
        {
            if (PlayerInfo.Inst.Dead == true) return;
            
            RoundNext();
        }
        public void Round_PVE_EndFunc()
        {
            if (PlayerInfo.Inst.Dead == true) return;
            
            RoundNext();
        }
        public void Round_PICk_EndFunc()
        {
            if (PlayerInfo.Inst.Dead == true) return;


            if (PlayerInfo.Inst.PlayerOb.TryGetComponent(out PlayerPickSelect pick))
            {
                pick.StopAllCoroutines();
                GameObject ob = pick.pickOb;
                
                CreateManager.inst.Pick_CreateCharacter(ob);
                ob.transform.rotation=Quaternion.identity;
                
            }
            int idx = PlayerInfo.Inst.PlayerIdx;
            //카메라설정 본인자리로 가기
            PlayerInfo.Inst.camer.transform.SetPositionAndRotation(PositionManager.inst.Camera_Pos[idx].position,
                PositionManager.inst.Camera_Pos[idx].rotation);
            
                
            //UI돌아오게하기
            UIManager.inst.BattleUiSetting();
            PlayerInfo.Inst.PickRound = false;
            //플레이어 돌아오게하기
            
            
            if (PlayerInfo.Inst.PlayerOb.TryGetComponent(out PlayerMoving moving))
            {
                
                moving.nav.enabled = false;
            PlayerInfo.Inst.PlayerOb.transform.position = PlayerInfo.Inst.PlayerMovePos.position;
                
                moving.nav.enabled = true;
            }
            RoundNext();
        }

        void RoundNext()
        {
            int Round = PlayerInfo.Inst.Round;
            
            RoundStart(Round+1);
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.T))
            {
                PlayerInfo.Inst.PlayerOb.transform.position = PlayerInfo.Inst.PlayerMovePos.position;
            }
        }
    }
}
