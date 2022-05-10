using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem_AllInfo : MonoBehaviour
{
    public static GameSystem_AllInfo inst;
    public Transform[] StartPos;
    public Transform[] CameraPos_At;
    public Transform[] CameraPos_De;
    public Transform PickPos;

    private void Awake()
    {
        inst = this;
    }
}
