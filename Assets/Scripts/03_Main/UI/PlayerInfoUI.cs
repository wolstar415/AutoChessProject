using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameS
{
    public class PlayerInfoUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI NickName;
        [SerializeField] private TextMeshProUGUI HpText;
        [SerializeField] private Slider HpSlider;
        [SerializeField] private Image image;
        [SerializeField] private Color DeadColor;
        [SerializeField] private GameObject clear;
        [SerializeField] private GameObject StateText;
        public bool IsDead=false;
        public int Idx = 0;
        
        

        public void HpSet(int hp)
        {
            
            HpText.text = hp.ToString();
            HpSlider.value = hp;
            if (hp<=0)
            {
                gameObject.GetComponent<CanvasGroup>().alpha = 0.3f;
            }
        }

        public void NickNameSet(string name)
        {
            NickName.text = name;
        }

        public void Dead()
        {
            HpText.text = "0";
            HpSlider.value = 100;
            image.color = DeadColor;
            IsDead = true;
        }

        public void BattleStart()
        {
            if (IsDead) return;
            
            HpText.gameObject.SetActive(false);
            StateText.SetActive(true);
            clear.SetActive(false);
        }
        public void BattleEnd()
        {
            if (IsDead) return;
            
            HpText.gameObject.SetActive(true);
            StateText.SetActive(false);
            clear.SetActive(false);
        }
        public void Clear()
        {
            if (IsDead) return;
            
            clear.SetActive(true);
        }
        
    }
}
