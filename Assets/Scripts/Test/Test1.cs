
using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Random = UnityEngine.Random;

public class Test1 : MonoBehaviour
{
    public float f = 10f;
    public float culTime = 0;
    public void Update()
    {
        if (f>5)
        {
            culTime += Time.deltaTime;
        f = Mathf.Lerp(10, 5, culTime/5f);
        Debug.Log(f);
        }
    }

    public void S1()
    {
        string s = ((int)Random.Range(0, 100)).ToString();
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // You would have to set the Receivers to All in order to receive this event on the local client as well
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(0, s, RaiseEventOptions.Default, SendOptions.SendUnreliable);

    }
}
