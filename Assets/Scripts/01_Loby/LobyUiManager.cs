using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GameS
{
    public class LobyUiManager : MonoBehaviourPunCallbacks
    {

        public List<langTextinfo> texts;
        public Slider musicSldier;
        public TextMeshProUGUI musicText;
        public TextMeshProUGUI playText;
        public Image CharImage;
        public GameObject ReadyOb;
        public GameObject ReadyLoding;
        public bool IsRoom = false;
        public PhotonView pv;
        public int PlayerCnt;

        private void Update()
        {
            if (PhotonNetwork.IsMasterClient&&Input.GetKeyDown(KeyCode.Space))
            {
                GameStart();
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
            IsRoom = false;
            StopAllCoroutines();
            PhotonNetwork.LeaveRoom();
        }

        public void MusicSlider()
        {
            GameManager.inst.audioSource.volume = musicSldier.value;
            musicText.text = musicSldier.value.ToString("F0");
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
            
        }

        public void PlayBtn()
        {
            ReadyOb.SetActive(true);
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers =8;
            PhotonNetwork.JoinOrCreateRoom(PhotonNetwork.NickName, roomOptions,null);
        }

        public override void OnJoinedRoom()
        {
            if (!IsRoom)
            {
                
            IsRoom = true;
            ReadyOb.SetActive(true);
            ReadyLoding.SetActive(false);
            StartCoroutine(timefunc());
            }
            Debug.Log("버그확인");
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

    }
}
