using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameS
{
    public class PickRoundManager : MonoBehaviour
    {
        public static PickRoundManager inst;
        List<int> checkplayer = new List<int>();    
        List<int> result=new List<int>();
        List<int> result1=new List<int>();
        List<int> result2=new List<int>();
        private void Awake()
        {
            inst = this;
        }

        public void PickCreateCharacter()
        {
            
        }

        public void FirstFunc()
        {
            //카드생성
            if (PlayerInfo.Inst.RoundIdx == 1000)
            {
                FirstCardCreate();
            }
            else
            {
                CardCreate();
            }
            
            //체크
            StartCoroutine(IFirstFunc());
        }

        public void PickFunc()
        {
            CardCreate();
            StartCoroutine(IPickFunc());
        }

        void FirstCardCreate()
        {
            result.Clear();
            result = GameSystem_AllInfo.inst.CardPickCnt(2, 9);
            List<int> resultitem = ItemRandom(0);

            for (int i = 0; i < 9; i++)
            {
                Vector3 pos;

                float angle = (i+1) * 40; 
                float x = Mathf.Cos(angle*Mathf.Deg2Rad)*6 ;
                float z = Mathf.Sin(angle*Mathf.Deg2Rad)*6 ;
                pos = new Vector3(x, 1, z) + GameSystem_AllInfo.inst.PickPos.position;
                Quaternion qu=Quaternion.Euler(0,170-40*i,0);
                GameObject card= PhotonNetwork.Instantiate(GameSystem_AllInfo.inst.Cards[result[i]], pos, qu);
                if (card.TryGetComponent(out Card_Info info))
                {
                    info.PickStart(i,Random.Range(0,8));
                }
                MasterInfo.inst.pickCards.Add(card);
            }
        }

        List<int> ItemRandom(int cnt)
        {
            result1.Clear();
            result2.Clear();
            
            result1 = new List<int>();
            result2 = new List<int>();

            for (int i = 0; i < 9; i++)
            {
                result.Add(Random.Range(0,9));
                result2.Add(i);
            }

            for (int i = 0; i < cnt; i++)
            {
                int ran = Random.Range(0, result2.Count);
                int ranidx = result2[ran];
                result2.RemoveAt(ran);
                result[ranidx] = Random.Range(9, 54);
            }

            return result;

        }

        void CardCreate()
        {
            result.Clear();
            
            List<int> resultitem=null;
            
            List<int> resultcheck1=null;
            List<int> resultcheck2=null;
            List<int> resultcheck3=null;
            List<int> resultcheck4=null;
            List<int> resultcheck5=null;
            if (PlayerInfo.Inst.RoundIdx==1001)
            {
                resultcheck1 = GameSystem_AllInfo.inst.CardPickCnt(1, 3);
                resultcheck2 = GameSystem_AllInfo.inst.CardPickCnt(2, 6);
                resultcheck3 = GameSystem_AllInfo.inst.CardPickCnt(3, 0);
                resultcheck4 = GameSystem_AllInfo.inst.CardPickCnt(4, 0);
                resultcheck5 = GameSystem_AllInfo.inst.CardPickCnt(5, 0);
                resultitem = ItemRandom(0);
            }
            else if (PlayerInfo.Inst.RoundIdx==1002)
            {
                resultcheck1 = GameSystem_AllInfo.inst.CardPickCnt(1, 2);
                resultcheck2 = GameSystem_AllInfo.inst.CardPickCnt(2, 5);
                resultcheck3 = GameSystem_AllInfo.inst.CardPickCnt(3, 2);
                resultcheck4 = GameSystem_AllInfo.inst.CardPickCnt(4, 0);
                resultcheck5 = GameSystem_AllInfo.inst.CardPickCnt(5, 0);
                resultitem = ItemRandom(0);
            }
            else if (PlayerInfo.Inst.RoundIdx==1003)
            {
                resultcheck1 = GameSystem_AllInfo.inst.CardPickCnt(1, 2);
                resultcheck2 = GameSystem_AllInfo.inst.CardPickCnt(2, 4);
                resultcheck3 = GameSystem_AllInfo.inst.CardPickCnt(3, 3);
                resultcheck4 = GameSystem_AllInfo.inst.CardPickCnt(4, 0);
                resultcheck5 = GameSystem_AllInfo.inst.CardPickCnt(5, 0);
                resultitem = ItemRandom(1);
            }
            else if (PlayerInfo.Inst.RoundIdx==1004)
            {
                resultcheck1 = GameSystem_AllInfo.inst.CardPickCnt(1, 1);
                resultcheck2 = GameSystem_AllInfo.inst.CardPickCnt(2, 4);
                resultcheck3 = GameSystem_AllInfo.inst.CardPickCnt(3, 3);
                resultcheck4 = GameSystem_AllInfo.inst.CardPickCnt(4, 1);
                resultcheck5 = GameSystem_AllInfo.inst.CardPickCnt(5, 0);
                resultitem = ItemRandom(3);
            }
            else if (PlayerInfo.Inst.RoundIdx==1005)
            {
                resultcheck1 = GameSystem_AllInfo.inst.CardPickCnt(1, 1);
                resultcheck2 = GameSystem_AllInfo.inst.CardPickCnt(2, 1);
                resultcheck3 = GameSystem_AllInfo.inst.CardPickCnt(3, 3);
                resultcheck4 = GameSystem_AllInfo.inst.CardPickCnt(4, 3);
                resultcheck5 = GameSystem_AllInfo.inst.CardPickCnt(5, 1);
                resultitem = ItemRandom(3);
            }
            else if (PlayerInfo.Inst.RoundIdx==1006)
            {
                resultcheck1 = GameSystem_AllInfo.inst.CardPickCnt(1, 1);
                resultcheck2 = GameSystem_AllInfo.inst.CardPickCnt(2, 1);
                resultcheck3 = GameSystem_AllInfo.inst.CardPickCnt(3, 3);
                resultcheck4 = GameSystem_AllInfo.inst.CardPickCnt(4, 2);
                resultcheck5 = GameSystem_AllInfo.inst.CardPickCnt(5, 2);
                resultitem = ItemRandom(9);
            }

            for (int i = 0; i < resultcheck1.Count; i++)
            {
                result.Add(resultcheck1[i]);
            }
            for (int i = 0; i < resultcheck2.Count; i++)
            {
                result.Add(resultcheck2[i]);
            }
            for (int i = 0; i < resultcheck3.Count; i++)
            {
                result.Add(resultcheck3[i]);
            }
            for (int i = 0; i < resultcheck4.Count; i++)
            {
                result.Add(resultcheck4[i]);
            }
            for (int i = 0; i < resultcheck5.Count; i++)
            {
                result.Add(resultcheck5[i]);
            }
            
            
            

            for (int i = 0; i < 9; i++)
            {
                int ran = Random.Range(0, result.Count);
                int createidx = result[ran];
                result.RemoveAt(ran);


                Vector3 pos;

                float angle = (i+1) * 40; 
                float x = Mathf.Cos(angle*Mathf.Deg2Rad)*6 ;
                float z = Mathf.Sin(angle*Mathf.Deg2Rad)*6 ;
                pos = new Vector3(x, 1, z) + GameSystem_AllInfo.inst.PickPos.position;
                Quaternion qu=Quaternion.Euler(0,170-40*i,0);
                GameObject card= PhotonNetwork.Instantiate(GameSystem_AllInfo.inst.Cards[createidx], pos, qu);
                if (card.TryGetComponent(out Card_Info info))
                {
                    info.PickStart(i,resultitem[i]);
                }
                MasterInfo.inst.pickCards.Add(card);
            }
        }

        IEnumerator IFirstFunc()
        {
            int TimeWait = 15;
            
            //3초뒤에 다 염
            
            yield return YieldInstructionCache.WaitForSeconds(3);
            
            NetworkManager.inst.PickAllOpen();
            //열기
            while (true)
            {
                if (TimeWait==0)
                {
                    break;
                }
                //다 선택했는지 체크하기
                bool check = false;
                for (int i = 0; i < NetworkManager.inst.players.Count; i++)
                {
                    if (NetworkManager.inst.players[i].State==1)
                    {
                        if (MasterInfo.inst.player_PickCheck[i]==1)
                        {
                            check = true;
                            break;
                        }
                    }
                }

                if (check==false)
                {
                    break;
                }

                TimeWait--;
                yield return YieldInstructionCache.WaitForSeconds(1);
            }

            NetworkManager.inst.NoSelectPickFunc();
            
            
            //라운드끝내기
            
            yield return YieldInstructionCache.WaitForSeconds(1);
            NetworkManager.inst.RoundFuncGo(3);
            yield return YieldInstructionCache.WaitForSeconds(1);
            //모두삭제
            for (int i = 0; i < MasterInfo.inst.pickCards.Count; i++)
            {
                PhotonNetwork.Destroy(MasterInfo.inst.pickCards[i]);
            }
            MasterInfo.inst.pickCards.Clear();

        }
        IEnumerator IPickFunc()
        {
            int TimeWait = 15;
            //3초뒤에 다 염
            
            yield return YieldInstructionCache.WaitForSeconds(3);

            MasterInfo.inst.LifeOrder();
            checkplayer.Clear();
            for (int i = 0; i < MasterInfo.inst.lifeRank.Count; i++)
            {
                int idx = MasterInfo.inst.lifeRank[i].PlayerIdx;
                if (!NetworkManager.inst.players[idx].Dead)
                {
                    checkplayer.Add(idx);
                }
            }

            int ch = 2;
            if (checkplayer.Count<=3)
            {
                ch = 1;
            }
            while (checkplayer.Count>0)
            {
                for (int i = 0; i < ch; i++)
                {
                    if (checkplayer.Count>0)
                    {
                        int Goidx = checkplayer[0];
                        checkplayer.RemoveAt(0);
                        NetworkManager.inst.PickOpen(Goidx);

                    }
                }
                yield return YieldInstructionCache.WaitForSeconds(3);
            }
            
            while (true)
            {
                if (TimeWait==0)
                {
                    break;
                }
                //다 선택했는지 체크하기
                bool check = false;
                for (int i = 0; i < NetworkManager.inst.players.Count; i++)
                {
                    if (NetworkManager.inst.players[i].State==1)
                    {
                        if (MasterInfo.inst.player_PickCheck[i]==1)
                        {
                            check = true;
                            break;
                        }
                    }
                }

                if (check==false)
                {
                    break;
                }

                TimeWait--;
                yield return YieldInstructionCache.WaitForSeconds(1);
            }

            NetworkManager.inst.NoSelectPickFunc();
            
            
            //라운드끝내기
            
            yield return YieldInstructionCache.WaitForSeconds(1);
            NetworkManager.inst.RoundFuncGo(3);
            yield return YieldInstructionCache.WaitForSeconds(1);
            //모두삭제
            for (int i = 0; i < MasterInfo.inst.pickCards.Count; i++)
            {
                PhotonNetwork.Destroy(MasterInfo.inst.pickCards[i]);
            }
            MasterInfo.inst.pickCards.Clear();

        }

        public void PickAllOpen()
        {
            for (int i = 0; i < 8; i++)
            {
                GameSystem_AllInfo.inst.pickNoMove[i].SetActive(false);
            }
        }
        public void PickAllClose()
        {
            for (int i = 0; i < 8; i++)
            {
                GameSystem_AllInfo.inst.pickNoMove[i].SetActive(true);
            }
        }
        public void PickOpen(int idx)
        {
            GameSystem_AllInfo.inst.pickNoMove[idx].SetActive(false);
        }
        
    }
}
