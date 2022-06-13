using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Unity.VisualScripting;
using UnityEngine.AI;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;



public enum PlayerClickState
{
    None = 0,
    Card = 1,
    item,
    Ui,
}

public enum PlayerClickState2
{
    None = 0,
    cardDraging = 1,
    itemDraging
}


    

public class ClickManager : MonoBehaviourPunCallbacks
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
    public GameObject ClickItem;
    private bool DragStart = false;

    private bool EnemyClick = false;

    public GameObject Charinfoui;
    public GameObject ItemDropCard;
    public bool SellCheck = false;
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
            // .Select(_ =>
            // {
            //     Vector3 mpos = Input.mousePosition;
            //     Ray cast = Camera.main.ScreenPointToRay(mpos);
            //     RaycastHit hit;
            //     if (Physics.Raycast(cast, out hit, 100, laymaskCard))
            //     {
            //         
            //     }
            //     return hit;
            // })
            // .Where(hit=>hit.collider!=null)
            .Subscribe(hit =>
            {
                ClickFunc();
                //ClickCardFunc(hit.collider.gameObject);
            });
        this.UpdateAsObservable()
            .Where(_=>ClickCard!=null&&!EnemyClick&&Input.GetMouseButton(0)&&clickstate==PlayerClickState.Card)
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
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    void ClickFunc()
    {
        if (!GameSystem_AllInfo.inst.IsStart)
        {
            return;
        }
        if (IsPointerOverUIObject())
        {
            
            return;
        }

        
        Vector3 mpos = Input.mousePosition;
        Ray cast = Camera.main.ScreenPointToRay(mpos);
        RaycastHit hit;
        if (!PlayerInfo.Inst.PickRound)
        {
            if (Physics.Raycast(cast, out hit, 100, laymaskCard))
            {
                ClickCardFunc(hit.collider.gameObject);
                return;
            }
        }
        
        
        //플레이어 이동
        if (PlayerInfo.Inst.PlayerOb.TryGetComponent(out PlayerMoving moving))
        {
            //Vector3 objPosition = Camera.main.ScreenToWorldPoint(mpos);
            //moving.movePos(objPosition);
            moving.check1();
            if (PlayerInfo.Inst.PickRound == false)
            {

            UIManager.inst.CardUIClose();
            UIManager.inst.CardBuyUiClose();
            }
            //Debug.Log("이동중");
        }

        
    }
    void ClickCardFunc(GameObject ob)
    {
        ClickCard = ob;
        MousePos = ob.transform.position;
        clickstate = PlayerClickState.Card;
        SellCheck = false;
        if (ClickCard.GetComponent<Card_Info>().pv.Owner!=PhotonNetwork.LocalPlayer)
        {
            EnemyClick = true;
        }
               
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
                UIManager.inst.SellSet(ClickCard.GetComponent<Card_Info>().costCheck());
            }
        }
        else
        {
            
            ClickCard.transform.position = objPosition;
        }
        
        
    }

    void UpCardFunc()
    {
        
            
        EnemyClick = false;
        if (ClickCard==null)
        {
            return;
        }
           

        if (SellCheck)
        {
            
            if (ClickCard.TryGetComponent(out Card_Info info))
            {
            
                int cost = info.costCheck();
                PlayerInfo.Inst.Gold += cost;
                info.remove();
            }
            
        }
        else
        {
            
            if (ClickCard.TryGetComponent(out CapsuleCollider col))
            {
                col.enabled = true;
            }
            if (clickstate==PlayerClickState.Card&&clickstate2!=PlayerClickState2.cardDraging)
            {
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
            
            
            
        }
        MousePos=Vector3.zero;
        PlayerInfo.Inst.PlayerTileOb.SetActive(false);
        PlayerInfo.Inst.FiledTileOb.SetActive(false);
        clickstate = PlayerClickState.None;
        clickstate2 = PlayerClickState2.None;
            

        SellCheck = false;
        ClickCard = null; 
        UIManager.inst.SellClose();
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
                ClickCardcom.FiledOut();
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
                ClickCardcom.FiledIn();
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
        tilecom.AddTile(ob);
        Vector3 pos = Tile.transform.position;
        pos.y = CharaterHe;
        ob.transform.position = pos;
    }
    void CharacterTileMoveReset(GameObject ob)
    {
        var ClickCardcom = ob.GetComponent<Card_Info>();
        
        ClickCardcom.MoveReset();
        
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
                obcom1.FiledOut();
                obcom2.FiledIn();
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
                obcom1.FiledIn();
                obcom2.FiledOut();

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
        if (Charinfoui.TryGetComponent(out CardUI_Info2 info))
        {
            info.InfoSet(ClickCard);
        }

        ClickCard = null;
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