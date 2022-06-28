using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace GameS
{
    public class TraitJobManager : MonoBehaviour
    {
        public static TraitJobManager inst;
        [SerializeField]  private GameObject Prefab;
        [SerializeField]  private GameObject parent;
        public List<GameObject> Obs;
        public List<GameObject> orberList;


        private void Awake()
        {
            inst = this;
        }
        private void Start()
        {
            for (int i = 0; i < CsvManager.inst.TraitandJobInfo.Count; i++)
            {
                GameObject ob = Instantiate(Prefab,parent.transform);
                ob.SetActive(false);
                if (ob.TryGetComponent(out TraitJobInfo info))
                {
                    int icon = CsvManager.inst.TraitandJobIcon[i];
                    int name = CsvManager.inst.TraitandJobName[i];
                    info.Idx = i;
                    info.CntIf[0] = CsvManager.inst.TraitandJobCnt1[i];
                    info.CntIf[1] = CsvManager.inst.TraitandJobCnt2[i];
                    info.CntIf[2] = CsvManager.inst.TraitandJobCnt3[i];
                    info.CntIf[3] = CsvManager.inst.TraitandJobCnt4[i];
                    info.CntIf[4] = CsvManager.inst.TraitandJobCnt5[i];
                    info.icon.sprite = IconManager.inst.icon[icon];
                    info.icon.name = CsvManager.inst.GameText(name);
                }
                Obs.Add(ob);
            }
            //생성..
        }
        public void TraitJobAdd(int idx)
        {
            if (idx==0)
            {
                return;
            }

            //PlayerInfo.Inst.TraitandJobCnt[idx]++;
            PlayerInfo.Inst.TraitandJobFunc(true, idx);
            if (Obs[idx].TryGetComponent(out TraitJobInfo info))
            {
                info.CntAdd();
            }
        }

        public void TraitJobRemove(int idx)
        {
            if (idx==0)
            {
                return;
            }
            //PlayerInfo.Inst.TraitandJobCnt[idx]--;
            PlayerInfo.Inst.TraitandJobFunc(false, idx);
            if (Obs[idx].TryGetComponent(out TraitJobInfo info))
            {
                info.CntRemove();
            }
        }

        public void OrderList()
        {
            orberList = Obs.Where(n =>
            {
                int i=0;
                if (n.TryGetComponent(out TraitJobInfo info))
                {
                    i = info.Cnt;
                }

                return i > 0;
            }).OrderByDescending(n=>n.GetComponent<TraitJobInfo>().LevelCheck())
                .ThenByDescending(n =>
            {
                int i=0;
                if (n.TryGetComponent(out TraitJobInfo info))
                {
                    i = info.Cnt;
                }

                return i;
            }).ToList();

            for (int i = 0; i < orberList.Count; i++)
            {
                orberList[i].transform.SetSiblingIndex(i);
            }
            
        }

    }
}
