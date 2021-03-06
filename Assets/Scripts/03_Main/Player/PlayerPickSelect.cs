using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace GameS
{
    public class PlayerPickSelect : MonoBehaviourPunCallbacks
    {
        [SerializeField] private PhotonView pv;
         public GameObject pickOb;
         [SerializeField] private Transform pos;
         private Camera cam;

         private void Start()
         {
             cam = Camera.main;
         }

         private void OnTriggerEnter(Collider other)
        {
            if (!pv.IsMine)
            {
                return;
            }
            if (PlayerInfo.Inst.PickRound&&PlayerInfo.Inst.IsPick&&other.transform.CompareTag("Cards"))
            {
                if (other.transform.TryGetComponent(out Card_Info info))
                {
                    if (info.IsPick)
                    {
                        PickOb(other.gameObject);
                        
                    }
                }


            }
            if (other.transform.CompareTag("Item"))
            {
                if (other.transform.TryGetComponent(out iteminfo info))
                {
                    if (!info.IsPick) return;

                    if (!Equals(info.pv.Owner, pv.Owner)) return;

                    if (info.IsCoin)
                    {
                        PlayerInfo.Inst.Gold++;
                    }
                    else if(info.IsItem)
                    {
                        if (info.idx>=1000)
                        {
                            
                        }
                        else
                        {
                            
                            
                            GameObject ob = ObjectPooler.SpawnFromPool("ItemMoveEffect",cam.WorldToScreenPoint(other.transform.position));
                            ob.GetComponent<ItemMoveInfo>().GoGo(info.idx);
                        }
                    }
                    EffectManager.inst.EffectCreate("ItemGetEffect",other.transform.position,Quaternion.identity,3f);
                    PhotonNetwork.Destroy(other.gameObject);
                }


            }
        }

        public void PickOb(GameObject ob)
        {
            PlayerInfo.Inst.IsPick = false;
            pickOb = ob;
            if (ob.TryGetComponent(out Card_Info info))
            {
                info.pv.TransferOwnership(PhotonNetwork.LocalPlayer);
                NetworkManager.inst.PickSelect();
                info.PickSelect();
                
                
                
            }

            StartCoroutine(CardMove());
        }

        IEnumerator CardMove()
        {
            while (true)
            {
                if (pickOb!=null)
                {
                    pickOb.transform.position = pos.position;
                }
                yield return null;
            }
        }


    }
}
