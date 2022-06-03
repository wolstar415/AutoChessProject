using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager inst;
    public TextMeshProUGUI GoldText;
    public TextMeshProUGUI FoodText;

    [SerializeField] private GameObject InfoPanelOb;
    [SerializeField] private GameObject PlayerInfoPanelOb;
    [Header("CardBuyUI")] [SerializeField] private TextMeshProUGUI[] ReRolltext;
    [SerializeField] private Slider XpSlider;
    [SerializeField] private TextMeshProUGUI XpText;
    [SerializeField] private TextMeshProUGUI LvText;
    [SerializeField] private GameObject DownPanel;
    [SerializeField] private GameObject SellPanel;
    [SerializeField] private TextMeshProUGUI Selltext1;
    [SerializeField] private TextMeshProUGUI Selltext2;
    [SerializeField] private GameObject BuyPanelOb;

    
    [Header("TraitAndJob")]
    public Color[] colors;
    [SerializeField] private GameObject JobPanelOb;
    [SerializeField] private Image JobBtnImage;

    [Header("item")] 
    [SerializeField] private GameObject ItemPanelOb;
    [SerializeField] private Image ItemBtnImage;
    private void Awake()
    {
        inst = this;
    }

    public void GoldSet()
    {
        int gold = PlayerInfo.Inst.Gold;
        GoldText.text = gold.ToString();
    }

    public void FoodSet()
    {
        int food = PlayerInfo.Inst.food;
        int foodmax = PlayerInfo.Inst.foodMax;
        FoodText.text = food + " / " + foodmax;
    }
    public void CardBuyButtonFunc1()
    {
        BuyPanelOb.SetActive(true);
        PlayerInfoPanelOb.SetActive(false);
    }

    public void CardResetButton()
    {
        if (PlayerInfo.Inst.Gold < 2) return;
        
        PlayerInfo.Inst.Gold -= 2;
        CardManager.inst.CardReset();
    }

    public void ReRollSet(int lv)
    {
        List<int> ReRoll = CsvManager.inst.ReRoll(lv);

        for (int i = 0; i < ReRoll.Count; i++)
        {
            ReRolltext[i].text = ReRoll[i].ToString() + "%";
        }
    }

    public void XpBuy()
    {
        if (PlayerInfo.Inst.Gold>=4)
        {
        PlayerInfo.Inst.XpPlus(4);
        PlayerInfo.Inst.Gold -= 4;
        }
    }

    public void XpSliderSet()
    {
        int xp = PlayerInfo.Inst.Xp;
        int xpmax = PlayerInfo.Inst.XpMax;

        XpSlider.maxValue = xpmax;
        XpSlider.value = xp;
        XpText.text = xp + " / " + xpmax;
        LvText.text = PlayerInfo.Inst.Level.ToString();

    }
    

    public void SellSet(int cost)
    {
        DownPanel.SetActive(false);
        SellPanel.SetActive(true);
        Selltext1.text = cost.ToString();
        Selltext2.text = cost.ToString();
    }

    public void SellClose()
    {
        DownPanel.SetActive(true);
        SellPanel.SetActive(false);
    }

    public void CardUIClose()
    {
        InfoPanelOb.SetActive(false);
        PlayerInfoPanelOb.SetActive(true);
    }

    public void CardBuyUiClose()
    {
        BuyPanelOb.SetActive(false);
        PlayerInfoPanelOb.SetActive(true);
        
    }

    public void TraitUIStart()
    {
        JobPanelOb.SetActive(true);
        ItemPanelOb.SetActive(false);
        JobBtnImage.color = colors[0];
        ItemBtnImage.color = colors[1];
    }

    public void ItemUIStart()
    {
        JobPanelOb.SetActive(false);
        ItemPanelOb.SetActive(true);
        JobBtnImage.color = colors[1];
        ItemBtnImage.color = colors[0];
    }

    public void CardInfoStart()
    {
        InfoPanelOb.SetActive(true);
        BuyPanelOb.SetActive(false);
        PlayerInfoPanelOb.SetActive(false);
    }
    
    
}
