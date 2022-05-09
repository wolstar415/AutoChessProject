using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem_AllInfo : MonoBehaviour
{
    public static GameSystem_AllInfo inst;
    public Transform[] StartPos;
    public Transform[] CameraPos;

    private void Awake()
    {
        inst = this;
    }
}
