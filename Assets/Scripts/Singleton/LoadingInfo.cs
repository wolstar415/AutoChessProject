using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GameS
{
    public class LoadingInfo : MonoBehaviour
    {
        public Image Icon;
        //public Sprite[] icons;
        public TextMeshProUGUI nickname;

        public void NickNameSet(string name)
        {
            nickname.text = name;
        }

        public void IconSet(int idx)
        {
            Icon.sprite = GameManager.Inst.charIcons[idx];
        }
    }
}
