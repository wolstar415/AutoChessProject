using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TileInfo : MonoBehaviour
{
    public Color[] colors;
    private void OnMouseEnter()
    {
        transform.GetComponent<MeshRenderer>().material.color = colors[1];
    }

    private void OnMouseExit()
    {
        transform.GetComponent<MeshRenderer>().material.color = colors[0];
    }
}
