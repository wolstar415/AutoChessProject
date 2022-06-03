using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class CreateManager : MonoBehaviour
{
    public static CreateManager inst;

    private void Awake()
    {
        inst = this;
    }

    public bool CheckCreate(int idx)
    {
        bool result=CreateIdx()>=0;

        if (CreateLevelCheck(idx)) result = true;
        
        return result;
    }
    public bool CreateLevelCheck(int idx)
    {
        bool result=false;

        int cnt = 0;
        for (int i = 0; i < PlayerInfo.Inst.PlayerCardCntLv[idx].Lv1.Count; i++)
        {
            if (PlayerInfo.Inst.PlayerCardCntLv[idx].Lv1[i].TryGetComponent(out Card_Info info))
            {
                bool b = info.IsFiled;
                if (GameSystem_AllInfo.inst.IsBattle==true)
                {
                    if (b==false)
                    {
                        cnt++;
                    }
                }
                else
                {
                    cnt++;
                }

            }

        }
        if (cnt==2)
        {
            result = true;
        }
        return result;
    }
    public int CreateIdx()
    {
        int result = -1;
        for (int i = 0; i < 9; i++)
        {
            if (PlayerInfo.Inst.PlayerTilestate[i]==-1)
            {
                result = i;
                break;
            }
        }

        return result;
    }

    public void CreateCharacter(int idx)
    {
        int CreatePosidx = CreateIdx();
        if (CreateLevelCheck(idx))
        {
            CreateLevelUp(idx);
        }
        else if (CreatePosidx >= 0)
        {
            string s = GameSystem_AllInfo.inst.Cards[idx];
            GameObject tile = PlayerInfo.Inst.PlayerTile[CreatePosidx];
            Vector3 Pos = tile.transform.position;
            Pos.y = 1.6f;
            GameObject ob =PhotonNetwork.Instantiate(s, Pos, Quaternion.identity);
            if (tile.TryGetComponent(out TileInfo info))
            {
                info.AddTile(ob);
            }
        }
    }

    public void CreateLevelUp(int idx)
    {
        List<GameObject> obs = new List<GameObject>();
        for (int i = 0; i < PlayerInfo.Inst.PlayerCardCntLv[idx].Lv1.Count; i++)
        {
            obs.Add(PlayerInfo.Inst.PlayerCardCntLv[idx].Lv1[i]);
        }

            GameObject target = obs[0];
            GameObject remove = obs[1];
            if (obs[1].TryGetComponent(out Card_Info info))
            {
                if (info.IsFiled)
                {
                    target = obs[1];
                    remove = obs[0];
                }
            
            }

            
            
            if (target.TryGetComponent(out Card_Info targetinfo))
            {
                targetinfo.LevelUp();
            }
            
            
                
            if (remove.TryGetComponent(out Card_Info removeinfo))
            {
                removeinfo.ItemMove(target);
                removeinfo.remove();
            }
            
        

    }

    public void CheckLevelUp(int idx,int lv)
    {
        List<GameObject> obs = new List<GameObject>();
        for (int i = 0; i < PlayerInfo.Inst.PlayerCardCntLv[idx].Lv(lv).Count; i++)
        {
            obs.Add(PlayerInfo.Inst.PlayerCardCntLv[idx].Lv(lv)[i]);
        }

        if (obs.Count<3)
        {
            return;
        }

        GameObject target=obs[0];

        for (int i = 0; i < obs.Count; i++)
        {
            if (obs[i].TryGetComponent(out Card_Info info))
            {
                if (info.IsFiled)
                {
                    target = obs[i];
                }
            }
        }

        obs.Remove(target);
        if (target.TryGetComponent(out Card_Info targetinfo))
        {
            targetinfo.LevelUp();
        }
        if (obs[0].TryGetComponent(out Card_Info remove1))
        {
            remove1.ItemMove(target);
            remove1.remove();
        }
        if (obs[1].TryGetComponent(out Card_Info remove2))
        {
            remove2.ItemMove(target);
            remove2.remove();
        }
    }
    
}
