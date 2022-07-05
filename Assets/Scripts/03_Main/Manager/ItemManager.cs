using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameS
{
    public class ItemManager : MonoBehaviour
    {
        public static ItemManager inst;

        public GameObject itemPrefab;
        
        private void Awake()
        {
            inst = this;
        }

        public void ItemAdd(int idx)
        {
            GameObject item = Instantiate(itemPrefab, GameSystem_AllInfo.inst.ItemParent);
            if (item.TryGetComponent(out ItemDraggable drag))
            {
               
                drag.Startfunc(idx);
            }
        }

        private void Update()
        {
            //test
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ItemAdd(Random.Range(0,11));
            }
        }
        public bool ItemCheck(GameObject ob,int Idx)
        {
            bool b = false;
            if (Idx<=8)
            {
                if (ob.TryGetComponent(out Card_Info info))
                {
               
                    for (int i = 0; i < 3; i++)
                    {
                        if (info.Item[i]==-1)
                        {
                            b = true;
                            break;
                        }
                        else if (info.Item[i]>=0 &&info.Item[i]<=8)
                        {
                            b = true;
                            break;
                        }
                    }
                }
            }
            else
            {
                if (ob.TryGetComponent(out Card_Info info))
                {
               
                    for (int i = 0; i < 3; i++)
                    {
                        if (info.Item[i]==-1)
                        {
                            b = true;
                            break;
                        }
                    }
                }
            }
            

            return b;
        }


        #region 아이템 확인 

                public int ItemMixIdx(int idx1, int idx2)
        {
            int result = 0;
            int x1 = idx1;
            int x2 = idx2;


            if (idx1>idx2)
            {
                x1 = idx2;
                x2 = idx1;
            }

            if (x1==0)
            {
                result = ItemMix0(x2);
            }
            else if (x1==1)
            {
                result = ItemMix1(x2);
            }
            else if (x1==2)
            {
                result = ItemMix2(x2);
            }
            else if (x1==3)
            {
                result = ItemMix3(x2);
            }
            else if (x1==4)
            {
                result = ItemMix4(x2);
            }
            else if (x1==5)
            {
                result = ItemMix5(x2);
            }
            else if (x1==6)
            {
                result = ItemMix6(x2);
            }
            else if (x1==7)
            {
                result = ItemMix7(x2);
            }
            else if (x1==8)
            {
                result = ItemMix8(x2);
            }
            
            return result;
        }

        int ItemMix0(int idx)
        {
            switch (idx)
            {
                case 0:
                    return 9;
                case 1:
                    return 10;
                case 2:
                    return 11;
                case 3:
                    return 12;
                case 4:
                    return 13;
                case 5:
                    return 14;
                case 6:
                    return 15;
                case 7:
                    return 16;
                case 8:
                    return 17;
                default:
                    break;
            }

            return 9;
        }
        int ItemMix1(int idx)
        {
            switch (idx)
            {
                case 1:
                    return 18;
                case 2:
                    return 19;
                case 3:
                    return 20;
                case 4:
                    return 21;
                case 5:
                    return 22;
                case 6:
                    return 23;
                case 7:
                    return 24;
                case 8:
                    return 25;
                default:
                    break;
            }

            return 9;
        }
        int ItemMix2(int idx)
        {
            switch (idx)
            {
                case 2:
                    return 26;
                case 3:
                    return 27;
                case 4:
                    return 28;
                case 5:
                    return 29;
                case 6:
                    return 30;
                case 7:
                    return 31;
                case 8:
                    return 32;
                default:
                    break;
            }

            return 9;
        }
        int ItemMix3(int idx)
        {
            switch (idx)
            {
                case 3:
                    return 33;
                case 4:
                    return 34;
                case 5:
                    return 35;
                case 6:
                    return 36;
                case 7:
                    return 37;
                case 8:
                    return 38;
                default:
                    break;
            }

            return 9;
        }
        int ItemMix4(int idx)
        {
            switch (idx)
            {
                case 4:
                    return 39;
                case 5:
                    return 40;
                case 6:
                    return 41;
                case 7:
                    return 42;
                case 8:
                    return 43;
                default:
                    break;
            }

            return 9;
        }
        int ItemMix5(int idx)
        {
            switch (idx)
            {
                case 5:
                    return 44;
                case 6:
                    return 45;
                case 7:
                    return 46;
                case 8:
                    return 47;
                default:
                    break;
            }

            return 9;
        }
        int ItemMix6(int idx)
        {
            switch (idx)
            {
                case 6:
                    return 48;
                case 7:
                    return 49;
                case 8:
                    return 50;
                default:
                    break;
            }

            return 9;
        }
        int ItemMix7(int idx)
        {
            switch (idx)
            {

                case 7:
                    return 51;
                case 8:
                    return 52;
                default:
                    break;
            }

            return 9;
        }
        int ItemMix8(int idx)
        {

            return 53;
        }

        #endregion

    }
}
