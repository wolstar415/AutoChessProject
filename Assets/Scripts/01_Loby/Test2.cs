using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class Test2 : MonoBehaviourPunCallbacks,IOnEventCallback
{
    public void OnEvent(EventData photonEvent)
    {

        if ((photonEvent.Code == 0))
        {
            Debug.Log("2¹ø"+photonEvent.CustomData);
        }
    }
}
