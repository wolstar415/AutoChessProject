using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public string GameLanguage;
    public static GameManager inst;
    public string OriNickName;
    public string NickName;
    

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
