using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardUI_Info : MonoBehaviour
{
    public int CardIdx;
    [Header("정보")]
    public int Idx;
    public int Lv;
    public int Cost;
    public bool IsBuy;
    [Header("UI")] 
    public Image Icon;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI CostText;
    public TextMeshProUGUI Trait1;
    public TextMeshProUGUI Trait2;
    public TextMeshProUGUI Job1;
    public TextMeshProUGUI Job2;
    public Image TraitIcon1;
    public Image TraitIcon2;
    public Image JobIcon1;
    public Image JobIcon2;

    public GameObject Ob_Card;
    public GameObject Ob_Trait1;
    public GameObject Ob_Trait2;
    public GameObject Ob_Job1;
    public GameObject Ob_Job2;
    public GameObject Effect;
    public void Cardback()
    {
        IsBuy = false;
        Idx = -1;
        Ob_Card.SetActive(false);
        Ob_Trait1.SetActive(false);
        Ob_Trait2.SetActive(false);
        Ob_Job1.SetActive(false);
        Ob_Job2.SetActive(false);
        Effect.SetActive(false);
        Icon.sprite = IconManager.inst.icon[230];
    }

    public void CardSet(int idx,int icon, int name, int cost, int trait1, int trait2, int job1, int job2)
    {
        int trait1text = CsvManager.inst.TraitandJobName[trait1];
        int trait2text = CsvManager.inst.TraitandJobName[trait2];
        int job1text = CsvManager.inst.TraitandJobName[job1];
        int job2text = CsvManager.inst.TraitandJobName[job2];
        int trait1icon = CsvManager.inst.TraitandJobIcon[trait1];
        int trait2icon = CsvManager.inst.TraitandJobIcon[trait2];
        int job1icon = CsvManager.inst.TraitandJobIcon[job1];
        int job2icon = CsvManager.inst.TraitandJobIcon[job2];
        Idx = idx;
        IsBuy = true;
        Icon.sprite = IconManager.inst.icon[icon];
        Name.text = CsvManager.inst.GameText(name);
        CostText.text = cost.ToString();
        Cost = cost;
        Lv = cost;
        Trait1.text = CsvManager.inst.GameText(trait1text);
        Trait2.text = CsvManager.inst.GameText(trait2text);
        Job1.text = CsvManager.inst.GameText(job1text);
        Job2.text = CsvManager.inst.GameText(job2text);
        TraitIcon1.sprite = IconManager.inst.icon[trait1icon];
        TraitIcon2.sprite = IconManager.inst.icon[trait2icon];
        JobIcon1.sprite = IconManager.inst.icon[job1icon];
        JobIcon2.sprite = IconManager.inst.icon[job2icon];
        Ob_Card.SetActive(true);
        Ob_Trait1.SetActive(true);
        Ob_Job1.SetActive(true);
        if (job2>0)
        {
            Ob_Job2.SetActive(true);
        }
        if (trait2>0)
        {
            Ob_Trait2.SetActive(true);
        }

        CheckEffect();
    }

    public void CheckEffect()
    {
        if (Idx==-1)
        {
            return;
        }
        
        if (PlayerInfo.Inst.PlayerCardCntLv[Idx].Lv1.Count>=2)
        {
            Effect.SetActive(true);
        }
        else
        {
            Effect.SetActive(false);
        }

    }
    

}
