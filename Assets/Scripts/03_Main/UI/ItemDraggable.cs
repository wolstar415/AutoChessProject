using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UniRx;


namespace GameS
{
    public class ItemDraggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler,
        IPointerExitHandler, IDropHandler,IPointerUpHandler,IPointerDownHandler
    {
        [SerializeField] private Transform canvas;
        [SerializeField] private Transform previousParent;
        [SerializeField] private RectTransform rect;
        [SerializeField] private Vector3 oriPos;
        [SerializeField] private CanvasGroup CanvasGroup;
        [SerializeField] private GridLayoutGroup gridLayout;
        //[SerializeField] LayerMask laymaskCard;
        public int Idx;
        [SerializeField] private Image ItemIcon;
        [Header("ItemUI")] [SerializeField] private GameObject CreateItemInfoOb;
        [SerializeField] private Image CreateItemIcon;
        [SerializeField] private TextMeshProUGUI CreateItemName;
        [SerializeField] private TextMeshProUGUI CreateItemInfo;
        [SerializeField] private GameObject CreateItemOb;
        [SerializeField] private Image CreateItem1;
        [SerializeField] private Image CreateItem2;
        [Header("CardUI")] [SerializeField] private GameObject CardItemInfoOb;
        [SerializeField] private Image CardItemIcon;
        [SerializeField] private TextMeshProUGUI CardItemName;
        [SerializeField] private TextMeshProUGUI CardItemInfo;
        [SerializeField] private GameObject CardItemOb;
        [SerializeField] private Image CardItem1;
        [SerializeField] private Image CardItem2;
        [SerializeField] private Image CardIcon;
        [SerializeField] private TextMeshProUGUI CardName;
        [SerializeField] private bool ShowInfo = false;
        GameObject targetcard = null;
        private bool ShowCard=false;

        private void Start()
        {
            canvas = GameSystem_AllInfo.inst.ItemCanvs;
            previousParent = GameSystem_AllInfo.inst.ItemParent;
            gridLayout = GameSystem_AllInfo.inst.ItemGridLayout;
            CreateItemInfoOb.SetActive(false);
            CardItemInfoOb.SetActive(false);
            
            
            
        }

        public void Startfunc(int idx)
        {
            Idx = idx;
            int icon = CsvManager.inst.itemInfo[idx].Icon;
            
            ItemIcon.sprite = IconManager.inst.icon[icon];
        }


        public void OnPointerDown(PointerEventData eventData)
        {
            oriPos = eventData.position;
            SetUiCreate();
            
            CreateItemInfoOb.SetActive(true);
            CardItemInfoOb.SetActive(false);

        }
        public void OnPointerUp(PointerEventData eventData)
        {
            CreateItemInfoOb.SetActive(false);
            CardItemInfoOb.SetActive(false);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            gridLayout.enabled = false;
            ShowInfo = false;
            ShowCard = false;
            targetcard = null;
            // = transform.parent;
            //transform.SetParent(canvas);
            //transform.SetAsLastSibling();
            CanvasGroup.blocksRaycasts = false;
            ClickManager.inst.clickstate = PlayerClickState.item;
            ClickManager.inst.ClickItem = gameObject;
            
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (Vector3.Distance(eventData.position, oriPos) >= 50 && !ShowInfo)
            {
                CreateItemInfoOb.SetActive(false);
                ShowInfo = true;
            }

            if (ShowInfo)
            {
                rect.position = eventData.position;

                
                
                

                Ray cast = Camera.main.ScreenPointToRay(eventData.position);
                RaycastHit hit;

                GameObject hitob = null;
                    if (Physics.Raycast(cast, out hit, 100, GameSystem_AllInfo.inst.masks[PlayerInfo.Inst.PlayerIdx]))
                    {
                        hitob = hit.collider.gameObject;




                    }

                    if (hitob==null)
                    {
                        if (ShowCard)
                        {
                            targetcard = null;
                            ShowCard = false;
                            outUicard();
                        }
                    }
                    else
                    {
                        if (targetcard!=hitob)
                        {
                            targetcard = hitob;
                            ShowCard = true;
                            SetUiCard(targetcard);
                        }
                    }

            }
        }


        

        public void OnEndDrag(PointerEventData eventData)
        {
            GameObject ob = gob(eventData.position);
            CanvasGroup.blocksRaycasts = true;
            if (ClickManager.inst.ItemDropCard != null)
            {
                if (ItemManager.inst.ItemCheck(ClickManager.inst.ItemDropCard,Idx))
                {
                    if (ClickManager.inst.ItemDropCard.TryGetComponent(out Card_Info info))
                    {
                        info.Itemadd(Idx);
                    }
                    Destroy(gameObject);
                }
            }
            else if (ob!=null)
            {
                if (ItemManager.inst.ItemCheck(ob,Idx))
                {
                    if (ob.TryGetComponent(out Card_Info info))
                    {
                        info.Itemadd(Idx);
                    }
                    Destroy(gameObject);
                }
            }


            ClickManager.inst.ItemDropCard = null;
            ClickManager.inst.ClickItem = null;
            ClickManager.inst.clickstate = PlayerClickState.None;
            gridLayout.enabled = true;
            ShowInfo = false;
            CreateItemInfoOb.SetActive(false);
            CardItemInfoOb.SetActive(false);
        }


        GameObject gob(Vector3 pos)
        {
            GameObject ob = null;
            Ray cast = Camera.main.ScreenPointToRay(pos);
            RaycastHit hit;
            if (Physics.Raycast(cast, out hit, 100, GameSystem_AllInfo.inst.masks[PlayerInfo.Inst.PlayerIdx]))
            {
                return hit.collider.gameObject;
            }

            return ob;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
           
            if (ClickManager.inst.ClickItem == gameObject)
            {
                return;
            }

            if (ClickManager.inst.clickstate == PlayerClickState.item)
            {
                
                int idx1=0;
                int idx2 = Idx;
                if (ClickManager.inst.ClickItem.TryGetComponent(out ItemDraggable info1))
                {
                    idx1= info1.Idx ;
                }

                if (idx1<=8 &&idx2<=8)
                {
                    SetUiCreate2(idx1,idx2);
                CreateItemInfoOb.SetActive(true);
                CreateItemOb.SetActive(true);
                }
                
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (ClickManager.inst.ClickItem == gameObject)
            {
                return;
            }

            if (ClickManager.inst.clickstate == PlayerClickState.item)
            {
                
                CreateItemInfoOb.SetActive(false);
                CreateItemOb.SetActive(false);
                
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (ClickManager.inst.ClickItem == gameObject)
            {
                return;
            }


            outUicard();
        }

        void SetUiCreate()
        {
            int itemicon = CsvManager.inst.itemInfo[Idx].Icon;
            int itemname = CsvManager.inst.itemInfo[Idx].Name;
            int iteminfo = CsvManager.inst.itemInfo[Idx].Info;
            CreateItemIcon.sprite = IconManager.inst.icon[itemicon];
            CreateItemName.text = CsvManager.inst.GameText(itemname);
            CreateItemInfo.text = CsvManager.inst.GameText(iteminfo);
        }
        void SetUiCreate2(int idx1,int idx2)
        {
            int mixidx = ItemManager.inst.ItemMixIdx(idx1, idx2);
            int itemicon = CsvManager.inst.itemInfo[mixidx].Icon;
            int itemname = CsvManager.inst.itemInfo[mixidx].Name;
            int iteminfo = CsvManager.inst.itemInfo[mixidx].Info;
            
            int itemicon1 = CsvManager.inst.itemInfo[idx1].Icon;
            int itemicon2 = CsvManager.inst.itemInfo[idx2].Icon;
            CreateItemIcon.sprite = IconManager.inst.icon[itemicon];
            CreateItemName.text = CsvManager.inst.GameText(itemname);
            CreateItemInfo.text = CsvManager.inst.GameText(iteminfo);
            
            CreateItem1.sprite = IconManager.inst.icon[itemicon2];
            CreateItem2.sprite = IconManager.inst.icon[itemicon1];
            
            
        }

        public void outUicard()
        {
            CreateItemInfoOb.SetActive(false);
            CardItemInfoOb.SetActive(false);
        }
        public void SetUiCard(GameObject ob)
        {
            int check = -1;
            int cardidx = 0;
            bool b = ItemManager.inst.ItemCheck(ob,Idx);

            if (ob.TryGetComponent(out Card_Info info))
            {
               
                for (int i = 0; i < 3; i++)
                {
                    if (info.Item[i]>=0 &&info.Item[i]<=8)
                    {
                        check = info.Item[i];
                        break;
                    }
                }
            }
            if (b==false)
            {
                return;
            }
            if (check==-1)
            {
                setuicard(cardidx);
            }
            else
            {
                Setuicardmix(cardidx,check,Idx);
            }
        }

         void Setuicardmix(int cardidx,int idx1,int idx2)
        {
            int mixidx = ItemManager.inst.ItemMixIdx(idx1, idx2);
            int itemicon = CsvManager.inst.itemInfo[mixidx].Icon;
            int itemname = CsvManager.inst.itemInfo[mixidx].Name;
            int iteminfo = CsvManager.inst.itemInfo[mixidx].Info;
            int cardicon = CsvManager.inst.characterInfo[cardidx].Icon;
            int cardname = CsvManager.inst.characterInfo[cardidx].Name;
            int itemicon1 = CsvManager.inst.itemInfo[idx1].Icon;
            int itemicon2 = CsvManager.inst.itemInfo[idx2].Icon;
            CardItemIcon.sprite = IconManager.inst.icon[itemicon];
            CardItemName.text = CsvManager.inst.GameText(itemname);
            CardItemInfo.text = CsvManager.inst.GameText(iteminfo);
            CardIcon.sprite = IconManager.inst.icon[cardicon];
            CardName.text = CsvManager.inst.GameText(cardname);
            
            CardItem1.sprite = IconManager.inst.icon[itemicon1];
            CardItem2.sprite = IconManager.inst.icon[itemicon2];
            CreateItemInfoOb.SetActive(false);
            CardItemInfoOb.SetActive(true);
            CardItemOb.SetActive(true);
        }

         void setuicard(int idx)
        {
            CardItemOb.SetActive(false);
            int itemicon = CsvManager.inst.itemInfo[Idx].Icon;
            int itemname = CsvManager.inst.itemInfo[Idx].Name;
            int iteminfo = CsvManager.inst.itemInfo[Idx].Info;
            int cardicon = CsvManager.inst.characterInfo[idx].Icon;
            int cardname = CsvManager.inst.characterInfo[idx].Name;
            CardItemIcon.sprite = IconManager.inst.icon[itemicon];
            CardItemName.text = CsvManager.inst.GameText(itemname);
            CardItemInfo.text = CsvManager.inst.GameText(iteminfo);
            CardIcon.sprite = IconManager.inst.icon[cardicon];
            CardName.text = CsvManager.inst.GameText(cardname);
            CreateItemInfoOb.SetActive(false);
            CardItemInfoOb.SetActive(true);
        }



    }
}