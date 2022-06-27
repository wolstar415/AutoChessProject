using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameS
{
    public class LoadingManager : MonoBehaviourPunCallbacks
    {
        public List<LoadingInfo> Infos;
        public PhotonView pv;

        private void Start()
        {
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                if (PhotonNetwork.PlayerList[i]==PhotonNetwork.LocalPlayer)
                {
                    pv.RPC(nameof(ChangePlayerinfo),RpcTarget.All,i,GameManager.inst.CharIdx,GameManager.inst.NickName);
                    break;
                }
            }

            if (PhotonNetwork.IsMasterClient)
            {
                StartCoroutine(LoadingFunc());
            }
        }

        IEnumerator LoadingFunc()
        {
            AsyncOperation op =  SceneManager.LoadSceneAsync("03_Main");
            op.allowSceneActivation = false;
            
            while (!op.isDone)
            {
                
                yield return null;
                if (op.progress < 0.9f)
                {

                }
                else
                {

                    yield return YieldInstructionCache.WaitForSeconds(1);
                    op.allowSceneActivation = true;
                    //PhotonNetwork.LoadLevel("03_Main");
                        yield break;
                    
                }
            }
        }

        [PunRPC]
        void ChangePlayerinfo(int idx, int icon, string name)
        {
            Infos[idx].IconSet(icon);
            Infos[idx].NickNameSet(name);
            Infos[idx].gameObject.SetActive(true);
        }
    }
}
