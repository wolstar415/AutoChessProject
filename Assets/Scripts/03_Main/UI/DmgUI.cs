using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace GameS
{
    public class DmgUI : MonoBehaviour,IPointerUpHandler,IPointerDownHandler
    {
        public int Idx;
        public float DmgAll;
        public float DmgPhy;
        public float DmgMagic;
        public float DmgTrue;
        public string name;
        public Image icon;
        public Slider phySlider;
        public Slider magicSlider;
        public Slider trueSlider;
        public GameObject[] starobs;
        public TextMeshProUGUI dmgtext;
        public void OnPointerUp(PointerEventData eventData)
        {
            Debug.Log("마우스땜");
            UIManager.inst.DmgUiob.SetActive(false);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("클릭");
            UIManager.inst.DmgUIFunc(name,DmgAll,DmgPhy,DmgMagic,DmgTrue);
            
        }

        public void DmgReset(Card_Info info)
        {
            //
            gameObject.SetActive(true);
            name = info.name;
            phySlider.value = 0;
            magicSlider.value = 0;
            trueSlider.value = 0;

            dmgtext.text = "0";
            int lv = info.Level;
            if (lv>=2) starobs[1].SetActive(true);
            if (lv>=3) starobs[2].SetActive(true);
            int II = info.Icon;
            icon.sprite = IconManager.inst.icon[II];

        }

        public void DmgUp(int dmgtype, float dmg)
        {
            if (dmgtype==2)
            {
                DmgMagic += dmg;
            }
            else if (dmgtype==3)
            {
                DmgTrue += dmg;
                
            }
            else
            {
                DmgPhy += dmg;
                
            }
            
                DmgAll += dmg;
                dmgtext.text = DmgAll.ToString("F0");
            PlayerInfo.Inst.roundDamgeMax = Mathf.Max(PlayerInfo.Inst.roundDamgeMax, DmgAll);
            UIManager.inst.DmgOrder();
        }

        public void DmgSet(int idx)
        {
            float Max = PlayerInfo.Inst.roundDamgeMax;
            if (DmgAll<=0||Max<=0)
            {
                return;
            }

            
            float f1 = DmgAll / Max;
            float f2=0;
            float f3=0;
            float f4=0;

            
            if (DmgPhy>0)
            {
                f2 = DmgPhy / DmgAll;
                phySlider.value = f1 * f2;
            }
            if (DmgMagic>0)
            {
                f3 = DmgMagic / DmgAll;
                magicSlider.value = f1 * f3;
            }
            if (DmgTrue>0)
            {
                f4 = DmgTrue / DmgAll;
                trueSlider.value = f1 * f4;
            }

            
            
            
            
            transform.SetSiblingIndex(idx);
        }

        public void DmgNoShow()
        {
            gameObject.SetActive(false);
            DmgAll = 0;
            DmgPhy = 0;
            DmgMagic = 0;
            DmgTrue = 0;
            dmgtext.text = "0";
            starobs[0].SetActive(true);
            starobs[1].SetActive(false);
            starobs[2].SetActive(false);
        }
    }
}
