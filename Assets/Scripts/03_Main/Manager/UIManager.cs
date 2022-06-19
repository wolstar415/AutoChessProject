using System;
using System.Collections;
using System.Collections.Generic;
using GameS;
using Photon.Pun;
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
    [SerializeField] private GameObject TopPancel;
    [SerializeField] private GameObject Foodob;
    [SerializeField] private GameObject DownPanel;
    [SerializeField] private GameObject SellPanel;
    [SerializeField] private TextMeshProUGUI Selltext1;
    [SerializeField] private TextMeshProUGUI Selltext2;
    [SerializeField] private GameObject BuyPanelOb;
    [SerializeField] private GameObject JobAndItemOb;
    
    [SerializeField] private Image LockImage;
    

    
    [Header("TraitAndJob")]
    public Color[] colors;
    [SerializeField] private GameObject JobPanelOb;
    [SerializeField] private Image JobBtnImage;

    [Header("item")] 
    [SerializeField] private GameObject ItemPanelOb;
    [SerializeField] private Image ItemBtnImage;
    [Header("TopUI")] 
    [SerializeField] private TextMeshProUGUI RoundText;
    [SerializeField] private GameObject FirstRound;
    [SerializeField] private GameObject NormalRound;
    [SerializeField] private Image[] FirstRoundImages;
    [SerializeField] private Image[] RoundImages;
    [SerializeField] private GameObject TopPanelOb;
    [SerializeField] private TextMeshProUGUI TimeText;
    [SerializeField] private Slider TimeSlider;
    private Coroutine timeCoroutine=null;
    [Header("PlayerInfo")]
    [SerializeField] 
    private GameObject playerInfoOb_Parent;
    [SerializeField] private GameObject[] playerInfoObs;
    [Header("연승관련")] 
    [SerializeField] private Sprite[] vicon;
    [SerializeField] private Image vImage;
    [SerializeField] private TextMeshProUGUI vText;
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
        TopPanelOb.SetActive(false);
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



    public void BattleStartUi()
    {
        TopPanelOb.SetActive(false);
        FoodText.gameObject.SetActive(false);
    }
    public void BattleEndUi()
    {
        TopPanelOb.SetActive(true);
        FoodText.gameObject.SetActive(true);
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
        
        if (PlayerInfo.Inst.Round>=2)
        {
            DownPanel.SetActive(true);
            
        }
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
        if (!PlayerInfo.Inst.IsBattle)
        {
        TopPanelOb.SetActive(true);
            
        }
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

    public void PickUiSetting()
    {
        TopPancel.SetActive(false);
        DownPanel.SetActive(false);
        BuyPanelOb.SetActive(false);
        InfoPanelOb.SetActive(false);
        PlayerInfoPanelOb.SetActive(false);
        Foodob.SetActive(false);
        JobAndItemOb.SetActive(false);
    }

    public void BattleUiSetting()
    {
        //BuyPanelOb.SetActive(false);
        InfoPanelOb.SetActive(false);
        TopPancel.SetActive(true);
        if (PlayerInfo.Inst.Round>=2)
        {
        DownPanel.SetActive(true);
            
        }
        PlayerInfoPanelOb.SetActive(true);
        Foodob.SetActive(true);
        JobAndItemOb.SetActive(true);
    }

    public void RoundChange()
    {
        FirstRound.SetActive(false);
        NormalRound.SetActive(true);
    }

    public void RoundSet()
    {
        int r = PlayerInfo.Inst.Round;
        int round=CsvManager.inst.RoundCheck1[r];
        int round2=CsvManager.inst.RoundCheck2[r];

        RoundText.text = round + "-" + round2;

        if (round==1)
        {
            for (int i = 0; i < FirstRoundImages.Length; i++)
            {
                FirstRoundImages[i].color = colors[2];
            }
            FirstRoundImages[round2-1].color = colors[3];
        }
        else
        {
            for (int i = 0; i < RoundImages.Length; i++)
            {
                RoundImages[i].color = colors[2];
            }
            RoundImages[round2-1].color = colors[3];
        }
    }

    public void TimeFunc(int Time)
    {
        if (timeCoroutine!=null)
        {
            StopCoroutine(timeCoroutine);
        }
        timeCoroutine=StartCoroutine(ITimeFunc(Time));
    }

    public void TimeEnd()
    {
        StopCoroutine(timeCoroutine);
        timeCoroutine = null;
    }

    IEnumerator ITimeFunc(int MaxTime)
    {
        TimeText.text = MaxTime.ToString();
        
        float f = 0;
        TimeSlider.value = f;
        TimeSlider.maxValue = MaxTime;
        while (true)
        {
            
            f += Time.deltaTime;
            if (f>=MaxTime)
            {
                break;
            }
            float maxf = MaxTime - f;
        TimeText.text = maxf.ToString("F0");
        TimeSlider.value = f;
            yield return null;
        }
        TimeSlider.value = MaxTime;
        TimeText.text = "0";

    }

    public void PlayerUiSetting(int idx, string name)
    {
        playerInfoObs[idx].SetActive(true);
        if (playerInfoObs[idx].TryGetComponent(out PlayerInfoUI info))
        {
            info.NickNameSet(name);
        }
    }

    public void PlayerInfoClick(int idx)
    {
        //공격중이라면 공격중인곳으로가기
        //아니라면 그곳으로 이동


        NetworkManager.inst.CameraMovePlayer(idx,false);

    }
    public void PlayerInfoBattleStart()
    {
        for (int i = 0; i < playerInfoObs.Length; i++)
        {
            if (playerInfoObs[i].TryGetComponent(out PlayerInfoUI p))
            {
                p.BattleStart();

            }

        }
    }
    public void PlayerInfoOrder()
    {
        for (int i = 0; i < MasterInfo.inst.lifeRank2.Count; i++)
        {
            int idx = MasterInfo.inst.lifeRank2[i].PlayerIdx;
            playerInfoObs[idx].transform.SetSiblingIndex(i);
            if (playerInfoObs[idx].TryGetComponent(out PlayerInfoUI p))
            {
                p.HpSet(MasterInfo.inst.lifeRank2[i].Life);
                p.BattleEnd();

            }
        }
    }

    public void DeadUi()
    {
        TopPancel.SetActive(false);
        DownPanel.SetActive(false);
        BuyPanelOb.SetActive(false);
        InfoPanelOb.SetActive(false);
        Foodob.SetActive(false);
        JobAndItemOb.SetActive(false);
        PlayerInfoPanelOb.SetActive(true);
    }




    public void LockButton()
    {
        if (PlayerInfo.Inst.IsLock)
        {
            LockCheck(false);
        }
        else
        {
            LockCheck(true);
        }
    }

    public void LockCheck(bool b)
    {
        if (b)
        {
            
        PlayerInfo.Inst.IsLock = true;
        LockImage.color = Color.white;
        }
        else
        {
            PlayerInfo.Inst.IsLock = false;
            LockImage.color = Color.black;
        }
    }
    
    public void PlayerInfoClear(int idx)
    {
        if (playerInfoObs[idx].TryGetComponent(out PlayerInfoUI p))
        {
            p.Clear();
        }
    }

    public void VictorySet()
    {
        if (PlayerInfo.Inst.IsVictory)
        {
            vImage.sprite = vicon[0];
            vText.text = PlayerInfo.Inst.victoryCnt.ToString();
        }
        else
        {
            vImage.sprite = vicon[1];
            vText.text = PlayerInfo.Inst.defeatCnt.ToString();
            
        }
    }
}
