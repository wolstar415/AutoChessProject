using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GameS
{
    [System.Serializable]
    public class BattleInfo
    {
        public int playeridx; //플레이어번호
        public int enemyidx; //싸워야할 상대
        public bool IsBattleMove; //이동해야하는지 체크
        public bool IsCopy; //내가 복사본을 보내는지 체크
        public int copyidx; //싸워야할 상대(복사본을 보내는곳)
    }

    [System.Serializable]
    public struct PlayerJobCheck
    {
        public int[] JobAndTrait;
    }

    public class GameSystem_AllInfo : MonoBehaviour
    {
        public bool IsStart = false;
        public GameObject Black;
        public static GameSystem_AllInfo inst;
        public Transform[] StartPos;
        public Transform[] CameraPos_At;
        public Transform[] CameraPos_De;
        public Transform PickPos;
        public LayerMask[] masks;
        public bool[] playerdead;

        public Transform TextUIparent;

        public List<string> Cards;
        public List<GameObject> Card_1;
        public List<GameObject> Card_2;
        public List<GameObject> Card_3;
        public List<GameObject> Card_4;
        public List<GameObject> Card_5;

        public Transform mainCanvas;
        public Transform itemMoveTrans;

        public List<GameObject> PickCard;

        public List<GameObject> pickNoMove;
        //public bool IsBattle = false;

        [Header("UI정보들")] public Transform ItemCanvs;
        public Transform ItemParent;
        public GridLayoutGroup ItemGridLayout;

        [Header("전투 플레이어")] public List<BattleInfo> battleinfos;

        [Header("플레이어 특성계열")] public List<PlayerJobCheck> playerJobcnt;

        private void Awake()
        {
            inst = this;
            Black.SetActive(true);
        }

        public void StartFunc()
        {
            IsStart = true;
            Black.SetActive(false);
        }

        public GameObject FindNearestObject(Vector3 pos, GameObject[] Obs)
        {
            // LINQ 메소드를 이용해 가장 가까운 적을 찾습니다.
            var neareastObject = Obs
                .OrderBy(obj => { return Vector3.Distance(pos, obj.transform.position); })
                .FirstOrDefault();

            return neareastObject;
        }

        public GameObject FindNearestObject(Vector3 pos, Collider[] Obs)
        {
            // LINQ 메소드를 이용해 가장 가까운 적을 찾습니다.
            var neareastObject = Obs
                .OrderBy(obj => { return Vector3.Distance(pos, obj.transform.position); })
                .FirstOrDefault();

            return neareastObject.gameObject;
        }

        public List<GameObject> CardList(int Lv)
        {
            switch (Lv)
            {
                case 1:
                    return Card_1;
                    break;
                case 2:
                    return Card_2;
                    break;
                case 3:
                    return Card_3;
                    break;
                case 4:
                    return Card_4;
                    break;
                case 5:
                    return Card_5;
                    break;
                default:
                    break;
            }

            return Card_1;
        }

        public List<int> CardPickCnt(int Lv, int num)
        {
            List<int> result = new List<int>();
            List<GameObject> dummyCard = CardList(Lv).ToList();

            for (int i = 0; i < num; i++)
            {
                int ran = Random.Range(0, dummyCard.Count);
                int idx = 0;
                if (dummyCard[ran].TryGetComponent(out Card_Info info))
                {
                    idx = info.Idx;
                }

                result.Add(idx);
                dummyCard.RemoveAt(ran);
            }

            return result;
        }

        public GameObject FindFirstObject(Vector3 pos, int Findidx, float dis, bool Nearest)
        {
            // Collider[] results = new Collider[20];
            // var size = Physics.OverlapSphereNonAlloc(pos, dis, results, GameSystem_AllInfo.inst.masks[Findidx]);
            //
            // if (size == 0||results.Length==0)
            // {
            //     return null;
            // }
            Collider[] c = Physics.OverlapSphere(pos, dis, GameSystem_AllInfo.inst.masks[Findidx]);

            if (c.Length==0)
            {
                return null;
            }
            if (Nearest)
            {
                c = c.Where(ob => ob.GetComponent<UnitState>().IsDead == false &&
                                              ob.GetComponent<Card_Info>().IsFiled &&
                                              ob.GetComponent<UnitState>().IsInvin == 0)
                    .OrderBy(ob => Vector3.Distance(pos, ob.transform.position)).ToArray();
            }
            else
            {
                c = c.Where(ob => ob.GetComponent<UnitState>().IsDead == false &&
                                              ob.GetComponent<UnitState>().IsInvin == 0 &&
                                              ob.GetComponent<Card_Info>().IsFiled)
                    .OrderByDescending(ob => Vector3.Distance(pos, ob.transform.position)).ToArray();
            }


            if (c.Length > 0)
            {
                return c[0].gameObject;
            }

            return null;
        }

        public GameObject FindRandomObject(Vector3 pos, int Findidx, float f)
        {
            Collider[] c = Physics.OverlapSphere(pos, f, GameSystem_AllInfo.inst.masks[Findidx]);
            c = c.Where(ob => ob.GetComponent<UnitState>().IsDead == false && ob.GetComponent<Card_Info>().IsFiled &&
                              ob.GetComponent<UnitState>().IsInvin == 0).ToArray();


            if (c.Length > 0)
            {
                int ran = Random.Range(0, c.Length);
                return c[ran].gameObject;
            }

            return null;
        }

        public List<GameObject> FindAllObject(Vector3 pos, int Findidx, float f)
        {
            Collider[] c = Physics.OverlapSphere(pos, f, GameSystem_AllInfo.inst.masks[Findidx]);
            List<GameObject> Dummy = c.Where(ob =>
                ob.GetComponent<UnitState>().IsDead == false && ob.GetComponent<Card_Info>().IsFiled &&
                ob.GetComponent<UnitState>().IsInvin == 0).Select(x => x.gameObject).ToList();


            return Dummy;
        }
    }
}