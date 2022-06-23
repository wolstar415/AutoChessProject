using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine;

namespace GameS
{
    public class TileInfo : MonoBehaviour
    {
        public Color[] colors;
        public GameObject tileGameob;
        public int Idx;
        public bool IsFiled;
        

        // private void OnMouseEnter()
        // {
        //
        //     if (ClickManager.inst.clickstate2 == PlayerClickState2.cardDraging)
        //     {
        //         transform.GetComponent<MeshRenderer>().material.color = colors[1];
        //         ClickManager.inst.TileOb = gameObject;
        //     }
        //
        // }
        //
        // private void OnMouseExit()
        // {
        //
        //     if (ClickManager.inst.clickstate2 == PlayerClickState2.cardDraging)
        //     {
        //         transform.GetComponent<MeshRenderer>().material.color = colors[0];
        //         ClickManager.inst.TileOb = null;
        //     }
        // }

        public void SetColor(bool b=true)
        {
            if (b)transform.GetComponent<MeshRenderer>().material.color = colors[0];
            else transform.GetComponent<MeshRenderer>().material.color = colors[1];
            
        }

        public void AddTile(GameObject ob)
        {
            var ClickCardcom = ob.GetComponent<Card_Info>();
            GameObject oriTile = ClickCardcom.TileOb;
            if (oriTile != null)
            {
                if (oriTile.TryGetComponent(out TileInfo info))
                {
                    info.RemoveTile();
                }
            }

            ClickCardcom.MoveIdx = Idx;
            ClickCardcom.TileOb = gameObject;
            ClickCardcom.TileCheck(IsFiled);
            if (IsFiled)
            {
                PlayerInfo.Inst.FiledTilestate[Idx] = ClickCardcom.Idx;
            }
            else
            {
                PlayerInfo.Inst.PlayerTilestate[Idx] = ClickCardcom.Idx;
            }

            tileGameob = ob;

        }

        public void RemoveTile()
        {
            if (IsFiled)
            {
                PlayerInfo.Inst.FiledTilestate[Idx] = -1;
            }
            else
            {
                PlayerInfo.Inst.PlayerTilestate[Idx] = -1;
            }

            tileGameob = null;
        }


    }
}