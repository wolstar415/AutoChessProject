using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

using System.Linq;
using System.Runtime.InteropServices;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using Newtonsoft.Json;

public class Test2 : MonoBehaviourPunCallbacks
{
    public Test2 test;
    public List<int> asd;
    int zxc = 0;
    public int[] asd2;


    public GameObject[] asd3;
    public PhotonView pv;
    public Dictionary<int, int> dic;
    public string s;
    public Test2 test2;
    private void Start()
    {
        s = JsonUtility.ToJson(test);
        dic=new Dictionary<int, int>();
        dic.Add(0, 100);
        dic.Add(1, 1000);
    }

    [PunRPC]
    void xcz(Dictionary<int, int> hhhh)
    {
        Debug.Log(hhhh[0]);
        
    }
    public void show()
    {
        byte[] fcv = SerializeMethod(asd3[0]);
        JsonUtility.FromJsonOverwrite(s,test2);
        //Debug.Log(gfd.asd3[0].name);
    }

    public void bt()
    {


    }



}
