using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class add_ButtonS : MonoBehaviour
{
    public TextMeshProUGUI Text1;
    public TextMeshProUGUI Text2;

    public GameObject meob;

    private void Start()
    {
        Text1.text = CsvManager.inst.GameText(514);
        Text2.text = CsvManager.inst.GameText(515);
    }

    public void btn1()
    {
        meob.SetActive(false);
    }
}
