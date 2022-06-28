using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

namespace GameS
{
    public class LobyUiManager : MonoBehaviourPunCallbacks
    {
        public static LobyUiManager inst;

        private void Awake()
        {
            inst = this;
        }

        public List<langTextinfo> texts;
        public Slider musicSldier;
        public TextMeshProUGUI musicText;
        public TextMeshProUGUI playText;
        public Image CharImage;
        public GameObject ReadyOb;
        public GameObject ReadyWait;
        public GameObject ReadyLoding;
        public bool IsRoom = false;
        public PhotonView pv;
        public int PlayerCnt;
        public Slider readySilder;
        public int maxplayer=8;

        [Header("데이터부분")] public TextMeshProUGUI[] playerdata;

        private void Update()
        {
            if (PhotonNetwork.IsMasterClient&&Input.GetKeyDown(KeyCode.Space))
            {
                GameStart();
            }
            else if (PhotonNetwork.IsMasterClient&&Input.GetKeyDown(KeyCode.Keypad4))
            {
                maxplayer--;
                Debug.Log($"현재 최대인원 : {maxplayer}");
            }
            else if (PhotonNetwork.IsMasterClient&&Input.GetKeyDown(KeyCode.Keypad6))
            {
                maxplayer++;
                Debug.Log($"현재 최대인원 : {maxplayer}");
            }
        }

        public void GameStart()
        {
            PhotonNetwork.LoadLevel("Loding");
        }

        private void Start()
        {
            textreset();
        }

        public void textreset()
        {
            for (int i = 0; i < texts.Count; i++)
            {
                texts[i].GetComponent<TextMeshProUGUI>().text = CsvManager.inst.GameText(texts[i].Idx);
            }
        }

        public void ExitBtn()
        {
            if (IsRoom)
            {
                
            PhotonNetwork.LeaveRoom();
            IsRoom = false;
            StopAllCoroutines();
            }
        }

        public void MusicSlider()
        {
            GameManager.inst.audioSource.volume = musicSldier.value;
            float f=musicSldier.value*100;
            musicText.text = f.ToString("F0");
        }

        public void LangugeSet(int idx)
        {
            switch (idx)
            {
                case 0:
                    GameManager.inst.GameLanguage = "Korean";
                    break;
                case 1:
                    GameManager.inst.GameLanguage = "Japan";
                    break;
                case 2:
                    GameManager.inst.GameLanguage = "China";
                    break;
                case 3:
                    GameManager.inst.GameLanguage = "English";
                    break;
                case 4:
                    GameManager.inst.GameLanguage = "fra";
                    break;
                case 5:
                    GameManager.inst.GameLanguage = "Germany";
                    break;
                case 6:
                    GameManager.inst.GameLanguage = "tur";
                    break;
                case 7:
                    GameManager.inst.GameLanguage = "rus";
                    break;
                default:
                    break;
            }

            textreset();


        }

        public void LobyPlayBtn()
        {
            playText.text = CsvManager.inst.GameText(523);
            CharImage.sprite= GameManager.inst.charIcons[GameManager.inst.CharIdx];
        }

        public void CharChange(int idx)
        {
            GameManager.inst.CharIdx = idx;
            CharImage.sprite = GameManager.inst.charIcons[idx];
            DataManager.inst.SaveData("CharIdx", idx);
        }

        public void PlayBtn()
        {
            if (IsRoom) return;
            
            ReadyLoding.SetActive(true);
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers =8;
            PhotonNetwork.JoinOrCreateRoom(PhotonNetwork.NickName+Random.Range(0,1000000), roomOptions,null);
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers =8;
            PhotonNetwork.JoinOrCreateRoom(PhotonNetwork.NickName+Random.Range(0,1000000), roomOptions,null);

        }

        public override void OnJoinedRoom()
        {

            IsRoom = true;
            
            ReadyLoding.SetActive(false);
            StartCoroutine(timefunc());

        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (PhotonNetwork.CurrentRoom.PlayerCount==maxplayer)
                {
                    pv.RPC(nameof(RPC_PlayReady),RpcTarget.All);
                    return;
                }
            }
        }

        [PunRPC]
        void RPC_PlayReady()
        {
            StopAllCoroutines();
            ReadyOb.SetActive(true);
            StartCoroutine(ReadyTime());
        }

        public void PlayOk()
        {
            StopAllCoroutines();
            ReadyOb.SetActive(false);
            ReadyWait.SetActive(true);
            pv.RPC(nameof(RPC_PlayOk),RpcTarget.MasterClient);
        }
        [PunRPC]
        void RPC_PlayOk()
        {
            PlayerCnt++;
            if (PlayerCnt>=maxplayer)
            {
                GameStart();
            }
        }

        [PunRPC]
        void RPC_NoStart()
        {
            if (IsRoom)
            {
                
            StopAllCoroutines();
            PhotonNetwork.LeaveRoom();
            playText.text = CsvManager.inst.GameText(523);
            IsRoom = false;
            ReadyOb.SetActive(false);
            ReadyWait.SetActive(false);
            }
        }

        public void NoStart()
        {
            if (IsRoom)
            {
                
            pv.RPC(nameof(RPC_NoStart),RpcTarget.All);
            }
            
        }

        IEnumerator ReadyTime()
        {
            float curtime = 0;
            float cooltime = 60f;
            while (curtime<=cooltime)
            {
                curtime += Time.deltaTime;
                float f = curtime / cooltime;
                readySilder.value = f;
                yield return null;
            }
            NoStart();
        }

        public override void OnCreatedRoom()
        {
            PlayerCnt = 0;
        }
        

        IEnumerator timefunc()
        {
            int min=0;
            int second=0;
            while (true)
            {
                second++;
                if (second==60)
                {
                    second = 0;
                    min++;
                }
                playText.text = min.ToString() + ":" + second.ToString("00");
                yield return YieldInstructionCache.WaitForSeconds(1);
            }
        }


        public void RankingSet()
        {

            playerdata[0].text = GameManager.inst.NickName;
            playerdata[1].text = GameManager.inst.Victory1.ToString();
            playerdata[2].text = GameManager.inst.Victory2.ToString();
            playerdata[3].text = GameManager.inst.Victory3.ToString();
            playerdata[4].text = GameManager.inst.Victory4.ToString();
            playerdata[5].text = GameManager.inst.Victory5.ToString();
            playerdata[6].text = GameManager.inst.Victory6.ToString();
            playerdata[7].text = GameManager.inst.Victory7.ToString();
            playerdata[8].text = GameManager.inst.Victory8.ToString();
            playerdata[9].text = GameManager.inst.Score.ToString();
            DataManager.inst.Ranking();



        }

    }
}
