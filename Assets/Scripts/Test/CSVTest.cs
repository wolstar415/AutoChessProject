using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CSVTest : MonoBehaviour
{
    public List<String> CharactersName;
    public string[] row;

    public string[] data;

    //private string URL ="https://docs.google.com/spreadsheets/d/1tqaUzgkVwxE2bLmNdsPophYnCnuGFQbiGWfsFSVdln8/gviz/tq?tqx=out:csv&sheet={캐릭터}";
    private string[] URL =
    {

        "https://docs.google.com/spreadsheets/d/1tqaUzgkVwxE2bLmNdsPophYnCnuGFQbiGWfsFSVdln8/export?format=csv&gid=0",
        "https://docs.google.com/spreadsheets/d/1tqaUzgkVwxE2bLmNdsPophYnCnuGFQbiGWfsFSVdln8/export?format=csv&gid=138748527",
        "https://docs.google.com/spreadsheets/d/1tqaUzgkVwxE2bLmNdsPophYnCnuGFQbiGWfsFSVdln8/export?format=csv&gid=972189345",
        "https://docs.google.com/spreadsheets/d/1tqaUzgkVwxE2bLmNdsPophYnCnuGFQbiGWfsFSVdln8/export?format=csv&gid=157520444",
        "https://docs.google.com/spreadsheets/d/1tqaUzgkVwxE2bLmNdsPophYnCnuGFQbiGWfsFSVdln8/export?format=csv&gid=1562761835",
        "https://docs.google.com/spreadsheets/d/1tqaUzgkVwxE2bLmNdsPophYnCnuGFQbiGWfsFSVdln8/export?format=csv&gid=638159907",
        "https://docs.google.com/spreadsheets/d/1tqaUzgkVwxE2bLmNdsPophYnCnuGFQbiGWfsFSVdln8/export?format=csv&gid=747828469",
        "https://docs.google.com/spreadsheets/d/1tqaUzgkVwxE2bLmNdsPophYnCnuGFQbiGWfsFSVdln8/export?format=csv&gid=1866138935",
        "https://docs.google.com/spreadsheets/d/1tqaUzgkVwxE2bLmNdsPophYnCnuGFQbiGWfsFSVdln8/export?format=csv&gid=306255472",
        "https://docs.google.com/spreadsheets/d/1tqaUzgkVwxE2bLmNdsPophYnCnuGFQbiGWfsFSVdln8/export?format=csv&gid=1583924170",


    };
    //private string URL2 ="https://docs.google.com/spreadsheets/d/1tqaUzgkVwxE2bLmNdsPophYnCnuGFQbiGWfsFSVdln8/export?format=csv&gid=138748527";
    //private string URL3 ="https://docs.google.com/spreadsheets/d/1tqaUzgkVwxE2bLmNdsPophYnCnuGFQbiGWfsFSVdln8/export?format=csv&gid=972189345";
    //private string URL4 ="https://docs.google.com/spreadsheets/d/1tqaUzgkVwxE2bLmNdsPophYnCnuGFQbiGWfsFSVdln8/export?format=csv&gid=157520444";
    //private string URL5 ="https://docs.google.com/spreadsheets/d/1tqaUzgkVwxE2bLmNdsPophYnCnuGFQbiGWfsFSVdln8/export?format=csv&gid=1562761835";
    //private string URL6 ="https://docs.google.com/spreadsheets/d/1tqaUzgkVwxE2bLmNdsPophYnCnuGFQbiGWfsFSVdln8/export?format=csv&gid=638159907";
    //private string URL7 ="https://docs.google.com/spreadsheets/d/1tqaUzgkVwxE2bLmNdsPophYnCnuGFQbiGWfsFSVdln8/export?format=csv&gid=747828469";
    //private string URL8 ="https://docs.google.com/spreadsheets/d/1tqaUzgkVwxE2bLmNdsPophYnCnuGFQbiGWfsFSVdln8/export?format=csv&gid=1866138935";
    //private string URL9 ="https://docs.google.com/spreadsheets/d/1tqaUzgkVwxE2bLmNdsPophYnCnuGFQbiGWfsFSVdln8/export?format=csv&gid=306255472";
    //private string URL10 ="https://docs.google.com/spreadsheets/d/1tqaUzgkVwxE2bLmNdsPophYnCnuGFQbiGWfsFSVdln8/export?format=csv&gid=1583924170";

    //private string URL ="https://docs.google.com/spreadsheets/d/1tqaUzgkVwxE2bLmNdsPophYnCnuGFQbiGWfsFSVdln8/export?format=csv";
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



        //var csvdata = CSVReader.Read2(data1);
        //Debug.Log((csvdata));

        // for (int i = 0; i < csvdata.Count; i++)
        // {
        //     CharactersName.Add((csvdata[i]["Name"].ToString()));
        //
        //
        //
        // }




    }

    void CharacherSeet()
    {
        
    }
    
}
