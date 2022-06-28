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
            PlayerInfo.Inst.PlayerTile = PositionManager.inst.playerPositioninfo[Enemyidx].EnemyPlayerTile;
            
            EventManager.inst.Sub_CardBattleMove.OnNext(1);
            NetworkManager.inst.GoldSet(PlayerInfo.Inst.EnemyIdx,PlayerInfo.Inst.Gold);
            CameraMoveFunc2();

            PlayerInfo.Inst.PlayerOb.GetComponent<PlayerMoving>().nav.enabled = false;
            PlayerInfo.Inst.PlayerOb.transform.position =
                PositionManager.inst.playerPositioninfo[Enemyidx].EnemyPlayerMovePos.position;
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
            PlayerInfo.Inst.EnemyFiledTile = PositionManager.inst.playerPositioninfo[pidx].EnemyFiledTile;
            PlayerInfo.Inst.PlayerTileOb = PositionManager.inst.playerPositioninfo[pidx].PlayerTileob;
            PlayerInfo.Inst.PlayerTile= PositionManager.inst.playerPositioninfo[pidx].PlayerTile;
            EventManager.inst.Sub_CardBattleMove.OnNext(2);
            PlayerInfo.Inst.EnemyIdx = -1;
            PlayerInfo.Inst.BattleMove = false;
            PlayerInfo.Inst.PlayerOb.GetComponent<PlayerMoving>().nav.enabled = false;
            PlayerInfo.Inst.PlayerOb.transform.position =
                PositionManager.inst.playerPositioninfo[pidx].PlayerMovePos.position;
            PlayerInfo.Inst.PlayerOb.GetComponent<PlayerMoving>().nav.enabled = true;
            CameraMoveFunc1();
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
            
            int Enemyidx = PlayerInfo.Inst.EnemyIdx;
            if (Enemyidx==-1||Enemyidx==10)
            {
                return;
            }
            for (int i = 0; i <5; i++)
            {

                    GameObject ob = PositionManager.inst.playerPositioninfo[Enemyidx].EnemyGoldOb[i];
                    if (ob.TryGetComponent(out MeshRenderer mesh))
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
            PlayerInfo.Inst.camer.transform.position = PositionManager.inst.Camera_AttackPos[idx].localPosition;
            PlayerInfo.Inst.camer.transform.rotation = PositionManager.inst.Camera_AttackPos[idx].localRotation;
        }

        public void RoundStart(int Round)
        {
            
            int RoundCheck = CsvManager.inst.RoundCheck3[Round];
            PlayerInfo.Inst.RoundIdx = RoundCheck;
            PlayerInfo.Inst.Round = Round;
            PlayerInfo.Inst.BattleEnd = false;
            int TimeWait = CsvManager.inst.RoundCheck4[Round];
            
            
            EventManager.inst.Sub_LevelUpCheck.OnNext(1); // 레벨업할수있는애들 ㄱㄱ
            

            
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
            int TimeWait = CsvManager.inst.RoundCheck4[PlayerInfo.Inst.Round];
            if (PlayerInfo.Inst.Dead == false)
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
            


            if (PhotonNetwork.IsMasterClient)
            {
                MasterInfo.inst.MasterPvPStart(TimeWait);
                MasterInfo.inst.MasterBattleRoundReady();
                
            }
            
            
            
        }
         void Round_PVE_StartFunc()
        {
                int TimeWait = CsvManager.inst.RoundCheck4[PlayerInfo.Inst.Round];
            if (PlayerInfo.Inst.Dead == false)
            {
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

            int idxcheck = PlayerInfo.Inst.RoundIdx;
            int idx = PlayerInfo.Inst.PlayerIdx;
            if (PlayerInfo.Inst.Dead == false)
            {
                
            
            
            PlayerInfo.Inst.PickRound = true;
            //UI설정
            UIManager.inst.PickUiSetting();
            

            //카메라이동
            
            
            if (PlayerInfo.Inst.PlayerOb.TryGetComponent(out PlayerMoving pl))
            {
                //플레이어이동
                pl.MovePos(PositionManager.inst.PickPos[idx].position);
            }
            //길막기 모두 정상
            }
            PlayerInfo.Inst.camer.transform.SetPositionAndRotation(PositionManager.inst.Camera_PickPos[idx].localPosition,PositionManager.inst.Camera_PickPos[idx].localRotation);

            PickRoundManager.inst.PickAllClose();
            if (PhotonNetwork.IsMasterClient)
            {
                //마스터에게 액션실행
                //카드들 생성
                MasterInfo.inst.PickPlayerCheck();
                MasterInfo.inst.PickCardCreate(idxcheck);
            }
            
            
        }
        
        public void Round_PVP_EndFunc()
        {

            if (PlayerInfo.Inst.Dead == false)
            {
                
            
                BattleEndC();
                UIManager.inst.BattleUiSetting();
                UIManager.inst.BattleEndUi();
                MoneyCheck(true);
                PlayerInfo.Inst.XpPlus(2);
            }
            
            if (PhotonNetwork.IsMasterClient)
            {
                NetworkManager.inst.MasterRoundNextStart();
            }

            
        }

        void BattleEndC()
        {
            PlayerInfo.Inst.IsBattle = false;
            PlayerInfo.Inst.IsCopy = false;
            PlayerInfo.Inst.EnemyIdx = -1;
            PlayerInfo.Inst.copydeadCnt = -1;
            
        }
        public void Round_PVE_EndFunc()
        {
            
            if (PlayerInfo.Inst.Dead == false)
            {
                

                BattleEndC();
                UIManager.inst.BattleUiSetting();
                UIManager.inst.BattleEndUi();
                PlayerInfo.Inst.EnemyIdx = -1;
                MoneyCheck(false);
                PlayerInfo.Inst.XpPlus(2);
            }
            
            if (PhotonNetwork.IsMasterClient) NetworkManager.inst.MasterRoundNextStart();
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
            if (PlayerInfo.Inst.Dead == false)
            {
                
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
                    // moving.nav.enabled = false;
                    // moving.gameObject.transform.position = PlayerInfo.Inst.PlayerMovePos.position;
                    // moving.nav.enabled = true;
                    moving.MovePos(PlayerInfo.Inst.PlayerMovePos.position);
                }
                
            }
            
            
            if (PhotonNetwork.IsMasterClient) NetworkManager.inst.MasterRoundNextStart();

        }

        void RoundNext()
        {
            int Round = PlayerInfo.Inst.Round;
            
            RoundStart(Round+1);
        }

    }
}
