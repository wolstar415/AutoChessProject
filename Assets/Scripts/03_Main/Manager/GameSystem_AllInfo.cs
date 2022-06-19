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
        public int playeridx;//플레이어번호
        public int enemyidx;//싸워야할 상대
        public bool IsBattleMove;//이동해야하는지 체크
        public bool IsCopy;//내가 복사본을 보내는지 체크
        public int copyidx;//싸워야할 상대(복사본을 보내는곳)
        
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

        public Transform TextUIparent;

        public List<string> Cards;
        public List<GameObject> Card_1;
        public List<GameObject> Card_2;
        public List<GameObject> Card_3;
        public List<GameObject> Card_4;
        public List<GameObject> Card_5;


        public List<GameObject> PickCard;

        public List<GameObject> pickNoMove;
        //public bool IsBattle = false;

        [Header("UI정보들")] public Transform ItemCanvs;
        public Transform ItemParent;
        public GridLayoutGroup ItemGridLayout;

        [Header("전투 플레이어")] 
        public List<BattleInfo> battleinfos;

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


    }
}