using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Realtime;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Unity.VisualScripting;
using UnityEngine.Tilemaps;




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
    const float CharaterHe = 1.6f;
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
        objPosition.y = CharaterHe;
        if (clickstate2!=PlayerClickState2.cardDraging)
        {
            if (Vector3.Distance(ClickCard.transform.position,objPosition)>=1)
            {
                clickstate2 = PlayerClickState2.cardDraging;
                
                if (ClickCard.TryGetComponent(out CapsuleCollider col))
                {
                    col.enabled = false;
                }
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
        if (ClickCard.TryGetComponent(out CapsuleCollider col))
        {
            col.enabled = true;
        }

        if (clickstate==PlayerClickState.Card&&clickstate2!=PlayerClickState2.cardDraging)
        {
            // 드래그안함 아무것도안하게
            
            ShowCharacterInfo(ClickCard);
            return;
        }
        clickstate = PlayerClickState.None;
        clickstate2 = PlayerClickState2.None;
        if (TileOb!=null)
        {
            if (TileOb.TryGetComponent(out TileInfo info))
            {
                info.SetColor();
                if (info.tileGameob==null)
                {
                    CharacterTileMove(ClickCard, TileOb);
                }
                else
                {
                    CharacterTileChange(ClickCard,info.tileGameob);
                }
            }
        }
        else
        {
            CharacterTileMoveReset(ClickCard);
        }
        MousePos=Vector3.zero;
        PlayerInfo.Inst.PlayerTileOb.SetActive(false);
        PlayerInfo.Inst.FiledTileOb.SetActive(false);
        
            

        
        ClickCard = null;
    }
    void CharacterTileMove(GameObject ob,GameObject Tile)
    {
        var tilecom = Tile.GetComponent<TileInfo>();
        var ClickCardcom = ob.GetComponent<Card_Info>();


        if (ClickCardcom.IsFiled)
        {
            if (!tilecom.IsFiled)
            {
                PlayerInfo.Inst.food--;
            }
        }
        else
        {
            if (tilecom.IsFiled)
            {
                int food = ClickCardcom.Food;
                if (PlayerInfo.Inst.food+food >PlayerInfo.Inst.foodMax)
                {
                    CharacterTileMoveReset(ob);
                    return;
                }

                PlayerInfo.Inst.food++;
            }
        }
        

        int idx = tilecom.Idx;
        GameObject oriTile = ClickCardcom.TileOb;
        if (oriTile.TryGetComponent(out TileInfo info))
        {
            info.tileGameob = null;
        }

        CharacterTileMoveFunc(ob, Tile);
    }

    void CharacterTileMoveFunc(GameObject ob,GameObject Tile)
    {
        var ClickCardcom = ob.GetComponent<Card_Info>();
        var tilecom = Tile.GetComponent<TileInfo>();
        int idx = tilecom.Idx;
        GameObject oriTile = ClickCardcom.TileOb;
        if (oriTile!=null)
        {
            if (oriTile.TryGetComponent(out TileInfo info))
            {
                info.tileGameob = null;
            }
        }
        ClickCardcom.MoveIdx = idx;
        tilecom.tileGameob = ob;
        ClickCardcom.TileOb = Tile;
        ClickCardcom.IsFiled = tilecom.IsFiled;
        Vector3 pos = Tile.transform.position;
        pos.y = CharaterHe;
        ob.transform.position = pos;
    }
    void CharacterTileMoveReset(GameObject ob)
    {
        var ClickCardcom = ob.GetComponent<Card_Info>();
        
        Vector3 pos = ClickCardcom.TileOb.transform.position;
        pos.y = CharaterHe;
        ob.transform.position = pos;
    }

    void CharacterTileChange(GameObject ob1, GameObject ob2)
    {
        var obcom1 = ob1.GetComponent<Card_Info>();
        var obcom2 = ob2.GetComponent<Card_Info>();
        GameObject Tile1 = obcom1.TileOb;
        GameObject Tile2 = obcom2.TileOb;
        if (obcom1.IsFiled)
        {
            if (!obcom2.IsFiled)
            {
            int food = PlayerInfo.Inst.food - obcom1.Food +obcom2.Food;
                if (food > PlayerInfo.Inst.foodMax)
                {
                    CharacterTileMoveReset(ClickCard);
                    return;
                }

                PlayerInfo.Inst.food = food;
            }

        }
        else
        {
            if (obcom2.IsFiled)
            {
                int food = PlayerInfo.Inst.food + obcom1.Food -obcom2.Food;
                if (food > PlayerInfo.Inst.foodMax)
                {
                    CharacterTileMoveReset(ClickCard);
                    return;
                }

                PlayerInfo.Inst.food = food;
            }
        }


        obcom1.TileOb = Tile2;
        obcom2.TileOb = Tile1;
        CharacterTileMoveFunc(ob1,Tile2);
        CharacterTileMoveFunc(ob2,Tile1);
        

    }

    void ShowCharacterInfo(GameObject ob)
    {
        
    }
    /*void Update()
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
    */
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