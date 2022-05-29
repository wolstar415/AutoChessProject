using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerClickState
{
    None=0,
    Card=1,
    item,
}
public enum PlayerClickState2
{
    None=0,
    cardDraging=1,
    itemDraging
}
public class ClickManager : MonoBehaviour
{
    public LayerMask laymaskTile;
    public LayerMask laymaskCard;
    [SerializeField] private GameObject ClickCard;
    private PlayerClickState clickstate;
    private PlayerClickState2 clickstate2;

    private bool DragStart = false;

    private void Start()
    {
        clickstate = PlayerClickState.None;
        clickstate2 = PlayerClickState2.None;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mpos = Input.mousePosition;
            Ray cast = Camera.main.ScreenPointToRay(mpos);
            RaycastHit hit;
        

            if (Physics.Raycast(cast,out hit,100,laymaskCard))
            {
                Debug.Log(hit.transform.name);
            }
        }

        

        
        
    }
}

// Vector3 mpos = Input.mousePosition;
// Ray cast = Camera.main.ScreenPointToRay(mpos);
// RaycastHit hit;
//         
//
// if (Physics.Raycast(cast,out hit,100,laymask))
// {
//     Debug.Log(hit.transform.name);
// }