
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Test1 : MonoBehaviour
{
    public void S1()
    {
        string s = ((int)Random.Range(0, 100)).ToString();
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // You would have to set the Receivers to All in order to receive this event on the local client as well
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(0, s, RaiseEventOptions.Default, SendOptions.SendUnreliable);

    }
}
