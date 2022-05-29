using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Unity.VisualScripting;


public enum PlayerClickState
{
    None = 0,
    Card = 1,
    item,
}

public enum PlayerClickState2
{
    None = 0,
    cardDraging = 1,
    itemDraging
}


    

public class ClickManager : MonoBehaviour
{
    public static ClickManager inst;
    public LayerMask laymaskTile;
    public LayerMask laymaskCard;
    [SerializeField] private GameObject ClickCard;
    [SerializeField] private Vector3 MousePos;
    public PlayerClickState clickstate;
    public PlayerClickState2 clickstate2;

    public GameObject TileOb;
    private bool DragStart = false;

    private void Awake()
    {
        inst = this;
    }

    private void Start()
    {
        
        clickstate = PlayerClickState.None;
        clickstate2 = PlayerClickState2.None;
        
        this.UpdateAsObservable()
            .Where(_=>Input.GetMouseButtonDown(0))
            .Select(_ =>
            {
                Vector3 mpos = Input.mousePosition;
                Ray cast = Camera.main.ScreenPointToRay(mpos);
                RaycastHit hit;
                if (Physics.Raycast(cast, out hit, 100, laymaskCard))
                {
                    
                }
                return hit;
            })
            .Where(hit=>hit.collider!=null)
            .Subscribe(hit =>
            {
                ClickCardFunc(hit.collider.gameObject);
            });
        this.UpdateAsObservable()
            .Where(_=>ClickCard!=null&&Input.GetMouseButton(0)&&clickstate==PlayerClickState.Card)
            .Subscribe(_ =>
            {
                DragCardFunc();
            });
        this.UpdateAsObservable()
            .Where(_ => ClickCard != null &&Input.GetMouseButtonUp(0))
            .Subscribe(_ =>
            {
                UpCardFunc();
            });

    }


    void ClickCardFunc(GameObject ob)
    {
        ClickCard = ob;
        MousePos = ob.transform.position;
        clickstate = PlayerClickState.Card;
               
    }

    void DragCardFunc()
    {
        Vector3 scrSpace = Camera.main.WorldToScreenPoint(ClickCard.transform.position);


               
        Vector3 MousePos = new Vector3(Input.mousePosition.x,
            Input.mousePosition.y, scrSpace.z);
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(MousePos);
        objPosition.y = 1.6f;
        if (clickstate2!=PlayerClickState2.cardDraging)
        {
            if (Vector3.Distance(ClickCard.transform.position,objPosition)>=1)
            {
                clickstate2 = PlayerClickState2.cardDraging;
                ClickCard.GetComponent<CapsuleCollider>().enabled = false;
                PlayerInfo.Inst.PlayerTileOb.SetActive(true);
                PlayerInfo.Inst.FiledTileOb.SetActive(true);
            }
        }
        else
        {
            
            ClickCard.transform.position = objPosition;
        }
        
        
    }

    void UpCardFunc()
    {
        if (ClickCard==null)
        {
            return;
        }
        ClickCard.GetComponent<CapsuleCollider>().enabled = true;
        clickstate = PlayerClickState.None;
        clickstate2 = PlayerClickState2.None;
        if (TileOb!=null)
        {
            int idx = TileOb.GetComponent<TileInfo>().Idx;
            ClickCard.GetComponent<Card_Info>().MoveIdx = idx;
            TileOb.GetComponent<TileInfo>().SetColor();
            TileOb.GetComponent<TileInfo>().tileGameob = ClickCard;
            ClickCard.GetComponent<Card_Info>().IsFiled = TileOb.GetComponent<TileInfo>().IsFiled;

        }
        MousePos=Vector3.zero;
        PlayerInfo.Inst.PlayerTileOb.SetActive(false);
        PlayerInfo.Inst.FiledTileOb.SetActive(false);
        GameObject MoveOb;
        int MoveIdx = ClickCard.GetComponent<Card_Info>().MoveIdx;
        if (ClickCard.GetComponent<Card_Info>().IsFiled)
        {
            MoveOb = PlayerInfo.Inst.FiledTile[MoveIdx];
        }
        else
        {
            MoveOb = PlayerInfo.Inst.PlayerTile[MoveIdx];
        }

        Vector3 pos = MoveOb.transform.position;
        pos.y = 1.6f;
        ClickCard.transform.position = pos;
        ClickCard = null;
    }

    void Update()
    {
        // if (Input.GetMouseButtonDown(0))
        // {
        //     
        //     Vector3 mpos = Input.mousePosition;
        //     Ray cast = Camera.main.ScreenPointToRay(mpos);
        //     RaycastHit hit;
        //
        //
        //     if (Physics.Raycast(cast, out hit, 1000, laymaskCard))
        //     {
        //         ClickCardFunc(hit.transform.gameObject);
        //     }
        // }
        // if(Input.GetMouseButton(0)&&clickstate==PlayerClickState.Card&&ClickCard!=null)
        // {
        //     DragCardFunc();
        // }
        //
        // if (Input.GetMouseButtonUp(0))
        // {
        //     UpCardFunc();
        // }
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