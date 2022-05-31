using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager inst;

    public List<GameObject> CardUi;
    private void Awake()
    {
        inst = this;
    }

    public void CardResetFunc(int[] idxs)
    {
        for (int i = 0; i < idxs.Length; i++)
        {
            CardSetting(i, idxs[i]);
            
        }
        
    }


    public void CardReset()
    {
        
        MasterInfo.inst.CardResetNetworkFunc1();
    }

    public void CardButton(int idx)
    {
        var Cardinfo = CardUi[idx].GetComponent<CardUI_Info>();
        if (Cardinfo.IsBuy==false)
        {
            return;
        }
        if (PlayerInfo.Inst.Gold<Cardinfo.Cost)
        {
            return;
        }

        if (!CreateManager.inst.CheckCreate(Cardinfo.Idx))
        {
            return;
        }
        PlayerInfo.Inst.Gold -= Cardinfo.Cost;
        CardBuy(idx);

    }

    public void CardSetting(int idx,int i)
    {
        var Cardinfo = CardUi[idx].GetComponent<CardUI_Info>();
        if (Cardinfo.IsBuy==true)
        {
            MasterInfo.inst.CardAdd(Cardinfo.Idx);
        }
        Cardinfo.IsBuy = true;
        CharacterInfo chinfo = CsvManager.inst.characterInfo[i];
        int cardIdx = chinfo.Idx;
        int icon = chinfo.Icon;
        int name = chinfo.Name;
        int cost = chinfo.Tier;
        int Trait1 = chinfo.Trait1;
        int Trait2 = chinfo.Trait2;
        int Job1 = chinfo.Job1;
        int Job2 = chinfo.Job2;


        Cardinfo.CardSet(cardIdx,icon,name,cost,Trait1,Trait2,Job1,Job2);
        //MasterInfo.inst.CardRemove(i);



    }

    public void CardBuy(int idx)
    {
        var Cardinfo = CardUi[idx].GetComponent<CardUI_Info>();
        int Cardidx = Cardinfo.Idx;
        Cardinfo.Cardback();
        CreateManager.inst.CreateCharacter(Cardidx);
        
    }
}
