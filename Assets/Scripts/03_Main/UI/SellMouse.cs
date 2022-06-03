using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameS
{
    public class SellMouse : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
    {

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (ClickManager.inst.clickstate==PlayerClickState.Card)
            {
                ClickManager.inst.SellCheck = true;
                
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (ClickManager.inst.clickstate==PlayerClickState.Card)
            {
                Invoke("Check",0.2f);
            }
        }
        void Check()
        {
            ClickManager.inst.SellCheck = false;
        }

    }
    
}
