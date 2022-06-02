using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameS
{
    public class ItemManager : MonoBehaviour
    {
        public static ItemManager inst;

        private void Awake()
        {
            inst = this;
        }

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
            Debug.Log(result);
            return result;
        }

        int ItemMix0(int idx)
        {
            switch (idx)
            {
                case 0:
                    return 9;
                    break;
                case 1:
                    return 10;
                    break;
                case 2:
                    return 11;
                    break;
                case 3:
                    return 12;
                    break;
                case 4:
                    return 13;
                    break;
                case 5:
                    return 14;
                    break;
                case 6:
                    return 15;
                    break;
                case 7:
                    return 16;
                    break;
                case 8:
                    return 17;
                    break;
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
                    break;
                case 2:
                    return 19;
                    break;
                case 3:
                    return 20;
                    break;
                case 4:
                    return 21;
                    break;
                case 5:
                    return 22;
                    break;
                case 6:
                    return 23;
                    break;
                case 7:
                    return 24;
                    break;
                case 8:
                    return 25;
                    break;
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
                    break;
                case 3:
                    return 27;
                    break;
                case 4:
                    return 28;
                    break;
                case 5:
                    return 29;
                    break;
                case 6:
                    return 30;
                    break;
                case 7:
                    return 31;
                    break;
                case 8:
                    return 32;
                    break;
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
                    break;
                case 4:
                    return 34;
                    break;
                case 5:
                    return 35;
                    break;
                case 6:
                    return 36;
                    break;
                case 7:
                    return 37;
                    break;
                case 8:
                    return 38;
                    break;
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
                    break;
                case 5:
                    return 40;
                    break;
                case 6:
                    return 41;
                    break;
                case 7:
                    return 42;
                    break;
                case 8:
                    return 43;
                    break;
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
                    break;
                case 6:
                    return 45;
                    break;
                case 7:
                    return 46;
                    break;
                case 8:
                    return 47;
                    break;
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
                    break;
                case 7:
                    return 49;
                    break;
                case 8:
                    return 50;
                    break;
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
                    break;
                case 8:
                    return 52;
                    break;
                default:
                    break;
            }

            return 9;
        }
        int ItemMix8(int idx)
        {

            return 53;
        }
    }
}
