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


namespace GameS
{
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
        private Camera _camera;
        readonly float CHARACTERY = 1.6f;
        public static ClickManager inst;
        public LayerMask laymaskTile;
        public LayerMask laymaskCard;
        public GameObject ClickCard;
        [SerializeField] private Vector3 MousePos;
        public PlayerClickState clickstate;
        public PlayerClickState2 clickstate2;

        public GameObject TileOb;
        public GameObject ClickItem;

        private bool EnemyClick = false;

        public GameObject Charinfoui;
        public GameObject ItemDropCard;
        public bool SellCheck = false;
        List<RaycastResult> results = new List<RaycastResult>();

        private void Awake()
        {
            inst = this;
            _camera = Camera.main;
        }

        private void Start()
        {
            clickstate = PlayerClickState.None;
            clickstate2 = PlayerClickState2.None;

            this.UpdateAsObservable()
                .Where(_ => Input.GetMouseButtonDown(0))
                .Subscribe(hit => { ClickFunc(); });
            this.UpdateAsObservable()
                .Where(_ => ClickCard != null && !EnemyClick && Input.GetMouseButton(0) &&
                            clickstate == PlayerClickState.Card)
                .Subscribe(_ => { DragCardFunc(); });
            this.UpdateAsObservable()
                .Where(_ => ClickCard != null && Input.GetMouseButtonUp(0))
                .Subscribe(_ => { UpCardFunc(); });
        }

