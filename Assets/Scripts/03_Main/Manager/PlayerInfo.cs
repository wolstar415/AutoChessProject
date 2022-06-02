using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class PlayerCardCnt
{
    public List<GameObject> Lv1;
    public List<GameObject> Lv2;
    public List<GameObject> Lv3;

    public List<GameObject> Lv(int lv)
    {
        if (lv==2)
        {
            return Lv2;
        }
        else if (lv==3)
        {
            return Lv3;
        }

        return Lv1;
    }
}
public class PlayerInfo : MonoBehaviour
{
    public GameObject PlayerOb;
    public static PlayerInfo Inst;
    [SerializeField]private int gold;
    public int Level;
    
    public int Life;
    public int PlayerIdx;
    public GameObject PlayerCharacter;
    public bool Dead = false;
    public bool IsLock=false;
    [SerializeField]private int _food=0;
    [SerializeField]private int _foodMax=0;
    public int Xp;
    public int XpMax;
    
    public GameObject FiledTileOb;
    public GameObject PlayerTileOb;
    public List<GameObject> PlayerTile;
    public List<GameObject> FiledTile;
    public List<GameObject> EnemyTile;
    public int[] PlayerTilestate;
    public int[] FiledTilestate;
    public int[] TraitandJobCnt;
    
    public int[] PlayerCardCnt;

    public List<PlayerCardCnt> PlayerCardCntLv;

    public Transform PlayerMovePos;

    private void Awake()
    {
        
        Inst = this;
        //테스트
        Level = 1;
    }

    private void Start()
    {
        Gold = 100;
        
    }


    public int Gold
    {
        get { return gold;}
        set
        {
            if (value<0)
            {
                value = 0;
            }

            gold = value;
            UIManager.inst.GoldSet();
        }
    }

    public int food
    {
        get { return _food;}
        set
        {
            _food = value;
            UIManager.inst.FoodSet();
        }
    }

    public int foodMax
    {
        get { return _foodMax;}
        set 
        {
            _foodMax = value;
            UIManager.inst.FoodSet();
        }
    }



}
