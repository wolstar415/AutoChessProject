using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;
using UnityEngine;

namespace GameS
{
    public class EffectManager : MonoBehaviourPunCallbacks
    {
        public static EffectManager inst;

        public PhotonView pv;
        private void Awake() => inst = this;




        public void EffectCreate(string name, Vector3 pos,Quaternion qu, float scale = 1f)
        {
            pv.RPC(nameof(RPC_EffectCreate),RpcTarget.All,name,pos,qu,scale);
        }


        [PunRPC]
        void RPC_EffectCreate(string name, Vector3 pos,Quaternion qu, float scale)
        {
            GameObject ob= ObjectPooler.SpawnFromPool(name, pos, qu);
            ob.transform.localScale = new Vector3(scale, scale, scale);
        }

        public void pveItemCreate(Vector3 pos,string name,int idx,int cnt)
        {
            
            //Random.onUnitSphere : 반경 1을 갖는 구의 표면상에서 임의의 지점을 반환합니다. (읽기 전용)



            for (int i = 0; i < cnt; i++)
            {
                Vector3 go = (UnityEngine.Random.onUnitSphere * 5f)+PlayerInfo.Inst.EnemyFiledTile[4].transform.position;
                go.y = 1.5f;
                GameObject ob = PhotonNetwork.Instantiate(name, pos, Quaternion.identity);
                if (ob.TryGetComponent(out iteminfo info))
                {
                    
                    if (name=="Coin")
                    {
                        info.StartFunc(idx);
                        
                    }
                    else
                    {
                        info.StartFunc(idx);
                        
                    }
                }
                ob.transform.DOJump(go, 5, 1, 1.5f);





            }
        }

    }
}
