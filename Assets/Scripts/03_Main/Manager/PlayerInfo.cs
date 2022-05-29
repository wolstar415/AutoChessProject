using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public static PlayerInfo Inst;
    public int Gold;
    public int Life;
    public int PlayerIdx;
    public GameObject PlayerCharacter;
    public bool Dead = false;

    public GameObject FiledTileOb;
    public GameObject PlayerTileOb;
    public List<GameObject> PlayerTile;
    public List<GameObject> FiledTile;
    public List<GameObject> EnemyTile;
    private void Awake()
    {
        Inst = this;
    }
}
