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

        void FirstCardCreate()
        {
            List<int> result = GameSystem_AllInfo.inst.CardPickCnt(1, 9);
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
                    info.PickStart(i,resultitem[i]);
                }
                MasterInfo.inst.pickCards.Add(card);
            }
        }

        List<int> ItemRandom(int cnt)
        {
            List<int> result = new List<int>();
            List<int> result2 = new List<int>();

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
            List<int> result=null;
            List<int> resultitem=null;
            
            if (PlayerInfo.Inst.RoundIdx==1001)
            {
                result = GameSystem_AllInfo.inst.CardPickCnt(1, 9);
                resultitem = ItemRandom(0);
            }
            
            
            

            for (int i = 0; i < 9; i++)
            {
                Vector3 pos;

                float angle = (i+1) * 40; 
                float x = Mathf.Cos(angle*Mathf.Deg2Rad)*1 ;
                float z = Mathf.Sin(angle*Mathf.Deg2Rad)*1 ;
                pos = new Vector3(x, z, 0) + GameSystem_AllInfo.inst.PickPos.position;
                GameObject card= PhotonNetwork.Instantiate(GameSystem_AllInfo.inst.Cards[result[i]], pos, Quaternion.identity);
                if (card.TryGetComponent(out Card_Info info))
                {
                    info.PickStart(i,resultitem[i]);
                }
                MasterInfo.inst.pickCards.Add(card);
            }
        }

        IEnumerator IFirstFunc()
        {
            int TimeWait = 20;
            //3초뒤에 다 염
            yield return YieldInstructionCache.WaitForSeconds(3);
            //열기
            NetworkManager.inst.PickAllOpen();
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
