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

            PlayerInfo.Inst.PlayerOb.GetComponent<PlayerMoving>().nav.enabled = false;
            PlayerInfo.Inst.PlayerOb.transform.position =
                PositionManager.inst.playerPositioninfo[Enemyidx].PlayerMovePos.position;
            PlayerInfo.Inst.PlayerOb.GetComponent<PlayerMoving>().nav.enabled = true;



        }

        //원래대로 이동하기
        public void BattleMoveFunc2()
        {
            GoldObReSet();
            if (PlayerInfo.Inst.EnemyIdx == -1 || PlayerInfo.Inst.BattleMove == false)
            {
                EventManager.inst.Sub_CardBattleMove.OnNext(2);
                return;
            }
            
            int pidx = PlayerInfo.Inst.PlayerIdx;
            EventManager.inst.Sub_CardBattleMove.OnNext(2);
            PlayerInfo.Inst.PlayerTileOb = PositionManager.inst.playerPositioninfo[pidx].PlayerTileob;
            CameraMoveFunc1();
            PlayerInfo.Inst.EnemyIdx = -1;
            PlayerInfo.Inst.BattleMove = false;
            PlayerInfo.Inst.PlayerOb.GetComponent<PlayerMoving>().nav.enabled = false;
            PlayerInfo.Inst.PlayerOb.transform.position =
                PositionManager.inst.playerPositioninfo[pidx].PlayerMovePos.position;
            PlayerInfo.Inst.PlayerOb.GetComponent<PlayerMoving>().nav.enabled = true;
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
            PlayerInfo.Inst.BattleEnd = false;
            int TimeWait = CsvManager.inst.RoundCheck4[Round];

            
            if (PlayerInfo.Inst.Round==2)
            {
                UIManager.inst.BattleUiSetting();
            }
            if (PlayerInfo.Inst.Round==4)
            {
                UIManager.inst.RoundChange();
            }
            
            
            
            if (RoundCheck<100)
            {
                Round_PVE();
                UIManager.inst.TimeFunc(TimeWait);
                PlayerInfo.Inst.PVP = false;
                
                
            }
            else if (RoundCheck<1000)
            {
                Round_PVP();
                UIManager.inst.TimeFunc(TimeWait);
                PlayerInfo.Inst.PVP = true;
            }
            else
            {
                Round_Pick();
            }
            UIManager.inst.RoundSet();
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
            int TimeWait = CsvManager.inst.RoundCheck4[PlayerInfo.Inst.Round];
            if (PlayerInfo.Inst.IsLock)
            {
                UIManager.inst.LockCheck(false);
            }
            else
            {
            CardManager.inst.CardReset();
                
            }

            if (PhotonNetwork.IsMasterClient)
            {
                MasterInfo.inst.MasterBattleRoundReady();
            }
            
            
            
        }
         void Round_PVE_StartFunc()
        {
            if (PlayerInfo.Inst.Dead == true) return;
            int TimeWait = CsvManager.inst.RoundCheck4[PlayerInfo.Inst.Round];
            PlayerInfo.Inst.EnemyIdx = 10;
            if (PlayerInfo.Inst.Round>=2)
            {
                if (PlayerInfo.Inst.IsLock)
                {
                    UIManager.inst.LockCheck(false);

                }
                else
                {
                    CardManager.inst.CardReset();
                
                }
            }
            PVEManager.inst.PVEstart();
            if (PhotonNetwork.IsMasterClient)
            {
                MasterInfo.inst.MasterPveStart(TimeWait);
                MasterInfo.inst.MasterBattleRoundReady();
            }

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
            PlayerInfo.Inst.camer.transform.SetPositionAndRotation(PositionManager.inst.Camera_PickPos[idx].localPosition,PositionManager.inst.Camera_PickPos[idx].localRotation);
            //플레이어이동
            PlayerInfo.Inst.PlayerOb.transform.position = PositionManager.inst.PickPos[idx].localPosition;
            //길막기 모두 정상
            PickRoundManager.inst.PickAllClose();
            
            
        }
        
        public void Round_PVP_EndFunc()
        {
            if (PlayerInfo.Inst.Dead == true) return;
            PlayerInfo.Inst.IsBattle = false;
            UIManager.inst.BattleUiSetting();
            UIManager.inst.BattleEndUi();
            MoneyCheck(true);
            RoundNext();
            
        }
        public void Round_PVE_EndFunc()
        {
            if (PlayerInfo.Inst.Dead == true) return;
            PlayerInfo.Inst.IsBattle = false;
            UIManager.inst.BattleUiSetting();
            UIManager.inst.BattleEndUi();
            PlayerInfo.Inst.EnemyIdx = -1;
            MoneyCheck(false);
            RoundNext();
        }

        public void MoneyCheck(bool PVP)
        {
            int money = 0;

            int round = PlayerInfo.Inst.Round;
            int m = PlayerInfo.Inst.Gold;
            if (round<=2)
            {
                money += 2;
            }
            else if(round==3)
            {
                money += 3;
            }
            else if(round==4)
            {
                money += 4;
            }
            else
            {
                money += 5;
            }

            if (PVP)
            {
                
            

            if (PlayerInfo.Inst.IsVictory&&PlayerInfo.Inst.victoryCnt>=2)
            {
                if (PlayerInfo.Inst.victoryCnt>=5)
                {
                    money += 3;
                }
                else if (PlayerInfo.Inst.victoryCnt>=4)
                {
                    money += 2;
                }
                else
                {
                    money += 1;
                }
            }
            else if(PlayerInfo.Inst.defeatCnt>=2)
            {
                if (PlayerInfo.Inst.defeatCnt>=5)
                {
                    money += 3;
                }
                else if (PlayerInfo.Inst.defeatCnt>=4)
                {
                    money += 2;
                }
                else
                {
                    money += 1;
                }
            }
            }
            
            if (m>=50)
            {
                money += 5;
            }
            else if (m>=40)
            {
                money += 4;
            }
            else if (m>=30)
            {
                money += 3;
            }
            else if (m>=20)
            {
                money += 2;
            }
            else if (m>=10)
            {
                money += 1;
            }

            PlayerInfo.Inst.Gold += money;

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
                moving.gameObject.transform.position = PlayerInfo.Inst.PlayerMovePos.position;
                moving.nav.enabled = true;
               // moving.MovePos(PlayerInfo.Inst.PlayerMovePos.position);
            }
            RoundNext();
        }

        void RoundNext()
        {
            int Round = PlayerInfo.Inst.Round;
            
            RoundStart(Round+1);
        }

    }
}
