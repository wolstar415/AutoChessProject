using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;



[System.Serializable]
public class SkillInfo
{
    public int Idx;
    public int Name;
    public int Icon;
    public int Info;
    public float[] Real1 = new float[3];
    public float[] Real2 = new float[3];
    public float[] Real3 = new float[3];
}

[System.Serializable]
public class CharacterInfo
{
    public int Idx;
    public int Level;
    public int Name;
    public int Icon;
    public int Food;
    public int Trait1;
    public int Trait2;
    public int Job1;
    public int Job2;
    public float Range;
    public bool IsRange;
    public float[] Hp = new float[3];
    public float[] At = new float[3];
    public float[] AtSpeed = new float[3];
    public float Defense;
    public float MagicDefense;
    public float Speed;
    public float Mana;
    public float Mana_Max;
    public SkillInfo skillinfo;
}

[System.Serializable]
public class ItemInfo
{
    public int Name;
    public int Info;
    public int Icon;
    public float Hp;
    public float Attack;
    public float AtkSpeed;
    public float Defense;
    public float MagicDefense;
    public float MagicAtk;
    public float Mana;
    public float CriPer;
    public float CriDmg;
    public float MissPer;
}

public class CsvManager : MonoBehaviour
{
    public static CsvManager inst;
    public string[] row;

    public string[] data;
    [Header("캐릭터")] public List<CharacterInfo> characterInfo;
    [Header("스킬")] public List<SkillInfo> skillinfo;

    [Header("특성")] public List<int> TraitandJobName;
    public List<int> TraitandJobInfo;
    public List<int> TraitandJobInfo1;

    [Header("가격")] public List<int> Cost1;
    public List<int> Cost2;
    public List<int> Cost3;
    [Header("아이템")] public List<ItemInfo> itemInfo;
    [Header("리롤확률")] public List<int> ReRoll1;
    public List<int> ReRoll2;
    public List<int> ReRoll3;
    public List<int> ReRoll4;
    public List<int> ReRoll5;

    [Header("라운드")] public List<int> RoundCheck1;
    public List<int> RoundCheck2;
    public List<int> RoundCheck3;
    [Header("데미지")] public List<int> RoundDamage;
    [Header("XP")] public List<int> Player_Xp;
    [Header("CardMax")] public List<int> CardMax;
    [Header("GameText")] public List<String> Text_Korea;
    public List<String> Text_English;
    public List<String> Text_Japan;
    public List<String> Text_China;

    //private string URL ="https://docs.google.com/spreadsheets/d/1tqaUzgkVwxE2bLmNdsPophYnCnuGFQbiGWfsFSVdln8/gviz/tq?tqx=out:csv&sheet={캐릭터}";
    [SerializeField] private string[] URL;

    private void Awake()
    {
        inst = this;
    }

