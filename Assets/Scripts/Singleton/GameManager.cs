using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public string GameLanguage;
    public static GameManager inst;
    public string OriNickName;
    public int CharIdx = 0;
    public AudioSource audioSource;
    public Sprite[] charIcons;
    
    [Header("플레이어 정보")]
    public string NickName;

    public int Victory1;
    public int Victory2;
    public int Victory3;
    public int Victory4;
    public int Victory5;
    public int Victory6;
    public int Victory7;
    public int Victory8;
    public int Score;

    private void Awake()
    {
        inst = this;
    }

    private void Start()
    {
        SystemLanguage lang = Application.systemLanguage;

            switch (lang)
            {
                case SystemLanguage.Korean:
                    GameLanguage = "Korean";
                    break;

                case SystemLanguage.English:
                    GameLanguage = "English";
                    break;

                case SystemLanguage.Japanese:
                    GameLanguage = "Japan";
                    break;
                case SystemLanguage.Chinese:
                    GameLanguage = "China";
                    break;
                case SystemLanguage.German:
                    GameLanguage = "Germany";
                    break;
                case SystemLanguage.Spanish:
                    GameLanguage = "Spain";
                    break;
                case SystemLanguage.Portuguese:
                    GameLanguage = "Portugal";
                    break;
                case SystemLanguage.Swedish:
                    GameLanguage = "Sweden";
                    break;
                case SystemLanguage.Italian:
                    GameLanguage = "Italy";
                    break;
                case SystemLanguage.Ukrainian:
                    GameLanguage = "ukr";
                    break;
                case SystemLanguage.Russian:
                    GameLanguage = "rus";
                    break;
                case SystemLanguage.Thai:
                    GameLanguage = "tha";
                    break;
                case SystemLanguage.Polish:
                    GameLanguage = "pol";
                    break;
                case SystemLanguage.French:
                    GameLanguage = "fra";
                    break;
                case SystemLanguage.Turkish:
                    GameLanguage = "tur";
                    break;
            }
            if (GameLanguage == "")
            {
                GameLanguage = "English";
            }
    }
}