        private bool IsPointerOverUIObject()
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            results.Clear();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }

        void ClickFunc()
        {
            if (PlayerInfo.Inst.Dead) return;


            if (!GameSystem_AllInfo.inst.IsStart)
            {
                return;
            }

            if (IsPointerOverUIObject())
            {
                return;
            }


            Vector3 mpos = Input.mousePosition;
            Ray cast = _camera.ScreenPointToRay(mpos);
            if (!PlayerInfo.Inst.PickRound)
            {
                if (Physics.Raycast(cast, out RaycastHit hit, 100, laymaskCard))
                {
                    ClickCardFunc(hit.collider.gameObject);
                    return;
                }
            }


            //???????????? ??????
            if (PlayerInfo.Inst.PlayerOb.TryGetComponent(out PlayerMoving moving))
            {
                moving.move();
                if (PlayerInfo.Inst.PickRound == false)
                {
                    UIManager.inst.CardUIClose();
                    UIManager.inst.CardBuyUiClose();
                }
            }
        }

        void ClickCardFunc(GameObject ob)
        {
            if (ob.GetComponent<CardState>().IsUnit)
            {
                ClickCard = null;
                return;
            }

            ClickCard = ob;
            if (PlayerInfo.Inst.IsBattle && ob.GetComponent<Card_Info>().IsFiled)
            {
                ShowCharacterInfo(ClickCard);
                return;
            }

            MousePos = ob.transform.position;
            clickstate = PlayerClickState.Card;
            SellCheck = false;
            if (ClickCard.GetComponent<Card_Info>().pv.Owner != PhotonNetwork.LocalPlayer)
            {
                EnemyClick = true;
            }
        }

        void DragCardFunc()
        {
            Vector3 scrSpace = _camera.WorldToScreenPoint(ClickCard.transform.position);


            Vector3 MousePos = new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, scrSpace.z);
            Vector3 objPosition = _camera.ScreenToWorldPoint(MousePos);
            objPosition.y = CHARACTERY;
            if (clickstate2 != PlayerClickState2.cardDraging)
            {
                if (Vector3.Distance(ClickCard.transform.position, objPosition) >= 1)
                {
                    clickstate2 = PlayerClickState2.cardDraging;
                    if (ClickCard.TryGetComponent(out CapsuleCollider col))
                    {
                        col.enabled = false;
                    }

                    PlayerInfo.Inst.PlayerTileOb.SetActive(true);
                    if (!PlayerInfo.Inst.IsBattle)
                    {
                        PlayerInfo.Inst.FiledTileOb.SetActive(true);
                    }

                    UIManager.inst.SellSet(ClickCard.GetComponent<Card_Info>().costCheck());
                }
            }
            else
            {
                ClickCard.transform.position = objPosition;

                Vector3 mpos = Input.mousePosition;
                Ray cast = _camera.ScreenPointToRay(mpos);
                RaycastHit hit;

                bool b = false;
                if (Physics.Raycast(cast, out hit, 100, 1 << LayerMask.NameToLayer("Tile")))
                {
                    if (TileOb!=null)
                    {
                        if (TileOb.TryGetComponent(out TileInfo tileinfo))
                        { 
                            tileinfo.SetColor(true);
                        }
                        
                    }
                    

                    TileOb = hit.collider.gameObject;
                    if (TileOb.TryGetComponent(out TileInfo tileinfo2))
                    {
                        tileinfo2.SetColor(false);
                    }

                    b = true;
                    //return;
                }

                if (b == false)
                {
                    if (TileOb != null)
                    {
                        if (TileOb.TryGetComponent(out TileInfo tileinfo))
                        {
                            tileinfo.SetColor(true);
                        }

                        TileOb = null;
                    }
                }
            }
        }

        void UpCardFunc()
        {
            if (TileOb != null)
            {
                if (TileOb.TryGetComponent(out TileInfo tileinfo))
                {
                    tileinfo.SetColor(true);
                }

                TileOb = null;
            }


            EnemyClick = false;
            if (ClickCard == null)
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

                if (clickstate == PlayerClickState.Card && clickstate2 != PlayerClickState2.cardDraging)
                {
                    ShowCharacterInfo(ClickCard);
                    return;
                }

                clickstate = PlayerClickState.None;
                clickstate2 = PlayerClickState2.None;
                Vector3 mpos = Input.mousePosition;
                Ray cast = _camera.ScreenPointToRay(mpos);
                RaycastHit hit;

                bool b = false;
                if (Physics.Raycast(cast, out hit, 100, 1 << LayerMask.NameToLayer("Tile")))
                {
                    //ClickCardFunc(hit.collider.gameObject);
                    TileOb = hit.collider.gameObject;
                    if (TileOb.TryGetComponent(out TileInfo info))
                    {
                        info.SetColor();
                        if (info.tileGameob == null)
                        {
                            CharacterTileMove(ClickCard, TileOb);
                        }
                        else
                        {
                            CharacterTileChange(ClickCard, info.tileGameob);
                        }

                        b = true;
                    }
                    //return;
                }

                if (!b) CharacterTileMoveReset(ClickCard);



            }

            resetfunc();
        }

        public void resetfunc()
        {
            MousePos = Vector3.zero;
            PlayerInfo.Inst.PlayerTileOb.SetActive(false);
            PlayerInfo.Inst.FiledTileOb.SetActive(false);
            clickstate = PlayerClickState.None;
            clickstate2 = PlayerClickState2.None;

            if (ClickCard.TryGetComponent(out CapsuleCollider col))
            {
                col.enabled = true;
            }

            SellCheck = false;
            ClickCard = null;
            UIManager.inst.SellClose();
        }

        void CharacterTileMove(GameObject ob, GameObject Tile)
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
                    if (PlayerInfo.Inst.food + food > PlayerInfo.Inst.foodMax)
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

        public void CharacterTileMoveFunc(GameObject ob, GameObject Tile)
        {
            var ClickCardcom = ob.GetComponent<Card_Info>();
            var tilecom = Tile.GetComponent<TileInfo>();
            int idx = tilecom.Idx;
            tilecom.AddTile(ob);
            Vector3 pos = Tile.transform.position;
            pos.y = CHARACTERY;
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
                    int food = PlayerInfo.Inst.food - obcom1.Food + obcom2.Food;
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
                    int food = PlayerInfo.Inst.food + obcom1.Food - obcom2.Food;
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
            CharacterTileMoveFunc(ob1, Tile2);
            CharacterTileMoveFunc(ob2, Tile1);
        }

        void ShowCharacterInfo(GameObject ob)
        {
            if (Charinfoui.TryGetComponent(out CardUI_Info2 info))
            {
                info.InfoSet(ClickCard);
            }

            ClickCard = null;
        }
    }
}