    private void Start()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            StartCoroutine((DataUpdate()));
        }
    }

    IEnumerator DataUpdate()
    {
        for (int i = 0; i < URL.Length; i++)
        {
            UnityWebRequest www = UnityWebRequest.Get(URL[i]);
            yield return www.SendWebRequest();
            data[i] = www.downloadHandler.text;
        }

        yield return null;
        SkillSeet();
        CharacherSeet();
        TraitJobSeet();
        CostSeet();
        ItemSeet();
        ReRoolSeet();
        RoundSeet();
        DamageSeet();
        XpSeet();
        CardMaxSeet();
        GameTextSeet();
        test();
    }

    void SkillSeet()
    {
        var csvdata = CSVReader.Read2(data[1]);
        for (int i = 0; i < csvdata.Count; i++)
        {
            skillinfo.Add(new SkillInfo());
            skillinfo[i].Idx = i;
            skillinfo[i].Icon = int.Parse(csvdata[i]["Icon"].ToString());
            skillinfo[i].Name = int.Parse(csvdata[i]["Name"].ToString());
            skillinfo[i].Info = int.Parse(csvdata[i]["Info"].ToString());
            skillinfo[i].Real1[0] = float.Parse(csvdata[i]["1_Real1"].ToString());
            skillinfo[i].Real1[1] = float.Parse(csvdata[i]["2_Real1"].ToString());
            skillinfo[i].Real1[2] = float.Parse(csvdata[i]["3_Real1"].ToString());
            skillinfo[i].Real2[0] = float.Parse(csvdata[i]["1_Real2"].ToString());
            skillinfo[i].Real2[1] = float.Parse(csvdata[i]["2_Real2"].ToString());
            skillinfo[i].Real2[2] = float.Parse(csvdata[i]["3_Real2"].ToString());
            skillinfo[i].Real3[0] = float.Parse(csvdata[i]["1_Real3"].ToString());
            skillinfo[i].Real3[1] = float.Parse(csvdata[i]["2_Real3"].ToString());
            skillinfo[i].Real3[2] = float.Parse(csvdata[i]["3_Real3"].ToString());
        }
    }

    void CharacherSeet()
    {
        var csvdata = CSVReader.Read2(data[0]);
        for (int i = 0; i < csvdata.Count; i++)
        {
            characterInfo.Add(new CharacterInfo());
            characterInfo[i].Idx = i;
            characterInfo[i].Level = int.Parse(csvdata[i]["Level"].ToString());
            characterInfo[i].Icon = int.Parse(csvdata[i]["Icon"].ToString());
            characterInfo[i].Food = int.Parse(csvdata[i]["Food"].ToString());
            characterInfo[i].Name = int.Parse(csvdata[i]["Name"].ToString());
            characterInfo[i].Trait1 = int.Parse(csvdata[i]["특성1"].ToString());
            characterInfo[i].Trait2 = int.Parse(csvdata[i]["특성2"].ToString());
            characterInfo[i].Job1 = int.Parse(csvdata[i]["계열1"].ToString());
            characterInfo[i].Job2 = int.Parse(csvdata[i]["계열2"].ToString());
            characterInfo[i].Range = float.Parse(csvdata[i]["사거리"].ToString());
            characterInfo[i].IsRange = Convert.ToBoolean(int.Parse(csvdata[i]["공격타입"].ToString()));
            characterInfo[i].Hp[0] = float.Parse(csvdata[i]["체력1"].ToString());
            characterInfo[i].Hp[1] = float.Parse(csvdata[i]["체력2"].ToString());
            characterInfo[i].Hp[2] = float.Parse(csvdata[i]["체력3"].ToString());
            characterInfo[i].AtSpeed[0] = float.Parse(csvdata[i]["공속1"].ToString());
            characterInfo[i].AtSpeed[1] = float.Parse(csvdata[i]["공속2"].ToString());
            characterInfo[i].AtSpeed[2] = float.Parse(csvdata[i]["공속3"].ToString());
            characterInfo[i].At[0] = float.Parse(csvdata[i]["공격력1"].ToString());
            characterInfo[i].At[1] = float.Parse(csvdata[i]["공격력2"].ToString());
            characterInfo[i].At[2] = float.Parse(csvdata[i]["공격력3"].ToString());
            characterInfo[i].Defense = float.Parse(csvdata[i]["방어력"].ToString());
            characterInfo[i].MagicDefense = float.Parse(csvdata[i]["마법저항력"].ToString());
            characterInfo[i].Speed = float.Parse(csvdata[i]["이속"].ToString());
            characterInfo[i].Mana = float.Parse(csvdata[i]["기본마나"].ToString());
            characterInfo[i].Mana_Max = float.Parse(csvdata[i]["마나"].ToString());
            characterInfo[i].skillinfo = skillinfo[i];
        }
    }

    void TraitJobSeet()
    {
        var csvdata = CSVReader.Read2(data[2]);
        for (int i = 0; i < csvdata.Count; i++)
        {
            TraitandJobName.Add(int.Parse(csvdata[i]["Name"].ToString()));
            TraitandJobInfo1.Add(int.Parse(csvdata[i]["설명1"].ToString()));
        }
    }

    void CostSeet()
    {
        var csvdata = CSVReader.Read2(data[3]);
        for (int i = 0; i < csvdata.Count; i++)
        {
            Cost1.Add(int.Parse(csvdata[i]["Cost1"].ToString()));
            Cost2.Add(int.Parse(csvdata[i]["Cost2"].ToString()));
            Cost3.Add(int.Parse(csvdata[i]["Cost3"].ToString()));

            //characterInfo[i].Level = int.Parse(csvdata[i]["Level"].ToString());
        }
    }

    void ItemSeet()
    {
        var csvdata = CSVReader.Read2(data[4]);
        for (int i = 0; i < csvdata.Count; i++)
        {
            itemInfo.Add(new ItemInfo());
            itemInfo[i].Name = int.Parse(csvdata[i]["Name"].ToString());
            itemInfo[i].Info = int.Parse(csvdata[i]["Info"].ToString());
            itemInfo[i].Icon = int.Parse(csvdata[i]["Icon"].ToString());
            itemInfo[i].Hp = float.Parse(csvdata[i]["체력"].ToString());
            itemInfo[i].Attack = float.Parse(csvdata[i]["공격력"].ToString());
            itemInfo[i].AtkSpeed = float.Parse(csvdata[i]["공속"].ToString());
            itemInfo[i].Defense = float.Parse(csvdata[i]["방어"].ToString());
            itemInfo[i].MagicDefense = float.Parse(csvdata[i]["마법방어"].ToString());
            itemInfo[i].MagicAtk = float.Parse(csvdata[i]["마법공격력"].ToString());
            itemInfo[i].Mana = float.Parse(csvdata[i]["마나"].ToString());
            itemInfo[i].CriPer = float.Parse(csvdata[i]["치명타확률"].ToString());
            itemInfo[i].CriDmg = float.Parse(csvdata[i]["치명타확률데미지"].ToString());
            itemInfo[i].MissPer = float.Parse(csvdata[i]["회피"].ToString());

            //characterInfo[i].Level = int.Parse(csvdata[i]["Level"].ToString());
        }
    }

    void ReRoolSeet()
    {
        var csvdata = CSVReader.Read2(data[5]);
        for (int i = 0; i < csvdata.Count; i++)
        {
            ReRoll1.Add(int.Parse(csvdata[i]["티어1"].ToString()));
            ReRoll2.Add(int.Parse(csvdata[i]["티어2"].ToString()));
            ReRoll3.Add(int.Parse(csvdata[i]["티어3"].ToString()));
            ReRoll4.Add(int.Parse(csvdata[i]["티어4"].ToString()));
            ReRoll5.Add(int.Parse(csvdata[i]["티어5"].ToString()));
        }
    }

    void RoundSeet()
    {
        var csvdata = CSVReader.Read2(data[6]);
        for (int i = 0; i < csvdata.Count; i++)
        {
            RoundCheck1.Add(int.Parse(csvdata[i]["Round"].ToString()));
            RoundCheck2.Add(int.Parse(csvdata[i]["Round2"].ToString()));
            RoundCheck3.Add(int.Parse(csvdata[i]["선택"].ToString()));
        }
    }

    void DamageSeet()
    {
        var csvdata = CSVReader.Read2(data[7]);
        for (int i = 0; i < csvdata.Count; i++)
        {
            RoundDamage.Add(int.Parse(csvdata[i]["Damage"].ToString()));
        }
    }

    void XpSeet()
    {
        var csvdata = CSVReader.Read2(data[9]);
        for (int i = 0; i < csvdata.Count; i++)
        {
            Player_Xp.Add(int.Parse(csvdata[i]["XP"].ToString()));
        }
    }

    void CardMaxSeet()
    {
        var csvdata = CSVReader.Read2(data[8]);
        for (int i = 0; i < csvdata.Count; i++)
        {
            CardMax.Add(int.Parse(csvdata[i]["Max"].ToString()));
        }
    }

    void GameTextSeet()
    {
        var csvdata = CSVReader.Read2(data[10]);
        for (int i = 0; i < csvdata.Count; i++)
        {
            Text_Korea.Add(csvdata[i]["Korean"].ToString());
            Text_English.Add(csvdata[i]["English"].ToString());
            Text_Japan.Add(csvdata[i]["Japan"].ToString());
            Text_China.Add(csvdata[i]["China"].ToString());
        }
    }

    void test()
    {
        SceneManager.LoadScene("03_Main");
    }
}