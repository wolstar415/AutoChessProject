using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconManager : MonoBehaviour
{
    public static IconManager inst;

    public Sprite[] icon;
    private void Awake()
    {
        inst = this;
    }

}
