using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerPositionInfo
{
    public List<GameObject> PlayerTile;
    public List<GameObject> EnemyPlayerTile;
    public List<GameObject> FiledTile;
    public List<GameObject> EnemyFiledTile;
    public Transform PlayerMovePos;
    public Transform EnemyPlayerMovePos;
    public List<GameObject> GoldOb;
    public List<GameObject> EnemyGoldOb;

    public GameObject FiledTileOb;
    public GameObject PlayerTileob;
    public GameObject EnemyPlayerTileob;
}
public class PositionManager : MonoBehaviour
{
    public static PositionManager inst;
    
    public Transform[] PickPos;

    [Header("Camera Pos")] 
    public Transform[] Camera_PickPos;

    public Transform[] Camera_Pos;
    public Transform[] Camera_AttackPos;

    public List<PlayerPositionInfo> playerPositioninfo;

    private void Awake()
    {
        inst = this;
    }
}
