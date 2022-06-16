using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GameS
{
    public class CardUI_Info2 : MonoBehaviour
    {
        public GameObject Card;
        public Card_Info info;
        [Header("UI")] [Space] public Slider HpSlider;
        public Slider MpSlider;
        public TextMeshProUGUI HpText;
        public TextMeshProUGUI MpText;
        public Image Icon;
        public Image TraitIcon;
        public Image JobIcon;
        public Image TraitJobIcon;
        public Image SkillIcon;
        public Image[] ItemIcon;
        public TextMeshProUGUI TraitName;
        public TextMeshProUGUI JobName;
        public TextMeshProUGUI TraitJobName;
        public TextMeshProUGUI SkillName;
        public TextMeshProUGUI At;
        public TextMeshProUGUI Magic;
        public TextMeshProUGUI Defense;
        public TextMeshProUGUI MagicDefense;
        public TextMeshProUGUI Atspeed;
        public TextMeshProUGUI Range;
        public TextMeshProUGUI CriPer;
        public TextMeshProUGUI CriDamge;
        public TextMeshProUGUI Name;
        public TextMeshProUGUI Cost;
        [Header("Info")] public Image SkillInfoIcon;
        public TextMeshProUGUI SkillInfoName;
        public TextMeshProUGUI SkillInfo_Info1;
        public TextMeshProUGUI SkillInfo_Info2;
        public Image ItemInfoIcon;
        public TextMeshProUGUI ItemInfoName;
        public TextMeshProUGUI ItemInfo_Info;
        [Space] [Header("체크용")] public GameObject TraitJob_ob;
        public GameObject ItemInfo_ob;
        public GameObject SkillInfo_ob;

        public void InfoSet(GameObject ob)
        {
            TraitJob_ob.SetActive(false);
            ItemInfo_ob.SetActive(false);
            SkillInfo_ob.SetActive(false);
            Card = ob;
            info = Card.GetComponent<Card_Info>();

            //gameObject.SetActive(true);

            Name.text = CsvManager.inst.GameText(info.Name);
            Icon.sprite = IconManager.inst.icon[info.Icon];
            int trait1text = CsvManager.inst.TraitandJobName[info.Character_trait1];
            int job1text = CsvManager.inst.TraitandJobName[info.Character_Job1];
            int trait1icon = CsvManager.inst.TraitandJobIcon[info.Character_trait1];
            int job1icon = CsvManager.inst.TraitandJobIcon[info.Character_Job1];
            TraitIcon.sprite = IconManager.inst.icon[trait1icon];
            TraitName.text = CsvManager.inst.GameText(trait1text);
            JobIcon.sprite = IconManager.inst.icon[job1icon];
            JobName.text = CsvManager.inst.GameText(job1text);
            TraitJobCheck();

            int skilltext = info.info.skillinfo.Name;
            int skillicon = info.info.skillinfo.Icon;
            SkillName.text = CsvManager.inst.GameText(skilltext);
            SkillIcon.sprite = IconManager.inst.icon[skillicon];

            At.text = info.stat.Atk_Damage().ToString("F0");
            Magic.text = info.stat.Magic_Damage().ToString("F0");
            ;
            Defense.text = info.stat.Defence().ToString("F0");
            ;
            MagicDefense.text = info.stat.Defence_Magic().ToString("F0");
            ;
            Atspeed.text = info.stat.Atk_Cool().ToString("F0");
            ;
            Range.text = info.stat.Range().ToString("F0");
            ;
            CriPer.text = info.stat.CriPer().ToString("F0");
            ;
            CriDamge.text = info.stat.CriDmg().ToString("F0");
            ;
            Name.text = CsvManager.inst.GameText(info.Name);
            Icon.sprite = IconManager.inst.icon[info.Icon];

            Cost.text = CsvManager.inst.GoldCost(info.Tier, info.Level).ToString();

            //스킬
            SkillInfoName.text = SkillName.text;
            SkillInfoIcon.sprite = SkillIcon.sprite;
            float real1 = info.info.skillinfo.Realcheck(1, info.Level);
            float real2 = info.info.skillinfo.Realcheck(2, info.Level);
            float real3 = info.info.skillinfo.Realcheck(3, info.Level);
            string s = string.Format(CsvManager.inst.GameText(info.info.skillinfo.Info), real1, real2, real3);
            string s1 = string.Format(CsvManager.inst.GameText(info.info.skillinfo.Info), info.info.skillinfo.Real1[0],
                info.info.skillinfo.Real1[1], info.info.skillinfo.Real1[2], info.info.skillinfo.Real2[0],
                info.info.skillinfo.Real2[1], info.info.skillinfo.Real2[2], info.info.skillinfo.Real3[0],
                info.info.skillinfo.Real3[1], info.info.skillinfo.Real3[2]);
            SkillInfo_Info1.text = s;
            SkillInfo_Info2.text = s1;
            ItemShow();
            UIManager.inst.CardInfoStart();
        }

        void ItemShow()
        {
            for (int i = 0; i < 3; i++)
            {
                ItemIcon[i].sprite = IconManager.inst.icon[230];
                if (info.Item[i] != -1)
                {
                    int itemicon = CsvManager.inst.itemInfo[info.Item[i]].Icon;

                    ItemIcon[i].sprite = IconManager.inst.icon[itemicon];
                }
            }
        }

        void TraitJobCheck()
        {
            if (info.Character_Job2 > 0)
            {
                int text = CsvManager.inst.TraitandJobName[info.Character_Job2];
                int icon = CsvManager.inst.TraitandJobIcon[info.Character_Job2];
                TraitJobIcon.sprite = IconManager.inst.icon[icon];
                TraitJobName.text = CsvManager.inst.GameText(text);
                TraitJob_ob.SetActive(true);
            }

            if (info.Character_trait2 > 0)
            {
                int text = CsvManager.inst.TraitandJobName[info.Character_trait2];
                int icon = CsvManager.inst.TraitandJobIcon[info.Character_trait2];
                TraitJobIcon.sprite = IconManager.inst.icon[icon];
                TraitJobName.text = CsvManager.inst.GameText(text);
                TraitJob_ob.SetActive(true);
            }
        }

        void HpMpset()
        {
            float Hp = info.stat.currentHp;
            float HpMax = info.stat.HpMax();
            float Mana = info.stat.currentMana;
            float ManaMax = info.stat.ManaMax();
            if (Hp == 0 || HpMax == 0)
            {
                HpSlider.value = 0;

            }
            else
            {
                float f1 = info.stat.currentHp / info.stat.HpMax();

                HpSlider.value = f1;
            }

            if (Mana == 0 || ManaMax == 0)
            {
                MpSlider.value = 0;

            }
            else
            {
                float f2 = info.stat.currentMana / info.stat.ManaMax();

                MpSlider.value = f2;
            }

            HpText.text = Hp.ToString("F0") + " / " + HpMax.ToString("F0");
            MpText.text = Mana.ToString("F0") + " / " + ManaMax.ToString("F0");

        }

        void Update()
        {
            if (Card == null)
            {
                gameObject.SetActive(false);
                return;
            }

            if (!Card.activeSelf)
            {
                gameObject.SetActive(false);
                return;
            }

            HpMpset();

        }

        public void SkillClick()
        {
            ItemInfo_ob.SetActive(false);
            SkillInfo_ob.SetActive(true);
        }

        public void ItemClick(int i)
        {
            if (info.Item[i] == -1)
            {
                return;
            }

            int idx = info.Item[i];
            ItemInfoName.text = CsvManager.inst.GameText(CsvManager.inst.itemInfo[idx].Name);
            ItemInfoIcon.sprite = IconManager.inst.icon[CsvManager.inst.itemInfo[idx].Icon];
            float real1 = CsvManager.inst.itemInfo[idx].Real[0];
            float real2 = CsvManager.inst.itemInfo[idx].Real[1];
            float real3 = CsvManager.inst.itemInfo[idx].Real[2];
            string s = string.Format(CsvManager.inst.GameText(CsvManager.inst.itemInfo[idx].Info), real1, real2, real3);
            ItemInfo_Info.text = s;
            //das
            SkillInfo_ob.SetActive(false);
            ItemInfo_ob.SetActive(true);
        }
    }
}