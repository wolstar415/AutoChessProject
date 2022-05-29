using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TileInfo : MonoBehaviour
{
    public Color[] colors;
    public GameObject tileGameob;
    public int Idx;
    public bool IsFiled;
    private void OnMouseEnter()
    {
        if (ClickManager.inst.clickstate2 == PlayerClickState2.cardDraging)
        {
            transform.GetComponent<MeshRenderer>().material.color = colors[1];
            ClickManager.inst.TileOb = gameObject;
        }
        
    }

    private void OnMouseExit()
    {
        if (ClickManager.inst.clickstate2 == PlayerClickState2.cardDraging)
        {
        transform.GetComponent<MeshRenderer>().material.color = colors[0];
        ClickManager.inst.TileOb = null;
        }
    }

    public void SetColor()
    {
        transform.GetComponent<MeshRenderer>().material.color = colors[0];
    }
}
