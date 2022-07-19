using System;
using System.Collections;
using System.Collections.Generic;
using Singleton1;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public string GameLanguage;
    public string OriNickName;
    public int CharIdx = 0;
    public AudioSource audioSource;
    public Sprite[] charIcons;
    
    [Header("플레이어 정보 서버저장")]
    public string NickName;

    public int Victory1; //1등
    public int Victory2; //2등
    public int Victory3; // 3등
    public int Victory4; //4등
    public int Victory5; // 5등
    public int Victory6; //6등
    public int Victory7; //7등
    public int Victory8; //8등
    public int Score; //점수

    

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
