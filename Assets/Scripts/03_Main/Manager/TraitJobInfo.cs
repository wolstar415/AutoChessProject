using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UniRx;
namespace GameS
{
    public class TraitJobInfo : MonoBehaviour,IPointerUpHandler,IPointerDownHandler
    {
        public int Cnt = 0;
        public int[] CntIf;

        public int Idx;
        public Image icon;
        public TextMeshProUGUI Name;
        public TextMeshProUGUI CntText;
        public CanvasGroup canvasgroup;
        public GameObject InfoOb;
        [Header("UI")] 
        [SerializeField] Image InfoIcon;
        [SerializeField] TextMeshProUGUI Infoname;
        [SerializeField] TextMeshProUGUI InfoInfo1;
        [SerializeField] TextMeshProUGUI InfoInfo2;
        [SerializeField] GameObject IconPrefab;
        [SerializeField] Transform parent;
        public Subject<bool> Sub_CardJobAndTraitShow = new Subject<bool>();
        public void OnPointerDown(PointerEventData eventData)
        {
            //UI보임
            InfoOb.SetActive(true);
            Info2Set();
            Sub_CardJobAndTraitShow.OnNext(true);
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            InfoOb.SetActive(false);
            Sub_CardJobAndTraitShow.OnNext(false);
            //UI끄기
        }

        private void Start()
        {
            int icon = CsvManager.inst.TraitandJobIcon[Idx];
            int name = CsvManager.inst.TraitandJobName[Idx];
            int info = CsvManager.inst.TraitandJobInfo[Idx];
            InfoIcon.sprite = IconManager.inst.icon[icon];
            Infoname.text = CsvManager.inst.GameText(name);
            InfoInfo1.text = CsvManager.inst.GameText(info);
            
            // 챔피언들 추가
            for (int i = 0; i < CsvManager.inst.traitJobInfo[Idx].Cards.Count; i++)
            {
                int card = CsvManager.inst.traitJobInfo[Idx].Cards[i];
                int cicon = CsvManager.inst.characterInfo[card].Icon;
                GameObject ob = Instantiate(IconPrefab, parent);
                if (ob.TryGetComponent(out Image image))
                {
                    image.sprite = IconManager.inst.icon[cicon];
                }

            }
        }

        void Info2Set()
        {
            List<string> s = new List<string>();
            s.Add("<color=grey>");
            s.Add("<color=grey>");
            s.Add("<color=grey>");
            s.Add("<color=grey>");
            s.Add("<color=grey>");
            int level = LevelCheck();
            s[level] = "<color=green>";
            int infocnt = CsvManager.inst.TraitandJobInfo1[Idx];
            string a = CsvManager.inst.GameText(infocnt);
            string info=String.Format(a,s[0],s[1],s[2],s[3],s[4]);
            InfoInfo2.text = info;

        }

        int LevelCheck()
        {
            int a = 0;
            for (int i = 0; i < 5; i++)
            {
                if (CntIf[i]>Cnt)
                {
                    break;
                }

                a++;

            }
            return a;
        }

        void UiCheck()
        {
            int level = LevelCheck();

            if (level>=1)
            {
                canvasgroup.alpha = 1;
            }
            else
            {
                canvasgroup.alpha = 0.6f;
            }
        }

        public void CntAdd()
        {
            Cnt++;
            int level = LevelCheck();
            if (Cnt>=1)
            {
                gameObject.SetActive(true);
            }
            CntText.text=Cnt+" / "+CntIf[level];
            UiCheck();
        }

        public void CntRemove()
        {
            
            Cnt--;
            int level = LevelCheck();
            if (Cnt<=0)
            {
                gameObject.SetActive(false);
            }
            CntText.text=Cnt+" / "+CntIf[level];
            UiCheck();
        }
    }
}
